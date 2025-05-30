using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    public class Tiles
    {
        public string Name { get; set; }
        public string Price { get; set; }

        public Tiles(string name, string price)
        {
            Name = name;
            Price = price;
        }
    }

    public class Board
    {
        public List<Tiles> TileList { get; set; } = new List<Tiles>();

        // Đọc thông tin từ file và khởi tạo các ô
        public Board(string filePath)
        {
            LoadTiles(filePath);
        }

        // Đọc dữ liệu từ file
        public void LoadTiles(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File không tồn tại!");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');

                if (parts.Length >= 4)
                {
                    string name = parts[1].Trim().Trim('"', '“', '”');
                    string price = parts[3].Trim();

                    // Nếu giá = NULL hoặc 0 thì không hiển thị giá
                    if (price == "0" || string.IsNullOrEmpty(price))
                    {
                        price = "";
                    }

                    TileList.Add(new Tiles(name, price != "" ? "$" + price : ""));
                }
            }
        }

        // Hàm vẽ bàn cờ lên form
        public void GenerateBoard(Form form, List<Tiles> tiles)
        {
            int count = 0;

            int cornerSize = 150;
            int horizontalTileWidth = 75;
            int horizontalTileHeight = 150;
            int verticalTileWidth = 150;
            int verticalTileHeight = 75;

            int boardSize = 11;
            int totalWidth = 2 * cornerSize + (boardSize - 3) * horizontalTileWidth;
            int totalHeight = totalWidth;

            int offsetX = 0;
            int offsetY = 0;

            // Điểm bắt đầu là góc trên trái
            int x = offsetX;
            int y = offsetY;

            // 1. Góc trên trái
            AddTileFromList(tiles, ref count, x, y, cornerSize, cornerSize, form);

            // 2. Trên cùng (trái -> phải)
            x += cornerSize;
            for (int i = 0; i < boardSize - 2; i++)
            {
                AddTileFromList(tiles, ref count, x, y, horizontalTileWidth, horizontalTileHeight, form);
                x += horizontalTileWidth;
            }

            // 3. Góc trên phải
            AddTileFromList(tiles, ref count, x, y, cornerSize, cornerSize, form);

            // 4. Bên phải (trên -> dưới)
            y += cornerSize;
            for (int i = 0; i < boardSize - 2; i++)
            {
                AddTileFromList(tiles, ref count, x, y, verticalTileWidth, verticalTileHeight, form);
                y += verticalTileHeight;
            }

            // 5. Góc dưới phải
            AddTileFromList(tiles, ref count, x, y, cornerSize, cornerSize, form);

            // 6. Dưới cùng (phải -> trái)
            x -= horizontalTileWidth;
            for (int i = 0; i < boardSize - 2; i++)
            {
                AddTileFromList(tiles, ref count, x, y, horizontalTileWidth, horizontalTileHeight, form);
                x -= horizontalTileWidth;
            }

            // 7. Góc dưới trái
            AddTileFromList(tiles, ref count, 0, y, cornerSize, cornerSize, form);

            // 8. Bên trái (dưới -> lên)
            y -= verticalTileHeight;
            for (int i = 0; i < boardSize - 2; i++)
            {
                AddTileFromList(tiles, ref count, 0, y, verticalTileWidth, verticalTileHeight, form);
                y -= verticalTileHeight;
            }
        }





        private void AddTileFromList(List<Tiles> tiles, ref int count, int x, int y, int width, int height, Form form)
        {
            if (count >= tiles.Count) return;
            var tileData = tiles[count++];

            Panel panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.Beige
            };

            Label lblName = new Label
            {
                Text = tileData.Name,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = height / 2
            };

            Label lblPrice = new Label
            {
                Text = tileData.Price.ToString(),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = height / 2
            };

            panel.Controls.Add(lblName);
            panel.Controls.Add(lblPrice);
            form.Controls.Add(panel);
        }


    }
}
