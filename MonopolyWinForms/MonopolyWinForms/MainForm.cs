using buyLand_Home;
using MonopolyWinForms.GameLogic;

namespace MonopolyWinForms
{
    public partial class MainForm : Form
    {
        private List<Tile> tiles;       // Danh sách các ô đất
        private Panel[] panels;         // Mảng các Panel tương ứng với các ô
        private int playerID = 1;

        public MainForm()
        {
            InitializeComponent();

            // Gán các panel đã được tạo từ Designer vào mảng panels
            panels = new Panel[]
            {
                panel1, panel2, panel3, panel4, panel5,
                panel6, panel7, panel8, panel9, panel10,
                panel11, panel12, panel13, panel14, panel15,
                panel16, panel17, panel18, panel19, panel20,
                panel21, panel22, panel23, panel24, panel25,
                panel26, panel27, panel28, panel29, panel30,
                panel31, panel32, panel33, panel34, panel35,
                panel36, panel37, panel38, panel39, panel40
            };

            // Tải dữ liệu các ô đất từ file
            tiles = Tile.LoadTilesFromFile(); // Gọi phương thức không cần truyền đường dẫn
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Gán dữ liệu Tile vào từng panel
            for (int i = 0; i < panels.Length && i < tiles.Count; i++)
            {
                panels[i].Tag = tiles[i];          // Gán dữ liệu Tile
                panels[i].Click += Panel_Click;    // Gắn sự kiện click
                panels[i].BackColor = Color.LightYellow;
                UpdateTileDisplay(i);              // Hiển thị thông tin ban đầu
            }
        }

        private void Panel_Click(object? sender, EventArgs e)
        {
            if (sender is Panel panel && panel.Tag is Tile tile)
            {
                int index = Array.IndexOf(panels, panel); // Xác định chỉ số ô

                int rentPrice = 0;
                if (tile.Level == 1)
                    rentPrice = tile.LandPrice;
                else if (tile.Level >= 2 && tile.Level <= 4)
                    rentPrice = tile.LandPrice + tile.HousePrice * (tile.Level - 1);
                else if (tile.Level == 5)
                    rentPrice = tile.LandPrice + tile.HousePrice * 3 + tile.HotelPrice;

                // Nếu ô đất chưa có chủ hoặc thuộc về người chơi, và chưa đạt cấp tối đa
                if ((tile.PlayerId == null || tile.PlayerId == playerID) && tile.Level < 5)
                {
                    BuyHome_Land buyHomeLandForm = new BuyHome_Land(playerID, tile);
                    buyHomeLandForm.ShowDialog();

                    // Cập nhật lại thông tin trên panel sau khi form đóng
                    UpdateTileDisplay(index);
                }
                else
                {
                    MessageBox.Show($"Bạn đang ở ô: {tile.Name}, giá thuê: ${rentPrice / 2}, chủ sở hữu: Player {tile.PlayerId}");
                }
            }
        }

        // Hàm cập nhật hiển thị label trên từng panel
        private void UpdateTileDisplay(int index)
        {
            if (index < 0 || index >= panels.Length || index >= tiles.Count)
                return;

            var tile = tiles[index];
            int rentPrice = 0;

            if (tile.Level == 1)
                rentPrice = tile.LandPrice;
            else if (tile.Level >= 2 && tile.Level <= 4)
                rentPrice = tile.LandPrice + tile.HousePrice * (tile.Level - 1);
            else if (tile.Level == 5)
                rentPrice = tile.LandPrice + tile.HousePrice * 3 + tile.HotelPrice;

            // Xóa label cũ trước khi thêm cái mới
            panels[index].Controls.Clear();

            panels[index].Controls.Add(new Label
            {
                Text = $"{tile.Name}\n${rentPrice/2}",
                AutoSize = true,
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(5, 5)
            });
        }
    }
}
