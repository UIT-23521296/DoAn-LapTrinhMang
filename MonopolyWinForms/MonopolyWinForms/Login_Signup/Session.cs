using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonopolyWinForms.Services;

namespace MonopolyWinForms.Login_Signup
{
    class Session
    {
        private static FirebaseService _firebase = new FirebaseService();
        
        public static int PlayerInGameId { get; set; }
        public static string UserId { get; private set; }
        public static Color Color { get; set; }
        public static string UserName { get; private set; }
        public static bool IsLoggedIn { get; private set; }
        public static string CurrentRoomId { get; private set; }  // Theo dõi phòng hiện tại
        public static bool IsHost { get; private set; }           // Người dùng có phải chủ phòng không

        public static async Task StartSession(string userId, string userName)
        {
            try
            {
                UserId = userId;
                UserName = userName;
                IsLoggedIn = true;
                PlayerInGameId = 0;
                CurrentRoomId = null;
                IsHost = false;

                // Tạo session trên Firebase
                var sessionData = new
                {
                    userId = userId,
                    userName = userName,
                    isActive = true,
                    lastActive = DateTime.UtcNow,
                    currentRoomId = (string)null,
                    isHost = false,
                    playerInGameId = 0
                };

                await _firebase.CreateSessionAsync(userId, sessionData);
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error starting session: {ex.Message}\n");
                throw;
            }
        }

        public static async Task JoinRoom(string roomId, bool isHost = false)
        {
            try
            {
                CurrentRoomId = roomId;
                IsHost = isHost;

                // Cập nhật session trên Firebase
                var sessionData = new
                {
                    currentRoomId = roomId,
                    isHost = isHost,
                    lastActive = DateTime.UtcNow
                };

                await _firebase.UpdateSessionAsync(UserId, sessionData);
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error joining room: {ex.Message}\n");
                throw;
            }
        }

        public static async Task LeaveRoom()
        {
            try
            {
                // Cập nhật session trên Firebase
                var sessionData = new
                {
                    currentRoomId = (string)null,
                    isHost = false,
                    lastActive = DateTime.UtcNow
                };

                await _firebase.UpdateSessionAsync(UserId, sessionData);

                // Reset local state
                CurrentRoomId = null;
                IsHost = false;
                PlayerInGameId = 0;
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error leaving room: {ex.Message}\n");
                throw;
            }
        }

        public static async Task EndSession()
        {
            try
            {
                if (IsLoggedIn)
                {
                    // Xóa session trên Firebase
                    await _firebase.DeleteSessionAsync(UserId);

                    // Reset local state
                    UserId = null;
                    UserName = null;
                    IsLoggedIn = false;
                    CurrentRoomId = null;
                    IsHost = false;
                    PlayerInGameId = 0;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error ending session: {ex.Message}\n");
                throw;
            }
        }

        // Kiểm tra session có còn hợp lệ không
        public static async Task<bool> ValidateSession()
        {
            if (!IsLoggedIn || string.IsNullOrEmpty(UserId))
                return false;

            return await _firebase.ValidateSessionAsync(UserId);
        }
    }
}
