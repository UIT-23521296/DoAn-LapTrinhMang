using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.Login_Signup
{
    class Session
    {
        public static string UserId { get; private set; }
        public static string UserName { get; private set; }
        public static bool IsLoggedIn { get; private set; }
        public static string CurrentRoomId { get; private set; }  // Theo dõi phòng hiện tại
        public static bool IsHost { get; private set; }           // Người dùng có phải chủ phòng không

        public static void StartSession(string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
            IsLoggedIn = true;
            CurrentRoomId = null;
            IsHost = false;
        }

        public static void JoinRoom(string roomId, bool isHost = false)
        {
            CurrentRoomId = roomId;
            IsHost = isHost;
        }

        public static void LeaveRoom()
        {
            CurrentRoomId = null;
            IsHost = false;
        }

        public static void EndSession()
        {
            UserId = null;
            UserName = null;
            IsLoggedIn = false;
            CurrentRoomId = null;
            IsHost = false;
        }
    }
}
