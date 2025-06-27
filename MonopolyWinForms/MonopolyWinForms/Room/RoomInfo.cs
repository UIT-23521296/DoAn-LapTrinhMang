using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

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
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o"); // ISO 8601

        [JsonIgnore]
        public DateTime CreatedAtDateTime
        {
            get
            {
                if (DateTime.TryParse(CreatedAt, null, DateTimeStyles.RoundtripKind, out var dt))
                    return dt;
                if (DateTime.TryParse(CreatedAt, out dt))
                    return dt;
                return DateTime.MinValue;
            }
        }

        public int CurrentPlayers => PlayerDisplayNames?.Count ?? 0;

        public override string ToString()
        {
            return $"{RoomName} [{CurrentPlayers}/{MaxPlayers}] - {HostIP}:{Port}" + (IsStarted ? " (Đang chơi)" : "");
        }

    }
}
