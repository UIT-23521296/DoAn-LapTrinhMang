using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonopolyWinForms.Login_Signup;

namespace MonopolyWinForms.GameLogic
{
    public class UpdateTileDisplay
    {        
        private Panel[] panels;
        private List<Tile> tiles;
        private List<Player> players;
        private MainForm mainform;
        private Monopoly monopoly;
        public UpdateTileDisplay(Panel[] panels, List<Tile> tiles, MainForm mainform, Monopoly monopoly, List<Player> players)
        {
            this.panels = panels;
            this.tiles = tiles;
            this.mainform = mainform;
            this.monopoly = monopoly;
            this.players = players;
        }

        public void UpdateTileDisplayUI(int index, Player currentPlayer)
        {
            if (index < 0 || index >= panels.Length || index >= tiles.Count)
                return;
            var tile = tiles[index];
            int playerID = currentPlayer.ID;
            var panel = panels[index];
            // Lưu lại tất cả các marker từ Dictionary
            var markersToKeep = new List<Panel>();
            foreach (var kvp in mainform.playerMarkers)
            {
                if (panel.Controls.Contains(kvp.Value))
                {
                    markersToKeep.Add(kvp.Value);
                }
            }
            // Xóa các controls khác (giữ lại marker)
            var controlsToRemove = new List<Control>();
            foreach (Control control in panel.Controls)
            {
                if (!markersToKeep.Contains(control))
                {
                    controlsToRemove.Add(control);
                }
            }
            foreach (var control in controlsToRemove)
            {
                panel.Controls.Remove(control);
            }
            int rentPrice = 0;
            if (tile.Monopoly == "9" && monopoly != null)
            {
                rentPrice = 50 * monopoly.CountBusesOwned(playerID);
            }
            else if (tile.Monopoly == "10" && monopoly != null)
            {
                if (monopoly.CountCompaniesOwned(playerID) == 1)
                    rentPrice = 25;
                else
                    rentPrice = 100;
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
                            Location = new Point(panels[index].Width - panels[index].Width / 7, 0) //bên trái
                        };
                        panels[index].Controls.Add(colorPanel);
                    }
                    else
                    {
                        Panel colorPanel = new Panel
                        {
                            Size = new Size(panels[index].Width / 7, panels[index].Height),
                            BackColor = GetPanelColor(tile.Monopoly),
                            Location = new Point(panels[index].Width - panels[index].Width, 0) // bên phải
                        };
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
                    panels[index].Controls.Add(colorPanel);
                }
            }
            // === 2. Label chữ nằm ở dưới ===
            Label label = new Label
            {
                Name = "labelrent",
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
                            : $"{tile.Name}\n${rentPrice/2}";
                        if (tile.PlayerId != null)
                        {
                            AddOwnerImageToTilePanel(panels[index], tile, currentPlayer);
                            label.TextAlign = ContentAlignment.TopCenter;
                            label.Padding = new Padding(0, 10, 0, 0);
                        }
                        else
                        {
                            label.Location = new Point(0, 0);
                            foreach (Control ctrl in panels[index].Controls.OfType<PictureBox>().ToList())
                            {
                                panels[index].Controls.Remove(ctrl);
                                ctrl.Dispose();
                            }
                        }
                    }
                    else
                    {
                        label.TextAlign = ContentAlignment.MiddleCenter;
                        label.Size = new Size(panels[index].Width * 6 / 7, panels[index].Height);
                        label.Location = new Point(panels[index].Width / 7, 0);
                        label.BackColor = Color.LightBlue;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\n${rentPrice/2}";
                        if (tile.PlayerId != null)
                        {
                            AddOwnerImageToTilePanel(panels[index], tile, currentPlayer);
                            label.TextAlign = ContentAlignment.TopCenter;
                            label.Padding = new Padding(0, 10, 0, 0);
                        }
                        else
                        {
                            foreach (Control ctrl in panels[index].Controls.OfType<PictureBox>().ToList())
                            {
                                panels[index].Controls.Remove(ctrl);
                                ctrl.Dispose();
                            }
                        }
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
                        : $"{tile.Name}\n${rentPrice / 2}";
                    if (tile.PlayerId != null)
                    {
                        AddOwnerImageToTilePanel(panels[index], tile, currentPlayer);
                        label.TextAlign = ContentAlignment.TopCenter;
                        label.Padding = new Padding(0, 15, 0, 0);
                    }
                    else
                    {
                        foreach (Control ctrl in panels[index].Controls.OfType<PictureBox>().ToList())
                        {
                            panels[index].Controls.Remove(ctrl);
                            ctrl.Dispose();
                        }
                    }
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
                            : $"{tile.Name}\n{currentPlayer.Name}\n${rentPrice}";
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
                            : $"{tile.Name}\n{currentPlayer.Name}\n${rentPrice}";
                        SetTileBackgroundImage(index, "ben_xe.png", tile);
                    }
                }
                else
                {
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Size = new Size(panels[index].Width, panels[index].Height * 3 / 7);
                    label.Location = new Point(0, 0);
                    label.BackColor = Color.Transparent;
                    label.Text = tile.PlayerId == null
                        ? $"{tile.Name}\n${tile.LandPrice}"
                        : $"{tile.Name}\n{currentPlayer.Name}\n${rentPrice}";
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
                            : $"{tile.Name}\n{currentPlayer.Name}\n${rentPrice}";
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
                            : $"{tile.Name}\n{currentPlayer.Name}\n${rentPrice}";
                        if (tile.Name == "Công ty Cấp nước")
                            SetTileBackgroundImage(index, "cty_nuoc.png", tile);
                        else if (tile.Name == "Công ty Điện lực")
                            SetTileBackgroundImage(index, "cty_dien.png", tile);
                    }
                }
                else
                {
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Size = new Size(panels[index].Width, panels[index].Height * 3 / 7);
                    label.Location = new Point(0, 0);
                    label.BackColor = Color.Transparent;
                    label.Text = tile.PlayerId == null
                        ? $"{tile.Name}\n${tile.LandPrice}"
                        : $"{tile.Name}\n{currentPlayer.Name}\n${rentPrice}";
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
            // Thuế
            else if (tile.Name == "Thuế thu nhập")
            {
                label.Size = panels[index].Size;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Location = new Point(0, 0);
                label.BackColor = Color.Lavender;
                label.Text = $"{tile.Name}\n\n$200 hoặc 10% tổng tài sản";
            }
            else if (tile.Name == "Thuế đặc biệt")
            {
                label.Size = panels[index].Size;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Location = new Point(0, 0);
                label.BackColor = Color.Lavender;
                label.Text = $"{tile.Name}\n\n15% tổng tài sản";
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
                    case "Ô bắt đầu":
                        label.Text = $"{tile.Name}\n\nNhận $200 khi đi qua";
                        label.BackColor = Color.Lavender;
                        break;
                    default:
                        label.BackColor = Color.Lavender;
                        break;
                }
            }
            panels[index].Controls.Add(label);
            foreach (var marker in markersToKeep)
            {
                marker.BringToFront();
            }
        }
        // Màu nền cho Monopoly
        public Color GetPanelColor(string monopolyType)
        {
            return mainform.monopolyColors.TryGetValue(monopolyType, out var color) ? color : Color.White;
        }
        public void SetTileBackgroundImage(int index, string imageName, Tile tile)
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
        public Image GetHouseImage(int level, Player player, List<Player> players, Tile tile)
        {
            // Giả sử có các ảnh như: house1.png, house2.png, hotel.png
            string imagePath;

            
            if (tile.PlayerId != null)
            {
                if (level == 5)
                    imagePath = Path.Combine(Application.StartupPath, "Assets", "Images", $"{players[tile.PlayerId.Value - 1].Color.Name}-hotel.png");
                else if (level >= 2 && level <= 4)
                    imagePath = Path.Combine(Application.StartupPath, "Assets", "Images", $"{players[tile.PlayerId.Value - 1].Color.Name}-house{level - 1}.png");
                else
                    imagePath = Path.Combine(Application.StartupPath, "Assets", "Images", $"{players[tile.PlayerId.Value - 1].Color.Name}-land.png");
                return Image.FromFile(imagePath);
            }
            else
            {
                if (level == 5)
                    imagePath = Path.Combine(Application.StartupPath, "Assets", "Images", $"{player.Color.Name}-hotel.png");
                else if (level >= 2 && level <= 4)
                    imagePath = Path.Combine(Application.StartupPath, "Assets", "Images", $"{player.Color.Name}-house{level - 1}.png");

                else
                    imagePath = Path.Combine(Application.StartupPath, "Assets", "Images", $"{player.Color.Name}-land.png");
                return Image.FromFile(imagePath);
            }
        }
        private void AddOwnerImageToTilePanel(Panel panel, Tile tile, Player player)
        {
            foreach (Control c in panel.Controls.OfType<PictureBox>().ToList())
            {
                panel.Controls.Remove(c);
                c.Dispose();
            }
            if (tile.PlayerId == null) return;
            PictureBox ownerPic = new PictureBox();
            ownerPic.SizeMode = PictureBoxSizeMode.Zoom;
            ownerPic.BackColor = Color.LightBlue;
            ownerPic.Image = GetHouseImage(tile.Level, player, players, tile);

            if (panel.Width < panel.Height) // ô dọc
            {
                ownerPic.Size = new Size(panel.Width, panel.Height/3);
                ownerPic.Location = new Point(0, panel.Height*2/3);
            }
            else // ô ngang
            {
                ownerPic.Size = new Size(panel.Width*6/7, panel.Height / 2);
                if (panel.Left < panel.Width / 2)
                {
                    // Panel nằm bên trái
                    ownerPic.Location = new Point(0, panel.Height /2);
                }
                else
                {
                    // Panel nằm bên phải
                    ownerPic.Location = new Point(panel.Width / 7, panel.Height /2);
                }
            }
            panel.Controls.Add(ownerPic);
            ownerPic.BringToFront();
        }
        public void UpdateBusStationRent(int playerId)
        {
            // Lấy tất cả bến xe người chơi sở hữu
            var ownedBusStations = tiles.FindAll(t => t.Monopoly == "9" && t.PlayerId == playerId);
            int count = ownedBusStations.Count;
            int rent = count switch
            {
                1 => 50,
                2 => 100,
                3 => 150,
                4 => 200,
                _ => 0
            };
            foreach (var tile in ownedBusStations)
            {
                tile.RentPrice = rent;
                foreach (Panel panel in panels)
                {
                    if (panel.Tag is Tile t && t.Id == tile.Id)
                    {
                        var rentLabel = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "labelrent");
                        if (rentLabel != null)
                        {
                            rentLabel.Text = $"{tile.Name}\nPlayer {tile.PlayerId}\n${rent}";
                        }
                    }
                }
            }
        }
        public void UpdateCompanyRent(int playerId)
        {
            var ownedCompany = tiles.FindAll(t => t.Monopoly == "10" && t.PlayerId == playerId);
            int count = ownedCompany.Count;
            int rent = count switch
            {
                1 => 25,
                2 => 100,
                _ => 0
            };
            foreach (var tile in ownedCompany)
            {
                tile.RentPrice = rent;
                foreach (Panel panel in panels)
                {
                    if (panel.Tag is Tile t && t.Id == tile.Id)
                    {
                        var rentLabel = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "labelrent");
                        if (rentLabel != null)
                        {
                            rentLabel.Text = $"{tile.Name}\nPlayer {tile.PlayerId}\n${rent}";
                        }
                    }
                }
            }
        }
    }
}
