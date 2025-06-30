using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonopolyWinForms.Services;
using MonopolyWinForms.GameLogic;
using Newtonsoft.Json;
using MonopolyWinForms.Room;
using System.Net.Http;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace MonopolyWinForms.Services
{
    public class GameManager
    {
        public static bool IsGameStarted { get; private set; }
        public static string? CurrentRoomId { get; private set; }
        public static int PlayTime { get; private set; }
        public static List<string>? Players { get; private set; }

        private static FirebaseService _firebase = new();
        private static System.Windows.Forms.Timer? _syncTimer;
        private static System.Windows.Forms.Timer? _chatTimer;
        private static DateTime _lastStateTime = DateTime.MinValue;
        private static DateTime _lastChatTime = DateTime.MinValue;

        public static event Action<GameState>? OnGameStateUpdated;
        public static event Action<string, string>? OnChatMessageReceived;
        public static event Action<string>? OnPlayerLeft;

        public static void StartGame(string roomId, List<string> players, int playtime)
        {
            IsGameStarted = true;
            CurrentRoomId = roomId;
            Players = players;
            PlayTime = playtime;

            InitTimers();
        }
        private static void InitTimers()
        {
            _syncTimer = new System.Windows.Forms.Timer { Interval = 500 };
            _syncTimer.Tick += async (_, __) => await SyncGameState();
            _syncTimer.Start();

            _chatTimer = new System.Windows.Forms.Timer { Interval = 500 };
            _chatTimer.Tick += async (_, __) => await SyncChat();
            _chatTimer.Start();

            _ = LoadInitialChat();
        }
        public static void EndGame()
        {
            IsGameStarted = false;
            CurrentRoomId = null;
            Players = null;
            _syncTimer?.Stop();
            _chatTimer?.Stop();
            OnGameStateUpdated = null;
            OnChatMessageReceived = null;
        }
        private static async Task SyncGameState()
        {
            if (!IsGameStarted || string.IsNullOrEmpty(CurrentRoomId)) return;
            var state = await _firebase.GetGameStateAsync(CurrentRoomId);
            if (state == null) return;
            if (state.LastUpdateTime > _lastStateTime)
            {
                _lastStateTime = state.LastUpdateTime;
                OnGameStateUpdated?.Invoke(state);
            }
        }
        private static async Task LoadInitialChat()
        {
            if (string.IsNullOrEmpty(CurrentRoomId)) return;
            var msgs = await _firebase.GetChatMessagesAsync(CurrentRoomId);
            if (msgs == null) return;
            foreach (var m in msgs)
                OnChatMessageReceived?.Invoke(m.SenderName, m.Message);
            if (msgs.Count > 0)
                _lastChatTime = DateTime.Parse(msgs[^1].Timestamp, null, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }
        private static async Task SyncChat()
        {
            if (!IsGameStarted || string.IsNullOrEmpty(CurrentRoomId)) return;
            var msgs = await _firebase.GetChatMessagesAsync(CurrentRoomId);
            if (msgs == null) return;
            foreach (var m in msgs)
            {
                var t = DateTime.Parse(m.Timestamp, null, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                if (t > _lastChatTime)
                {
                    OnChatMessageReceived?.Invoke(m.SenderName, m.Message);
                    _lastChatTime = t;
                }
            }
        }
        public static Task UpdateGameState(GameState gs) => _firebase.UpdateGameStateAsync(gs);
        public static Task<GameState?> GetLatestGameState() => _firebase.GetGameStateAsync(CurrentRoomId!);
        public static Task SendChatMessage(string roomId, string sender, string message) =>
            _firebase.SendChatMessageAsync(roomId, new FirebaseService.ChatMessage { SenderName = sender, Message = message, Timestamp = DateTime.UtcNow.ToString("O") });

        public static async Task CleanupGameData(string roomId)
        {
            try
            {
                await _firebase.CleanupGameDataAsync(roomId);
                EndGame();
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error in GameManager.CleanupGameData: {ex.Message}\n");
            }
        }
        public static Task<RoomInfo?> GetRoomAsync(string roomId) => _firebase.GetRoomAsync(roomId);
        public static Task UpdateRoomAsync(string roomId, RoomInfo r) => _firebase.CreateRoomAsync(roomId, r);
        public static async Task NotifyPlayerLeft(string roomId, string playerName)
        {
            if (!IsGameStarted) return;
            IsGameStarted = false;
            await SendChatMessage(roomId, "Hệ thống", $"{playerName} đã thoát game. Trò chơi kết thúc!");
            OnPlayerLeft?.Invoke(playerName);
            await Task.Delay(500);
            await _firebase.CleanupGameDataAsync(roomId);
            EndGame();
        }
    }
}