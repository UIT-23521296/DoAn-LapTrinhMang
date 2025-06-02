using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonopolyWinForms.Services;
using MonopolyWinForms.GameLogic;
using Newtonsoft.Json;
using System.Net.Http;

namespace MonopolyWinForms.Services
{
    public class GameManager
    {
        public static bool IsGameStarted { get; private set; }
        public static string CurrentRoomId { get; private set; }
        public static List<string> Players { get; private set; }
        private static FirebaseService firebase;
        private static System.Windows.Forms.Timer syncTimer;
        private static System.Windows.Forms.Timer chatSyncTimer;
        private static DateTime lastUpdateTime = DateTime.MinValue;
        private static DateTime lastChatUpdateTime = DateTime.MinValue;
        public static event Action<GameState> OnGameStateUpdated;
        public static event Action<string, string> OnChatMessageReceived;

        public static void StartGame(string roomId, List<string> players)
        {
            try
            {
                File.AppendAllText("log.txt", $"StartGame called with roomId: {roomId}\n");
                IsGameStarted = true;
                CurrentRoomId = roomId;
                Players = players;
                firebase = new FirebaseService();
                File.AppendAllText("log.txt", $"Firebase initialized\n");
                InitializeSyncTimer();
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"❌ Exception in StartGame: {ex.Message}\n");
            }
        }

        private static void InitializeSyncTimer()
        {
            // Timer cho game state
            syncTimer = new System.Windows.Forms.Timer();
            syncTimer.Interval = 100;
            syncTimer.Tick += SyncTimer_Tick;
            syncTimer.Start();

            // Timer cho chat
            chatSyncTimer = new System.Windows.Forms.Timer();
            chatSyncTimer.Interval = 500; // Kiểm tra chat mỗi 500ms
            chatSyncTimer.Tick += ChatSyncTimer_Tick;
            chatSyncTimer.Start();

            // Khởi tạo chat listener
            _ = InitializeChatListener();
        }

        private static async Task InitializeChatListener()
        {
            try
            {
                var chatMessages = await firebase.GetChatMessagesAsync(CurrentRoomId);
                if (chatMessages != null)
                {
                    foreach (var message in chatMessages)
                    {
                        OnChatMessageReceived?.Invoke(
                            message.Value.SenderName.ToString(),
                            message.Value.Message.ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error initializing chat listener: {ex.Message}\n");
            }
        }

        private static async void SyncTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!IsGameStarted || string.IsNullOrEmpty(CurrentRoomId))
                {
                    return;
                }

                var gameState = await firebase.GetGameStateAsync(CurrentRoomId);
                if (gameState != null && gameState.LastUpdateTime > lastUpdateTime)
                {
                    lastUpdateTime = gameState.LastUpdateTime;
                    OnGameStateUpdated?.Invoke(gameState);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error in SyncTimer_Tick: {ex.Message}\n");
            }
        }

        private static async void ChatSyncTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!IsGameStarted || string.IsNullOrEmpty(CurrentRoomId))
                {
                    return;
                }

                var chatMessages = await firebase.GetChatMessagesAsync(CurrentRoomId);
                if (chatMessages != null)
                {
                    foreach (var message in chatMessages)
                    {
                        // Chỉ gửi tin nhắn mới
                        var messageTime = DateTime.Parse(message.Value.Timestamp.ToString());
                        if (messageTime > lastChatUpdateTime)
                        {
                            OnChatMessageReceived?.Invoke(
                                message.Value.SenderName.ToString(),
                                message.Value.Message.ToString()
                            );
                            lastChatUpdateTime = messageTime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error in ChatSyncTimer_Tick: {ex.Message}\n");
            }
        }

        public static async Task UpdateGameState(GameState gameState)
        {
            try
            {
                File.AppendAllText("log.txt", $"Game manager ở đây\n");
                await firebase.UpdateGameStateAsync(gameState);
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error in GameManager.UpdateGameState: {ex.Message}\n");
            }
        }
        
        public static void EndGame()
        {
            IsGameStarted = false;
            CurrentRoomId = null;
            Players = null;
            syncTimer?.Stop();
            chatSyncTimer?.Stop(); // Dừng timer chat
            OnGameStateUpdated = null;
            OnChatMessageReceived = null;
        }

        public static async Task SendChatMessage(string roomId, object chatMessage)
        {
            try
            {
                await firebase.SendChatMessageAsync(roomId, chatMessage);
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error sending chat message: {ex.Message}\n");
            }
        }
    }
}
