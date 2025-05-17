using buyLand_Home;
using MonopolyWinForms.BuyLand_Home;
using MonopolyWinForms.GameLogic;
using System;
using System.Data.SqlTypes;
using System.Drawing.Drawing2D;
using System.Numerics;
namespace MonopolyWinForms
{
    public partial class MainForm : Form
    {
        private List<Tile> tiles;       // Danh sách các ô đất
        private Panel[] panels;         // Mảng các Panel tương ứng với các ô
        private Monopoly monopoly;      // Khởi tạo Monopoly
        private List<Player> players;   // Danh sách người chơi
        private int currentPlayerIndex = 0;
        private Random random = new Random();
        private Dictionary<int, Panel> playerMarkers = new Dictionary<int, Panel>();
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
        private int CalculateRent(Tile tile, int playerId)
        {
            if (tile == null) return 0;
            if (tile.PlayerId == null || tile.PlayerId == playerId)
                return 0;
            switch (tile.Monopoly)
            {
                case "9": // Bến xe
                    int busCount = monopoly.CountBusesOwned(tile.PlayerId.Value);
                    return busCount switch
                    {
                        1 => 50,
                        2 => 100,
                        3 => 150,
                        4 => 200,
                        _ => 0
                    };
                case "10": // Công ty
                    int companyCount = monopoly.CountCompaniesOwned(tile.PlayerId.Value);
                    // Giả sử giá trị xúc xắc được truyền vào hoặc random
                    int diceValue = random.Next(1, 7) + random.Next(1, 7); // Tổng 2 xúc xắc
                    return companyCount switch
                    {
                        1 => diceValue * 20,
                        2 => diceValue * 50,
                        _ => 0
                    };
                default: // Ô đất thường
                    return tile.Level switch
                    {
                        1 => tile.LandPrice/2,
                        2 => (tile.LandPrice + tile.HousePrice)/2,
                        3 => (tile.LandPrice + tile.HousePrice * 2)/2,
                        4 => (tile.LandPrice + tile.HousePrice * 3) / 2,
                        5 => (tile.LandPrice + tile.HousePrice * 3 + tile.HotelPrice)/2,
                        _ => 0
                    };
            }
        }
        public MainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
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
            players = new List<Player>();
            tiles = Tile.LoadTilesFromFile();
            monopoly = new Monopoly(tiles);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            Player player1 = new Player(1, 1000);
            Player player2 = new Player(2, 1000);
            Player player3 = new Player(3, 1000);
            Player player4 = new Player(4, 1000);
            players.Add(player1);
            players.Add(player2);
            players.Add(player3);
            players.Add(player4);
            UpdatePlayerPanel(panel41, player1);
            UpdatePlayerPanel(panel42, player2);
            UpdatePlayerPanel(panel43, player3);
            UpdatePlayerPanel(panel44, player4);
            for (int i = 0; i < panels.Length && i < tiles.Count; i++)
            {
                panels[i].Tag = tiles[i];
                UpdateTileDisplay(i, players[currentPlayerIndex]);
            }
            foreach (var player in players)
            {
                player.TileIndex = 0;
                InitializePlayerMarker(player);
            }
        }
        public Panel GetPlayerPanel(int playerId)
        {
            return playerId switch
            {
                1 => panel41,
                2 => panel42,
                3 => panel43,
                4 => panel44,
                _ => panel1
            };
        }
        // Hàm cập nhật hiển thị label trên từng panel
        public void UpdateTileDisplay(int index, Player currentPlayer)
        {
            if (index < 0 || index >= panels.Length || index >= tiles.Count)
                return;
            var tile = tiles[index];
            int playerID = currentPlayer.ID;
            var panel = panels[index];
            // Lưu lại tất cả các marker từ Dictionary
            var markersToKeep = new List<Panel>();
            foreach (var kvp in playerMarkers){
                if (panel.Controls.Contains(kvp.Value)){
                    markersToKeep.Add(kvp.Value);
                }
            }
            // Xóa các controls khác (giữ lại marker)
            var controlsToRemove = new List<Control>();
            foreach (Control control in panel.Controls){
                if (!markersToKeep.Contains(control)){
                    controlsToRemove.Add(control);
                }
            }
            foreach (var control in controlsToRemove){
                panel.Controls.Remove(control);
            }
            int rentPrice = 0;
            if (tile.Monopoly == "9" && monopoly != null){
                rentPrice = 50 * monopoly.CountBusesOwned(playerID);
            }
            else if (tile.Monopoly == "10" && monopoly != null){
                if (monopoly.CountCompaniesOwned(playerID) == 1)
                    rentPrice = 25;
                else
                    rentPrice = 100;
            }
            else if (tile.Monopoly != "9" && tile.Monopoly != "10"){
                rentPrice = tile.Level switch{
                    1 => tile.LandPrice,
                    >= 2 and <= 4 => tile.LandPrice + tile.HousePrice * (tile.Level - 1),
                    5 => tile.LandPrice + tile.HousePrice * 3 + tile.HotelPrice,
                    _ => 0
                };
            }
            if (tile.Monopoly != "0" && tile.Monopoly != "9" && tile.Monopoly != "10"){
                if (panels[index].Width > panels[index].Height){
                    if (panels[index].Location.X < 500){
                        Panel colorPanel = new Panel{
                            Size = new Size(panels[index].Width / 7, panels[index].Height),
                            BackColor = GetPanelColor(tile.Monopoly),
                            Location = new Point(panels[index].Width - panels[index].Width / 7, 0) //bên trái
                        };
                        panels[index].Controls.Add(colorPanel);
                    }else{
                        Panel colorPanel = new Panel{
                            Size = new Size(panels[index].Width / 7, panels[index].Height),
                            BackColor = GetPanelColor(tile.Monopoly),
                            Location = new Point(panels[index].Width - panels[index].Width, 0) // bên phải
                        };
                        panels[index].Controls.Add(colorPanel);
                    }
                }else{
                    Panel colorPanel = new Panel{
                        Size = new Size(panels[index].Width, panels[index].Height / 5),
                        BackColor = GetPanelColor(tile.Monopoly),
                        Location = new Point(0, 0)
                    };
                    panels[index].Controls.Add(colorPanel);
                }
            }
            // === 2. Label chữ nằm ở dưới ===
            Label label = new Label{
                Name = "labelrent",
                AutoSize = false,
                ForeColor = Color.Black,
                Font = new Font("Arial", 6, FontStyle.Bold)
            };
            // Phân loại hiển thị tùy theo loại ô
            // Ô bình thường
            if (tile.Monopoly != "0" && tile.Monopoly != "9" && tile.Monopoly != "10"){
                if (panels[index].Width > panels[index].Height){
                    if (panels[index].Location.X < 500){
                        label.TextAlign = ContentAlignment.MiddleCenter;
                        label.Size = new Size(panels[index].Width * 6 / 7, panels[index].Height);
                        label.Location = new Point(0, 0);
                        label.BackColor = Color.LightBlue;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                    }else{
                        label.TextAlign = ContentAlignment.MiddleCenter;
                        label.Size = new Size(panels[index].Width * 6 / 7, panels[index].Height);
                        label.Location = new Point(panels[index].Width / 7, 0);
                        label.BackColor = Color.LightBlue;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                    }
                }else{
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
            else if (tile.Monopoly == "9"){
                if (panels[index].Width > panels[index].Height){
                    if (panels[index].Location.X < 500){
                        label.TextAlign = ContentAlignment.MiddleRight;
                        label.Size = new Size(panels[index].Width / 2, panels[index].Height);
                        label.Location = new Point(panels[index].Width - panels[index].Width / 2, 0);
                        label.BackColor = Color.Transparent;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                        SetTileBackgroundImage(index, "ben_xe.png", tile);
                    }else{
                        label.TextAlign = ContentAlignment.MiddleLeft;
                        label.Size = new Size(panels[index].Width / 2, panels[index].Height);
                        label.Location = new Point(panels[index].Width - panels[index].Width, 0);
                        label.BackColor = Color.Transparent;
                        label.Text = tile.PlayerId == null
                            ? $"{tile.Name}\n${tile.LandPrice}"
                            : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                        SetTileBackgroundImage(index, "ben_xe.png", tile);
                    }
                }else{
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Size = new Size(panels[index].Width, panels[index].Height * 3 / 7);
                    label.Location = new Point(0, 0);
                    label.BackColor = Color.Transparent;
                    label.Text = tile.PlayerId == null
                        ? $"{tile.Name}\n${tile.LandPrice}"
                        : $"{tile.Name}\nPlayer {tile.PlayerId}\n${rentPrice}";
                    SetTileBackgroundImage(index, "ben_xe.png", tile);
                }
            }
            //Công ty
            else if (tile.Monopoly == "10"){
                if (panels[index].Width > panels[index].Height){
                    if (panels[index].Location.X < 500){
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
                    }else{
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
                }else{
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Size = new Size(panels[index].Width, panels[index].Height * 3 / 7);
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
            else if (tile.Name == "Khí vận"){
                label.Size = panels[index].Size;
                label.Location = new Point(0, 0);
                label.BackColor = Color.Transparent;
                SetTileBackgroundImage(index, "khi_van.png", tile);
            }
            // Thuế
            else if (tile.Name == "Thuế thu nhập"){
                label.Size = panels[index].Size;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Location = new Point(0, 0);
                label.BackColor = Color.Lavender;
                label.Text = $"{tile.Name}\n\n$200 hoặc 10% tổng tài sản";
            }
            else if (tile.Name == "Thuế đặc biệt"){
                label.Size = panels[index].Size;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Location = new Point(0, 0);
                label.BackColor = Color.Lavender;
                label.Text = $"{tile.Name}\n\n15% tổng tài sản";
            }
            // 4 góc
            else{
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
            } panels[index].Controls.Add(label);
            foreach (var marker in markersToKeep){
                marker.BringToFront();
            }
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
            if (tile.Monopoly == "9" || tile.Monopoly == "10"){
                Bitmap resizedImage = new Bitmap(panels[index].Width, panels[index].Height);
                using (Graphics g = Graphics.FromImage(resizedImage)){
                    g.Clear(panels[index].BackColor);
                    if (panels[index].Width > panels[index].Height){
                        int newWidth = panels[index].Width / 2;
                        if (panels[index].Location.X < 500){
                            g.DrawImage(img, new Rectangle(0, 0, newWidth, panels[index].Height));
                        }else{
                            g.DrawImage(img, new Rectangle(panels[index].Width - newWidth, 0, newWidth, panels[index].Height));
                        }
                    }else{
                        int newHeight = panels[index].Height * 4 / 7;
                        g.DrawImage(img, new Rectangle(0, panels[index].Height - newHeight, panels[index].Width, newHeight));
                    }
                } img.Dispose();
                panels[index].BackgroundImage = resizedImage;
                panels[index].BackgroundImageLayout = ImageLayout.None;
            }else if (tile.Name == "Khí vận"){
                panels[index].BackgroundImage = img;
                panels[index].BackgroundImageLayout = ImageLayout.Stretch;
            }else{
                int newHeight = panels[index].Height * 6 / 7;
                Bitmap resizedImage = new Bitmap(panels[index].Width, panels[index].Height);
                using (Graphics g = Graphics.FromImage(resizedImage)){
                    g.Clear(panels[index].BackColor);
                    g.DrawImage(img, new Rectangle(0, 0, panels[index].Width, newHeight));
                } img.Dispose();
                panels[index].BackgroundImage = resizedImage;
                panels[index].BackgroundImageLayout = ImageLayout.None;
            }
        }
        public void UpdateBusStationRent(int playerId)
        {
            // Lấy tất cả bến xe người chơi sở hữu
            var ownedBusStations = tiles.FindAll(t => t.Monopoly == "9" && t.PlayerId == playerId);
            int count = ownedBusStations.Count;
            int rent = count switch{
                1 => 50,
                2 => 100,
                3 => 150,
                4 => 200,
                _ => 0
            };
            foreach (var tile in ownedBusStations){
                tile.RentPrice = rent;
                foreach (Panel panel in panels){
                    if (panel.Tag is Tile t && t.Id == tile.Id){
                        var rentLabel = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "labelrent");
                        if (rentLabel != null){
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
            int rent = count switch{
                1 => 25,
                2 => 100,
                _ => 0
            };
            foreach (var tile in ownedCompany){
                tile.RentPrice = rent;
                foreach (Panel panel in panels){
                    if (panel.Tag is Tile t && t.Id == tile.Id){
                        var rentLabel = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "labelrent");
                        if (rentLabel != null){
                            rentLabel.Text = $"{tile.Name}\nPlayer {tile.PlayerId}\n${rent}";
                        }
                    }
                }
            }
        }
        public void UpdatePlayerPanel(Panel playerPanel, Player player)
        {
            playerPanel.Controls.Clear();
            PictureBox pic = new PictureBox{
                Size = new Size(50, 50),
                Location = new Point(5, 5),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = player.GetAvatar() // Trả về Image
            };
            playerPanel.Controls.Add(pic);
            Label nameLabel = new Label{
                Text = player.Name,
                Location = new Point(60, 5),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black
            };
            playerPanel.Controls.Add(nameLabel);
            Label moneyLabel = new Label{
                Text = $"Tiền: ${player.Money}",
                Location = new Point(60, 30),
                AutoSize = true,
                Font = new Font("Arial", 9),
                ForeColor = Color.Green
            };
            playerPanel.Controls.Add(moneyLabel);
        }
        private void InitializePlayerMarker(Player player)
        {
            Panel marker = new Panel{
                Size = new Size(22, 22),
                BackColor = player.Color,
                Name = $"player{player.ID}Marker"
            };
            // Tạo hình tròn
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, marker.Width, marker.Height);
            marker.Region = new Region(path);
            var tilePanel = panels[player.TileIndex];
            var tile = tilePanel?.Tag as Tile;
            if (tilePanel != null){
                // Thêm sự kiện Paint để vẽ lại khi cần
                marker.Paint += (sender, e) =>{
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (var brush = new SolidBrush(player.Color)){ // chỉnh màu
                        e.Graphics.FillEllipse(brush, 0, 0, marker.Width, marker.Height);
                    }
                };
                Point position = CalculateMarkerPosition(tilePanel, tile, player.ID);
                marker.Location = position;
                tilePanel.Controls.Add(marker);
                marker.BringToFront();
                playerMarkers[player.ID] = marker;
            }
        }
        public void UpdatePlayerMarkerPosition(Player player, int newIndex)
        {
            if (playerMarkers.TryGetValue(player.ID, out var marker)){
                if (player.TileIndex >= 0 && player.TileIndex < panels.Length){
                    panels[player.TileIndex].Controls.Remove(marker);
                }
                player.TileIndex = newIndex;
                var newTilePanel = panels[newIndex];
                var tile = newTilePanel?.Tag as Tile;
                if (newTilePanel != null){
                    Point newPosition = CalculateMarkerPosition(newTilePanel, tile, player.ID);
                    newTilePanel.Controls.Add(marker);
                    marker.Location = newPosition;
                    marker.BringToFront();
                    if (tile != null){
                        ShowTileActionForm(tile, player);
                    }
                }
            }
        }
        private void ShowTileActionForm(Tile tile, Player currentPlayer)
        {
            // Kiểm tra nếu ô đã có chủ và không phải là người chơi hiện tại
            if (tile.PlayerId != null && tile.PlayerId != currentPlayer.ID){
                int rent = CalculateRent(tile, currentPlayer.ID);
                if (currentPlayer.DoubleMoney >= 1)
                {
                    currentPlayer.Money -= (rent*2);
                    currentPlayer.DoubleMoney--;
                    UpdatePlayerPanel(GetPlayerPanel(currentPlayer.ID), currentPlayer);
                    MessageBox.Show($"Bạn phải trả ${rent*2} tiền thuê cho Player {tile.PlayerId}!", "Trả tiền thuê");
                    return;
                }
                else if (currentPlayer.ReduceHalfMoney >= 1)
                {
                    currentPlayer.Money -= (rent / 2);
                    currentPlayer.ReduceHalfMoney--;
                    UpdatePlayerPanel(GetPlayerPanel(currentPlayer.ID), currentPlayer);
                    MessageBox.Show($"Bạn phải trả ${rent/2} tiền thuê cho Player {tile.PlayerId}!", "Trả tiền thuê");
                    return;
                }else{
                    currentPlayer.Money -= rent;
                    UpdatePlayerPanel(GetPlayerPanel(currentPlayer.ID), currentPlayer);
                    MessageBox.Show($"Bạn phải trả ${rent} tiền thuê cho Player {tile.PlayerId}!", "Trả tiền thuê");
                    return;
                }               
            }switch (tile.Monopoly){
                case "0": // Ô đặc biệt
                    HandleSpecialTile(tile, currentPlayer);
                    break;
                case "9": // Bến xe
                    HandleBusStationTile(tile, currentPlayer);
                    break;
                case "10": // Công ty
                    HandleCompanyTile(tile, currentPlayer);
                    break;
                default: // Ô đất thường
                    HandlePropertyTile(tile, currentPlayer);
                    break;
            }
        }
        private void HandleSpecialTile(Tile tile, Player currentPlayer)
        {
            switch (tile.Name)
            {
                case "Khí vận":
                    DrawChanceCard(currentPlayer);
                    break;
                case "Cơ hội":
                    DrawCommunityChestCard(currentPlayer);
                    break;
                case "Thuế thu nhập":
                    HandleIncomeTax(currentPlayer);
                    break;
                case "Thế đặc biệt":
                    HandleSpecialTax(currentPlayer);
                    break;
                case "Nhà tù":
                    HandleJustVisiting();
                    break;
                case "Đi thẳng vào tù":
                    HandleGoToJail(currentPlayer);
                    break;
            }
            UpdatePlayerPanel(GetPlayerPanel(currentPlayer.ID), currentPlayer);
        }
        // ======= CÁC HÀM XỬ LÝ CHI TIẾT =======
        private void DrawChanceCard(Player player)
        {
            var path = "Co_hoi.txt";
            var cards = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            if (cards.Length > 0)
            {
                string card = cards[random.Next(cards.Length)];
                ProcessCardEffect(player, card, "Khí vận");
            }
        }
        private void DrawCommunityChestCard(Player player)
        {
            var path = "Co_hoi.txt";
            var cards = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            if (cards.Length > 0)
            {
                string card = cards[random.Next(cards.Length)];
                ProcessCardEffect(player, card, "Cơ hội");
            }
        }
        private void ProcessCardEffect(Player player, string card, string deckType)
        {
            string message = $"{deckType}: {card}";
            bool movePlayer = false;
            int moveToIndex = -1;
            MessageBox.Show(message, "Thẻ Bài");
            switch (card)
            {
                case "Đi thẳng vào tù":
                    HandleGoToJail(player);
                    return;
                case "Tự do ra tù":
                    player.AddOutPrisonCard();
                    break;
                case "Trả gấp đôi tiền thuê cho ô tiếp theo":
                    player.DoubleMoney++;
                    break;
                case "Giảm 50% tiền thuê cho ô tiếp theo":
                    player.ReduceHalfMoney++;
                    break;
                case "Đi đến ô bắt đầu":
                    moveToIndex = 0;
                    player.AddMoney(200); // Về ô bắt đầu nhận 200
                    movePlayer = true;
                    break;
                case "Bán 1 căn nhà":
                    ManageProperties(player);
                    break;
                case "Phá nhà":
                    ManageProperties(player);
                    break;
                case "Đến ô bến xe tiếp theo":
                    moveToIndex = GetNextBusStation(player.TileIndex);
                    player.DoubleMoney++;
                    movePlayer = true;
                    break;
                case "Đến ô công ty tiếp theo":
                    moveToIndex = GetNextCompany(player.TileIndex);
                    movePlayer = true;
                    break;
                case "Đi đến ô tùy chọn":
                    int selectedIndex = PromptSelectTile();
                    if (selectedIndex >= 0){
                        moveToIndex = selectedIndex;
                        movePlayer = true;
                    }
                    break;
                default:
                    break;
            }
            if (movePlayer && moveToIndex >= 0){
                UpdatePlayerMarkerPosition(player, moveToIndex);
            }
        }
        private void HandleIncomeTax(Player player)
        {
            int tax = Math.Min(200, player.Money / 10);
            player.SubtractMoney(tax);
            MessageBox.Show($"Thuế thu nhập: Bạn phải trả ${tax}", "Thuế");
        }
        private void HandleSpecialTax(Player player)
        {
            int tax = (int)(player.Money * 0.15);
            player.SubtractMoney(tax);
            MessageBox.Show($"Thuế đặc biệt: trả 15% tổng tài sản", "Thuế");
        }
        private void HandleJustVisiting()
        {
            MessageBox.Show("thăm tù", "Thăm Tù");
        }
        public void HandleGoToJail(Player player)
        {
            if (player.OutPrison > 0){
                player.OutPrison--;
                MessageBox.Show("Sử dụng thẻ thoát tù, bạn không phải vào tù", "Thoát Tù");
                return;
            }
            int jailIndex = tiles.FindIndex(t => t.Name == "Nhà tù");
            player.TileIndex = jailIndex;
            UpdatePlayerMarkerPosition(player, jailIndex);
            MessageBox.Show("Bạn bị vào tù!", "Vào Tù");
        }
        private void HandleBusStationTile(Tile tile, Player currentPlayer)
        {
            if (tile.PlayerId == null){
                if (currentPlayer.Money < tile.LandPrice){
                    MessageBox.Show("Không đủ tiền mua bến xe!", "Thông báo");
                    return;
                }
                using (var buyBusForm = new BuyBus(currentPlayer.ID, tile, monopoly, this)){
                    if (buyBusForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= tile.LandPrice;
                        UpdatePlayerPanel(GetPlayerPanel(currentPlayer.ID), currentPlayer);
                        UpdateBusStationRent(currentPlayer.ID);
                        UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                    }
                }
            }
        }
        private void HandleCompanyTile(Tile tile, Player currentPlayer)
        {
            if (tile.PlayerId == null){
                if (currentPlayer.Money < tile.LandPrice){
                    MessageBox.Show("Không đủ tiền!", "Thông báo");
                    return;
                }
                using (var buyCompanyForm = new BuyCompany(currentPlayer.ID, tile, monopoly, this)){
                    if (buyCompanyForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= tile.LandPrice;
                        UpdatePlayerPanel(GetPlayerPanel(currentPlayer.ID), currentPlayer);
                        UpdateCompanyRent(currentPlayer.ID);
                        UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                    }
                }
            }
        }
        private void HandlePropertyTile(Tile tile, Player currentPlayer)
        {
            if (tile.PlayerId == null){
                if (currentPlayer.Money < tile.LandPrice){
                    MessageBox.Show("Không đủ tiền mua đất!", "Thông báo");
                    return;
                }
                using (var landForm = new BuyHome_Land(currentPlayer.ID, tile, monopoly, this)){
                    if (landForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= tile.LandPrice;
                        UpdatePlayerPanel(GetPlayerPanel(currentPlayer.ID), currentPlayer);
                        UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                    }
                }
            }
            else if (tile.PlayerId == currentPlayer.ID)
            {
                int upgradeCost = tile.Level == 4 ? tile.HotelPrice : tile.HousePrice;
                if (currentPlayer.Money < upgradeCost){
                    MessageBox.Show("Không đủ tiền nâng cấp!", "Thông báo");
                    return;
                }
                using (var upgradeForm = new BuyHome_Land(currentPlayer.ID, tile, monopoly, this)){
                    if (upgradeForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= upgradeCost;
                        UpdatePlayerPanel(GetPlayerPanel(currentPlayer.ID), currentPlayer);
                        UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                    }
                }
            }
        }
        private Point CalculateMarkerPosition(Panel tilePanel, Tile tile, int playerId)
        {
            const int markerSize = 22;
            int offsetX = 0;
            int offsetY = 0;
            switch (playerId)
            {
                case 1: // Player 1: trên trái
                    offsetX = -20;
                    offsetY = -15;
                    break;
                case 2: // Player 2: trên phải
                    offsetX = 20;
                    offsetY = -15;
                    break;
                case 3: // Player 3: dưới trái
                    offsetX = -20;
                    offsetY = 15;
                    break;
                case 4: // Player 4: dưới phải
                    offsetX = 20;
                    offsetY = 15;
                    break;
            }
            int centerX = (tilePanel.Width - markerSize) / 2 + offsetX;
            int centerY = (tilePanel.Height - markerSize) / 2 + offsetY;
            return new Point(centerX, centerY);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var player = players[currentPlayerIndex];
            int dice1 = random.Next(1, 7);
            int dice2 = random.Next(1, 7);
            int totalSteps = dice1 + dice2;
            MessageBox.Show($"Bạn tung được: {dice1} và {dice2} (Tổng: {totalSteps})", "Kết quả xúc xắc");
            int newIndex = (player.TileIndex + totalSteps) % panels.Length;
            if (newIndex < player.TileIndex)
            {
                player.AddMoney(200);
                UpdatePlayerPanel(GetPlayerPanel(player.ID), player);
                MessageBox.Show("Bạn đi qua ô bắt đầu, nhận $200!", "Nhận tiền");
            }
            UpdatePlayerMarkerPosition(player, newIndex);
            bool isDouble = dice1 == dice2;
            if (isDouble){
                player.RolledDoubleDice();
            }else{
                player.ResetDoubleDice();
            }
            if (player.DoubleDices == 3){
                player.ResetDoubleDice();
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                return;
            }
            if (!isDouble){
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            }
        }
        private int GetNextBusStation(int currentIndex)
        {
            int[] busStations = { 5, 15, 25, 35 };
            foreach (int station in busStations){
                if (station > currentIndex)
                    return station;
            }return busStations[0];
        }
        private int GetNextCompany(int currentIndex)
        {
            int[] companies = { 12, 28 };
            foreach (int comp in companies){
                if (comp > currentIndex)
                    return comp;
            }return companies[0];
        }
        private int PromptSelectTile()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập số ô muốn đến (0-39):", "Chọn ô");
            if (int.TryParse(input, out int tileIndex) && tileIndex >= 0 && tileIndex < 40){
                return tileIndex;
            }return -1;
        }
        private void ManageProperties(Player player)
        {
            var ownedTiles = tiles
                .Where(t => t.PlayerId == player.ID)
                .ToList();

            if (ownedTiles.Count == 0)
            {
                MessageBox.Show("Bạn không sở hữu ô đất nào!", "Thông báo");
                return;
            }

            using (Form selectForm = new Form())
            {
                selectForm.Text = "Quản lý tài sản";
                selectForm.Width = 350;
                selectForm.Height = 250;

                ListBox listBox = new ListBox { Dock = DockStyle.Top, Height = 120 };
                foreach (var tile in ownedTiles)
                {
                    listBox.Items.Add($"{tile.Name} (Cấp nhà: {tile.Level})");
                }

                Button btnDestroy = new Button { Text = "🔨 Phá 1 cấp nhà", Dock = DockStyle.Top, Height = 40 };
                Button btnSell = new Button { Text = "💰 Bán đất & nhà", Dock = DockStyle.Top, Height = 40 };

                btnDestroy.Click += (s, e) =>
                {
                    if (listBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("Vui lòng chọn một ô đất!");
                        return;
                    }

                    var selectedTile = ownedTiles[listBox.SelectedIndex];

                    if (selectedTile.Level == 0)
                    {
                        MessageBox.Show("Ô này không có nhà để phá!");
                        return;
                    }

                    selectedTile.DestroyOneHouseLevel(); // dùng hàm trong lớp Tile

                    string msg = $"Đã phá 1 cấp nhà tại ô {selectedTile.Name}. Cấp hiện tại: {selectedTile.Level}";

                    if (selectedTile.Level == 0)
                    {
                        msg += "\nĐã mất quyền sở hữu ô này.";
                    }

                    UpdateTileDisplay(selectedTile.Id, player);
                    listBox.Items[listBox.SelectedIndex] = $"{selectedTile.Name} (Cấp nhà: {selectedTile.Level})";
                    MessageBox.Show(msg, "Kết quả");
                };

                btnSell.Click += (s, e) =>
                {
                    if (listBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("Vui lòng chọn một ô đất!");
                        return;
                    }

                    var selectedTile = ownedTiles[listBox.SelectedIndex];

                    int refund = selectedTile.SellLandAndHouses(); // dùng hàm trong lớp Tile
                    player.AddMoney(refund);

                    UpdateTileDisplay(selectedTile.Id, player);
                    MessageBox.Show($"Bạn đã bán {selectedTile.Name} và nhận ${refund}", "Đã bán");

                    listBox.Items.RemoveAt(listBox.SelectedIndex);
                    ownedTiles.Remove(selectedTile);
                };

                selectForm.Controls.Add(btnSell);
                selectForm.Controls.Add(btnDestroy);
                selectForm.Controls.Add(listBox);

                selectForm.ShowDialog();
            }
        }
        private void CheckPlayerBankruptcy(Player player)
        {
            if (player.Money < 0 && !player.IsBankrupt)
            {
                // 1. Kiểm tra tổng giá trị tài sản
                int totalAssets = CalculatePlayerAssets(player);

                if (player.Money + totalAssets < 0)
                {
                    // 2. Xử lý phá sản
                    ProcessBankruptcy(player);
                }
                else
                {
                    // 3. Yêu cầu bán tài sản
                    ForceSellAssets(player);
                }
            }
        }
        private int CalculatePlayerAssets(Player player)
        {
            int total = 0;
            foreach (var tile in tiles.Where(t => t.PlayerId == player.ID))
            {
                total += tile.LandPrice;
                if (tile.Level > 0)
                {
                    total += tile.HousePrice * (tile.Level < 5 ? tile.Level : 3); // 3 nhà + 1 khách sạn
                    if (tile.Level == 5) total += tile.HotelPrice;
                }
            }
            return total;
        }
        private void ProcessBankruptcy(Player player)
        {
            // 1. Đánh dấu người chơi phá sản
            player.DeclareBankruptcy();

            // 2. Trả lại tất cả tài sản cho ngân hàng (set PlayerId = null)
            foreach (var tile in tiles.Where(t => t.PlayerId == player.ID))
            {
                tile.PlayerId = null;
                tile.Level = 0;
                UpdateTileDisplay(tiles.IndexOf(tile), players[currentPlayerIndex]);
            }

            // 3. Xóa marker người chơi
            if (playerMarkers.ContainsKey(player.ID))
            {
                var marker = playerMarkers[player.ID];
                panels[player.TileIndex].Controls.Remove(marker);
                playerMarkers.Remove(player.ID);
            }

            // 4. Cập nhật UI
            UpdatePlayerPanel(GetPlayerPanel(player.ID), player);

            // 5. Thông báo
            MessageBox.Show($"Người chơi {player.ID} đã phá sản và rời khỏi game!", "Phá sản");

            // 6. Loại khỏi danh sách người chơi hiện tại
            players.Remove(player);

            // 7. Kiểm tra kết thúc game
            CheckGameEnd();
        }
        private void ForceSellAssets(Player player)
        {
            var ownedTiles = tiles.Where(t => t.PlayerId == player.ID && t.Level > 0)
                                 .OrderByDescending(t => t.Level) // Bán nhà/hotel trước
                                 .ThenByDescending(t => t.LandPrice) // Sau đó bán đất giá cao trước
                                 .ToList();

            if (!ownedTiles.Any())
            {
                // Không còn tài sản để bán
                ProcessBankruptcy(player);
                return;
            }

            using (var form = new Form())
            {
                form.Text = "Bạn cần bán tài sản để trả nợ";
                form.Width = 400;
                form.Height = 300;

                var label = new Label
                {
                    Text = $"Bạn đang nợ ${-player.Money}. Vui lòng chọn tài sản để bán:",
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var listBox = new ListBox
                {
                    Dock = DockStyle.Fill,
                    DisplayMember = "DisplayText"
                };

                foreach (var tile in ownedTiles)
                {
                    int sellValue = CalculateSellValue(tile);
                    listBox.Items.Add(new
                    {
                        Tile = tile,
                        DisplayText = $"{tile.Name} (Cấp {tile.Level}) - Bán được ${sellValue}",
                        Value = sellValue
                    });
                }

                var btnSell = new Button
                {
                    Text = "Bán tài sản đã chọn",
                    Dock = DockStyle.Bottom,
                    Height = 40
                };

                btnSell.Click += (s, e) =>
                {
                    if (listBox.SelectedItem == null) return;

                    dynamic selected = listBox.SelectedItem;
                    Tile tile = selected.Tile;
                    int sellValue = selected.Value;

                    // Bán tài sản
                    tile.SellLandAndHouses();
                    player.Money += sellValue;

                    // Cập nhật hiển thị
                    UpdateTileDisplay(tiles.IndexOf(tile), player);
                    UpdatePlayerPanel(GetPlayerPanel(player.ID), player);

                    // Đóng form nếu đủ tiền
                    if (player.Money >= 0)
                    {
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    else
                    {
                        // Làm mới danh sách
                        listBox.Items.Clear();
                        var newList = tiles.Where(t => t.PlayerId == player.ID && t.Level > 0)
                                         .OrderByDescending(t => t.Level)
                                         .ThenByDescending(t => t.LandPrice)
                                         .ToList();

                        if (!newList.Any())
                        {
                            ProcessBankruptcy(player);
                            form.Close();
                            return;
                        }

                        foreach (var t in newList)
                        {
                            int value = CalculateSellValue(t);
                            listBox.Items.Add(new
                            {
                                Tile = t,
                                DisplayText = $"{t.Name} (Cấp {t.Level}) - Bán được ${value}",
                                Value = value
                            });
                        }
                    }
                };

                form.Controls.Add(listBox);
                form.Controls.Add(label);
                form.Controls.Add(btnSell);

                form.ShowDialog();
            }
        }
        private int CalculateSellValue(Tile tile)
        {
            // Hoàn 50% giá trị đã đầu tư
            int value = tile.LandPrice / 2;

            if (tile.Level > 0)
            {
                if (tile.Level == 5) // Hotel
                    value += (tile.HousePrice * 3 + tile.HotelPrice) / 2;
                else
                    value += (tile.HousePrice * tile.Level) / 2;
            }

            return value;
        }
        private void CheckGameEnd()
        {
            if (players.Count == 1)
            {
                MessageBox.Show($"Người chơi {players[0].ID} đã chiến thắng!", "Game kết thúc");
                // Disable các nút điều khiển game
                button1.Enabled = false;
                // Có thể thêm nút "Chơi lại"
            }
        }
    }
}
