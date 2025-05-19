using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MonopolyWinForms.GameLogic
{
    public class Tile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? PlayerId { get; set; }
        public int LandPrice { get; set; }
        public int HousePrice { get; set; }
        public int HotelPrice { get; set; }
        public int Level { get; set; }
        public string Monopoly { get; set; } = string.Empty;
        public int RentPrice { get; set; }
        public Tile() { }
        public Tile(int id, string name, int playerId, int landPrice, int housePrice, int hotelPrice, int level, string monopoly)
        {
            Id = id;
            Name = name;
            PlayerId = playerId;
            LandPrice = landPrice;
            HousePrice = housePrice;
            HotelPrice = hotelPrice;
            Level = level;
            Monopoly = monopoly;
        }
        public static List<Tile> LoadTilesFromFile()
        {
            var tiles = new List<Tile>();
            // Đường dẫn đến file Tiles.txt trong thư mục của ứng dụng
            string filePath = Path.Combine(Application.StartupPath, "Tiles.txt");
            if (!File.Exists(filePath))
            {
                return tiles; // Trả về danh sách trống nếu file không tồn tại
            }
            // Đọc tất cả dòng trong file vào một mảng
            string[] lines = File.ReadAllLines(filePath);
            // Duyệt qua từng dòng trong mảng lines
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length < 8) // Kiểm tra xem có ít hơn 8 phần tử không, nếu có thì thử dấu ';'
                {
                    parts = line.Split(';');
                }
                if (parts.Length < 8)
                {
                    continue;
                }
                var tile = new Tile
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1].Replace("\"", ""),
                    LandPrice = int.Parse(parts[3]),
                    HousePrice = int.Parse(parts[4]),
                    HotelPrice = int.Parse(parts[5]),
                    Level = int.Parse(parts[6]),
                    Monopoly = parts[7].Trim()
                };
                // Xử lý PlayerId (kiểm tra "null" và chuyển thành int? hoặc null)
                tile.PlayerId = string.IsNullOrWhiteSpace(parts[2]) || parts[2].ToLower() == "null"
                    ? (int?)null
                    : int.TryParse(parts[2], out int playerId) ? playerId : (int?)null;

                tiles.Add(tile);
            }
            return tiles;
        }
        public void DestroyOneHouseLevel()
        {
            if (Level > 0)
            {
                Level--;
                if (Level == 0)
                {
                    PlayerId = null;
                }
            }
        }
        public int SellLandAndHouses()
        {
            int value = LandPrice;
            if (Level >= 1 && Level <= 4)
                value += HousePrice * Level;
            else if (Level == 5)
                value += HotelPrice;

            Level = 0;
            PlayerId = null;
            return value;
        }
    }
}
