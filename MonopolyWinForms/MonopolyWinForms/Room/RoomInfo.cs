using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.Room
{
    public class RoomInfo
    {
        public string RoomName { get; set; }
        public string HostIP { get; set; }
        public int Port { get; set; }
        public int CurrentPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int PlayTime { get; set; }
    }
}
