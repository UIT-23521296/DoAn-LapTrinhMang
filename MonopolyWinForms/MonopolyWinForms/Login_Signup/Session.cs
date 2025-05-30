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

        public static void StartSession(string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
            IsLoggedIn = true;
        }

        public static void EndSession()
        {
            UserId = null;
            UserName = null;
            IsLoggedIn = false;
        }
    }
}
