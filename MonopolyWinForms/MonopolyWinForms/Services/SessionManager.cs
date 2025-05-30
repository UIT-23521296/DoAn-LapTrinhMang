using MonopolyWinForms.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.Services
{
    class SessionManager
    {
        public static string CurrentUserId { get; set; }
        public static string CurrentUserDisplayName { get; set; }
        public static RoomInfo CurrentRoom { get; set; }
    }
}
