using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonopolyWinForms.GameLogic;
using MonopolyWinForms.Login_Signup;
namespace MonopolyWinForms.Services
{
    public class GameState
    {
        public string RoomId { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public List<Player> Players { get; set; }
        public List<Tile> Tiles { get; set; }
        public bool IsGameStarted { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public GameState()
        {
            Players = new List<Player>();
            Tiles = new List<Tile>();
            LastUpdateTime = DateTime.UtcNow;
        }

        public GameState(string roomId, int currentPlayerIndex, List<Player> players, List<Tile> tiles)
        {
            RoomId = roomId;
            CurrentPlayerIndex = currentPlayerIndex;
            Players = players;
            Tiles = tiles;
            IsGameStarted = true;
            LastUpdateTime = DateTime.UtcNow;
        }
    }
}
