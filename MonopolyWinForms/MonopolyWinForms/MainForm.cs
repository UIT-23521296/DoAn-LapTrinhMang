using buyLand_Home;
using MonopolyWinForms.BuyLand_Home;
using MonopolyWinForms.GameLogic;

namespace MonopolyWinForms
{
    public partial class MainForm : Form
    {
        private List<Tile> tiles;       // Danh sách các ô đất
        private Panel[] panels;         // Mảng các Panel tương ứng với các ô
        private Monopoly? monopoly;

        private int playerID = 1;       // Cái này để thử

        private readonly Dictionary<string, Color> monopolyColors = new()
        {
            ["1"] = Color.Violet,
            ["2"] = Color.Yellow,
            ["3"] = Color.DodgerBlue,
            ["4"] = Color.LightGreen,
            ["5"] = Color.Red,
            ["6"] = Color.Pink,
            ["7"] = Color.Purple,
            ["8"] = Color.Gray
        };
        public MainForm()
        {
            InitializeComponent();
            panels = new Panel[] {
            panel1, panel2, panel3, panel4, panel5,
            panel6, panel7, panel8, panel9, panel10,
            panel11, panel12, panel13, panel14, panel15,
            panel16, panel17, panel18, panel19, panel20,
            panel21, panel22, panel23, panel24, panel25,
            panel26, panel27, panel28, panel29, panel30,
            panel31, panel32, panel33, panel34, panel35,
            panel36, panel37, panel38, panel39, panel40
            };
            tiles = Tile.LoadTilesFromFile();
            monopoly = new Monopoly(tiles);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Gán dữ liệu Tile vào từng panel
            for (int i = 0; i < panels.Length && i < tiles.Count; i++)
            {
                panels[i].Tag = tiles[i];          // Gán dữ liệu Tile vào Tag của panel
                AttachClickEventRecursively(panels[i]);    // cái này chỉ để test
                UpdateTileDisplay(i);              // Hiển thị thông tin ban đầu
            }
        }
        // cái này để test chạy thoi
        private void Panel_Click(object? sender, EventArgs e)
        {
            Control? current = sender as Control;
            // Dò lên đến khi tìm thấy Panel chứa Tile trong Tag
            while (current != null && (current.Tag == null || !(current.Tag is Tile)))
            {
                current = current.Parent;
            }
            if (current is Panel panel && panel.Tag is Tile tile)
            {
                int index = Array.IndexOf(panels, panel);
                if (index == -1)
                    return;
                switch (tile.Monopoly)
                {
                    case "0":
                        return;

                    case "9":
                        new BuyBus(playerID, tile, tiles).ShowDialog();
                        UpdateTileDisplay(index);
                        return;

                    case "10":
                        new BuyCompany(playerID, tile).ShowDialog();
                        UpdateTileDisplay(index);
                        return;
                }
                // Các ô đất thông thường (monopoly từ 1–8)
                int rentPrice = tile.Level switch
                {
                    1 => tile.LandPrice,
                    >= 2 and <= 4 => tile.LandPrice + tile.HousePrice * (tile.Level - 1),
                    5 => tile.LandPrice + tile.HousePrice * 3 + tile.HotelPrice,
                    _ => 0
                };
                // Nếu ô chưa có chủ hoặc thuộc người chơi, và chưa đạt cấp tối đa
                if ((tile.PlayerId == null || tile.PlayerId == playerID) && tile.Level < 5)
                {
                    new BuyHome_Land(playerID, tile).ShowDialog();
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
            panels[index].Controls.Clear();
            panels[index].BackgroundImage = null;
            int rentPrice = 0;
            if (tile.Monopoly == "9" && monopoly != null)
            {
                rentPrice = 50 * monopoly.CountBusesOwned(playerID);
            }
            else if (tile.Monopoly != "9" && tile.Monopoly != "10")
            {
                rentPrice = tile.Level switch
                {
                    1 => tile.LandPrice,
                    >= 2 and <= 4 => tile.LandPrice + tile.HousePrice * (tile.Level - 1),
                    5 => tile.LandPrice + tile.HousePrice * 3 + tile.HotelPrice,
                    _ => 0
                };
            }
            if (tile.Monopoly != "0" && tile.Monopoly != "9" && tile.Monopoly != "10")
            {
                if (panels[index].Width > panels[index].Height)
                {
                    if (panels[index].Location.X < 500)
                    {
                        Panel colorPanel = new Panel
                        {
                            Size = new Size(panels[index].Width / 7, panels[index].Height),
                            BackColor = GetPanelColor(tile.Monopoly),
                            // Đặt vị trí sao cho nó nằm bên trái của panel cha
                            Location = new Point(panels[index].Width - panels[index].Width / 7, 0)
                        };
                        colorPanel.Click += Panel_Click;
                        panels[index].Controls.Add(colorPanel);
                    }
                    else
                    {
                        Panel colorPanel = new Panel
                        {
                            Size = new Size(panels[index].Width / 7, panels[index].Height),
                            BackColor = GetPanelColor(tile.Monopoly),
                            // Đặt vị trí sao cho nó nằm bên trái của panel cha
                            Location = new Point(panels[index].Width - panels[index].Width, 0)
                        };
                        colorPanel.Click += Panel_Click;
                        panels[index].Controls.Add(colorPanel);
                    }
                }
                else
                {
                    Panel colorPanel = new Panel
                    {
                        Size = new Size(panels[index].Width, panels[index].Height / 5),
                        BackColor = GetPanelColor(tile.Monopoly),
                        Location = new Point(0, 0)
                    };
                    colorPanel.Click += Panel_Click;
                    panels[index].Controls.Add(colorPanel);
                }
            }
            // === 2. Label chữ nằm ở dưới ===
            Label label = new Label
            {
                AutoSize = false,
                ForeColor = Color.Black,
                Font = new Font("Arial", 6, FontStyle.Bold)
            };
            // Phân loại hiển thị tùy theo loại ô
            // Ô bình thường
            if (tile.Monopoly != "0" && tile.Monopoly != "9" && tile.Monopoly != "10")
            {
                if (panels[index].Width > panels[index].Height)
                {
                    if (panels[index].Location.X < 500)
                    {
                        label.TextAlign = ContentAlignment.MiddleCenter;
                        label.Size = new Size(panels[index].Width * 6 / 7, panels[index].Height);
                        label.Location = new Point(0, 0);
                        label.BackColor = Color.LightBlue;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                    }
                    else
                    {
                        label.TextAlign = ContentAlignment.MiddleCenter;
                        label.Size = new Size(panels[index].Width * 6 / 7, panels[index].Height);
                        label.Location = new Point(panels[index].Width / 7, 0);
                        label.BackColor = Color.LightBlue;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                    }
                }
                else
                {
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Size = new Size(panels[index].Width, panels[index].Height * 4 / 5);
                    label.Location = new Point(0, panels[index].Height / 5);
                    label.BackColor = Color.LightBlue;
                    label.Text = tile.PlayerId == null
                        ? $"{tile.Name}\n${tile.LandPrice}"
                        : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                }
            }
            // Bến xe
            else if (tile.Monopoly == "9")
            {
                if (panels[index].Width > panels[index].Height)
                {
                    if (panels[index].Location.X < 500)
                    {
                        label.TextAlign = ContentAlignment.MiddleRight;
                        label.Size = new Size(panels[index].Width / 2, panels[index].Height);
                        label.Location = new Point(panels[index].Width - panels[index].Width / 2, 0);
                        label.BackColor = Color.Transparent;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                        SetTileBackgroundImage(index, "ben_xe.png", tile);
                    }
                    else
                    {
                        label.TextAlign = ContentAlignment.MiddleLeft;
                        label.Size = new Size(panels[index].Width / 2, panels[index].Height);
                        label.Location = new Point(panels[index].Width - panels[index].Width, 0);
                        label.BackColor = Color.Transparent;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                        SetTileBackgroundImage(index, "ben_xe.png", tile);
                    }
                }
                else
                {
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Size = new Size(panels[index].Width, panels[index].Height * 3/7);
                    label.Location = new Point(0, 0);
                    label.BackColor = Color.Transparent;
                    label.Text = tile.PlayerId == null
                        ? $"{tile.Name}\n${tile.LandPrice}"
                        : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                    SetTileBackgroundImage(index, "ben_xe.png", tile);
                }
            }
            //Công ty
            else if (tile.Monopoly == "10")
            {
                if (panels[index].Width > panels[index].Height)
                {
                    if (panels[index].Location.X < 500)
                    {
                        label.TextAlign = ContentAlignment.MiddleRight;
                        label.Size = new Size(panels[index].Width / 2, panels[index].Height);
                        label.Location = new Point(panels[index].Width - panels[index].Width / 2, 0);
                        label.BackColor = Color.Transparent;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                        if (tile.Name == "Công ty Cấp nước")
                            SetTileBackgroundImage(index, "cty_nuoc.png", tile);
                        else if (tile.Name == "Công ty Điện lực")
                            SetTileBackgroundImage(index, "cty_dien.png", tile);
                    }
                    else
                    {
                        label.TextAlign = ContentAlignment.MiddleLeft;
                        label.Size = new Size(panels[index].Width / 2, panels[index].Height);
                        label.Location = new Point(panels[index].Width - panels[index].Width, 0);
                        label.BackColor = Color.Transparent;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                        if (tile.Name == "Công ty Cấp nước")
                            SetTileBackgroundImage(index, "cty_nuoc.png", tile);
                        else if (tile.Name == "Công ty Điện lực")
                            SetTileBackgroundImage(index, "cty_dien.png", tile);
                    }
                }
                else
                {
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Size = new Size(panels[index].Width, panels[index].Height * 3/7);
                    label.Location = new Point(0, 0);
                    label.BackColor = Color.Transparent;
                    label.Text = tile.PlayerId == null
                        ? $"{tile.Name}\n${tile.LandPrice}"
                        : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                    if (tile.Name == "Công ty Cấp nước")
                        SetTileBackgroundImage(index, "cty_nuoc.png", tile);
                    else if (tile.Name == "Công ty Điện lực")
                        SetTileBackgroundImage(index, "cty_dien.png", tile);
                }
            }
            // Khí vận
            else if (tile.Name == "Khí vận")
            {
                label.Size = panels[index].Size;
                label.Location = new Point(0, 0);
                label.BackColor = Color.Transparent;
                SetTileBackgroundImage(index, "khi_van.png", tile);
            }
            else if (tile.Name == "Thuế thu nhập")
            {
                label.Size = panels[index].Size;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Location = new Point(0, 0);
                label.BackColor = Color.Lavender;
                label.Text = $"{tile.Name}\n\n$200 hoặc 10% tổng tài sản";
            }
            // 4 góc
            else
            {
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Size = panels[index].Size;
                label.Location = new Point(0, 0);
                label.BackColor = Color.Transparent;
                label.Text = tile.Name;

                switch (tile.Name)
                {
                    case "Bãi đậu xe":
                        SetTileBackgroundImage(index, "bai_dau_xe.png", tile);
                        label.TextAlign = ContentAlignment.BottomCenter;
                        break;
                    case "Nhà tù":
                        SetTileBackgroundImage(index, "ở_tù.png", tile);
                        label.TextAlign = ContentAlignment.BottomCenter;
                        break;
                    case "Đi thẳng vào tù":
                        SetTileBackgroundImage(index, "police.png", tile);
                        label.TextAlign = ContentAlignment.BottomCenter;
                        break;
                    default:
                        label.BackColor = Color.Lavender;
                        break;
                }
            }
            label.Click += Panel_Click;
            panels[index].Controls.Add(label);

        }
        // Màu nền cho Monopoly
        private Color GetPanelColor(string monopolyType)
        {
            return monopolyColors.TryGetValue(monopolyType, out var color) ? color : Color.White;
        }
        private void SetTileBackgroundImage(int index, string imageName, Tile tile)
        {
            string path = Path.Combine(Application.StartupPath, "Assets", "Images", imageName);
            if (!File.Exists(path)) return;
            Image img = Image.FromFile(path);
            if (tile.Monopoly == "9" || tile.Monopoly == "10")
            {
                Bitmap resizedImage = new Bitmap(panels[index].Width, panels[index].Height);
                using (Graphics g = Graphics.FromImage(resizedImage))
                {
                    g.Clear(panels[index].BackColor);

                    if (panels[index].Width > panels[index].Height)
                    {
                        int newWidth = panels[index].Width / 2;
                        if (panels[index].Location.X < 500)
                        {
                            g.DrawImage(img, new Rectangle(0, 0, newWidth, panels[index].Height));
                        }
                        else
                        {
                            g.DrawImage(img, new Rectangle(panels[index].Width - newWidth, 0, newWidth, panels[index].Height));
                        }
                    }
                    else
                    {
                        int newHeight = panels[index].Height * 4 / 7;
                        g.DrawImage(img, new Rectangle(0, panels[index].Height - newHeight, panels[index].Width, newHeight));
                    }
                }
                img.Dispose();
                panels[index].BackgroundImage = resizedImage;
                panels[index].BackgroundImageLayout = ImageLayout.None;
            }
            else if (tile.Name == "Khí vận")
            {
                panels[index].BackgroundImage = img;
                panels[index].BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                int newHeight = panels[index].Height * 6 / 7;
                Bitmap resizedImage = new Bitmap(panels[index].Width, panels[index].Height);

                using (Graphics g = Graphics.FromImage(resizedImage))
                {
                    g.Clear(panels[index].BackColor);
                    g.DrawImage(img, new Rectangle(0, 0, panels[index].Width, newHeight));
                }

                img.Dispose();
                panels[index].BackgroundImage = resizedImage;
                panels[index].BackgroundImageLayout = ImageLayout.None;
            }
        }
        // cái này để test chạy thoi
        private void AttachClickEventRecursively(Control control)
        {
            control.Click += Panel_Click;
            foreach (Control child in control.Controls)
            {
                AttachClickEventRecursively(child);
            }
        }
    }
}
