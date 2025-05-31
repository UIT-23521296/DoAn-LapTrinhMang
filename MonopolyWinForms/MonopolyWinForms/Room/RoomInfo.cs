using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.Room
{
    public class RoomInfo
    {
        public string RoomId { get; set; } = Guid.NewGuid().ToString();
        public string RoomName { get; set; }
        public string HostId { get; set; }
        public string HostIP { get; set; }
        public int Port { get; set; }
        public int MaxPlayers { get; set; }
        public int PlayTime { get; set; }
        public List<string> PlayerIds { get; set; } = new List<string>();
        public List<string> PlayerDisplayNames { get; set; } = new List<string>();
        public List<string> ReadyPlayers { get; set; } = new List<string>();
        public bool IsStarted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CurrentPlayers => PlayerDisplayNames?.Count ?? 0;

        public override string ToString()
        {
            return $"{RoomName} [{CurrentPlayers}/{MaxPlayers}] - {HostIP}:{Port}" + (IsStarted ? " (Đang chơi)" : "");
        }

    }
}
