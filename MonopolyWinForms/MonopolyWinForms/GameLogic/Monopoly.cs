using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    class Monopoly
    {
        private List<Tile> tiles;
        public Monopoly(List<Tile> tiles)
        {
            this.tiles = tiles;
        }
        // Trả về số ô đất mà người chơi sở hữu trong nhóm MonoGroup cụ thể
        public int CountOwnedTilesInGroup(int playerId, string monoGroup)
        {
            return tiles.Count(t => t.PlayerId == playerId && t.Monopoly == monoGroup);
        }
        // Trả về tổng số ô trong nhóm MonoGroup
        public int TotalTilesInGroup(string monoGroup)
        {
            return tiles.Count(t => t.Monopoly == monoGroup);
        }
        // Kiểm tra người chơi có sở hữu toàn bộ nhóm hay không
        public bool HasFullMonopoly(int playerId, string monoGroup)
        {
            int owned = CountOwnedTilesInGroup(playerId, monoGroup);
            int total = TotalTilesInGroup(monoGroup);
            return owned == total && total > 0;
        }
        // Số monopoly mà người chơi đã sở hữu toàn bộ
        public int GetAllFullMonopolyGroups(int playerId)
        {
            var groups = tiles.Select(t => t.Monopoly).Distinct();
            return groups.Count(g => HasFullMonopoly(playerId, g));
        }
        // Đếm số lượng xe buýt mà người chơi sở hữu trong nhóm Monopoly
        public int CountBusesOwned(int playerId)
        {
            // Lọc các ô đất thuộc nhóm "Bus" mà người chơi sở hữu (PlayerId == playerId)
            return tiles.Count(t => t.PlayerId == playerId && t.Monopoly == "9");
        }

    }
}
