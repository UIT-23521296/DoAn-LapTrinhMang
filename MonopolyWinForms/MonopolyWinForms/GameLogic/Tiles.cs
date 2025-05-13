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

        // Constructor mặc định
        public Tile() { }

        // Constructor đầy đủ tham số
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

        // Hàm static để load từ file txt trong thư mục của ứng dụng
        public static List<Tile> LoadTilesFromFile()
        {
            var tiles = new List<Tile>();

            // Đường dẫn đến file Tiles.txt trong thư mục của ứng dụng
            string filePath = Path.Combine(Application.StartupPath, "Tiles.txt");

            // Kiểm tra nếu file không tồn tại
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Không tìm thấy file Tiles.txt!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    continue; // Bỏ qua dòng này nếu không đủ phần tử
                }

                var tile = new Tile
                {
                    Id = int.Parse(parts[0]),        // ID
                    Name = parts[1],                 // Tên ô đất
                    LandPrice = int.Parse(parts[3]), // Giá đất
                    HousePrice = int.Parse(parts[4]), // Giá nhà
                    HotelPrice = int.Parse(parts[5]), // Giá khách sạn
                    Level = int.Parse(parts[6]),     // Cấp độ
                    Monopoly = parts[7].Trim()           // Nhóm màu
                };

                // Xử lý PlayerId (kiểm tra "null" và chuyển thành int? hoặc null)
                tile.PlayerId = string.IsNullOrWhiteSpace(parts[2]) || parts[2].ToLower() == "null"
                    ? (int?)null
                    : int.TryParse(parts[2], out int playerId) ? playerId : (int?)null;

                tiles.Add(tile);
            }

            return tiles;
        }
    }
}
