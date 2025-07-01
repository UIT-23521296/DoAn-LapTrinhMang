using Firebase.Database;
using MonopolyWinForms.FormManage;
using MonopolyWinForms.GameLogic;
using MonopolyWinForms.Services;
using MonopolyWinForms.Login_Signup;
using MonopolyWinForms.Room;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MonopolyWinForms
{
    public partial class MainForm : Form
    {
        //Khởi tạo
        public Panel[] panels;
        private Label lblCurrentTurn;
        public int currentPlayerIndex = 0;
        
        private Chatbox? chatbox;
        private BankruptcyManager BankManager;
        private CountdownClock? countdown;
        private InitializePlayerMarker Initialize;
        public Monopoly monopoly;
        public List<Player> players;        
        private RentCalculator rentCalculator;
        private TileActionHandler? Action;
        public List<Tile> tiles;
        private UpdateTileDisplay UpdateTile;
        private UpdatePlayerPanel UpdatePlayer;
        private static bool _globalPlayerLeftHandled = false;
        private static JoinRoom? _joinRoomInstance = null;


        public Random random = new Random();
        public Dictionary<int, Panel> playerMarkers = new Dictionary<int, Panel>();
        public readonly Dictionary<string, Color> monopolyColors = new()
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
        //Chatbox
        private void InitializeChatBox(){
            chatbox = new Chatbox(players[currentPlayerIndex]);
            chatbox.Location = new Point(1230, 670);
            chatbox.OnSendMessage += HandleChatMessage;
            this.Controls.Add(chatbox);
        }

        private void InitializeTurnLabel()
        {
            lblCurrentTurn = new Label
            {
                Name = "lblCurrentTurn",
                AutoSize = true,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(1230, 100), 
                Text = "Lượt chơi: "
            };
            this.Controls.Add(lblCurrentTurn);
        }
        private async void HandleChatMessage(string senderName, string message)
        {
            try
            {
                // Tạo đối tượng chat message
                await GameManager.SendChatMessage(
                    GameManager.CurrentRoomId!,
                    senderName,
                    message
                );
            }
            catch (Exception ex)
            {
                AddToGameLog($"Lỗi gửi tin nhắn: {ex.Message}", LogType.Error);
            }
        }
        private async void GameOver()
        {
            try
            {
                MessageBox.Show("⏰ Hết thời gian! Trò chơi kết thúc!", "Thông báo");

                // Hiển thị kết quả game
                var resultForm = new GameResultForm(players, tiles);
                resultForm.ShowDialog();

                await GameManager.SendChatMessage(
                    GameManager.CurrentRoomId!,
                    "Hệ thống",
                    "Trò chơi đã kết thúc!"
                );

                // Dọn dẹp dữ liệu game
                await GameManager.CleanupGameData(GameManager.CurrentRoomId);

                this.Hide();
                var jr = Application.OpenForms.OfType<JoinRoom>().FirstOrDefault();
                if (jr == null) new JoinRoom().Show();
                else jr.Activate();
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error in GameOver: {ex.Message}\n");
            }
        }

        public MainForm()
        {
            InitializeComponent(); 
            GameManager.OnGameStateUpdated += HandleGameStateUpdate;
            GameManager.OnChatMessageReceived += HandleChatMessageReceived;
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
            UpdatePlayer = new UpdatePlayerPanel();
            UpdateTile = new UpdateTileDisplay(panels, tiles, this, monopoly, players);
            UpdatePlayer = new UpdatePlayerPanel();
            Initialize = new InitializePlayerMarker(panels, playerMarkers, this);
            Action = new TileActionHandler(players, tiles, panels, currentPlayerIndex, monopoly, this, random, playerMarkers);
            rentCalculator = new RentCalculator(monopoly);
            BankManager = new BankruptcyManager(players, tiles, panels, playerMarkers, this, currentPlayerIndex);
            
            // Thêm event handler cho FormClosing
            this.FormClosing += MainForm_FormClosing;

            // Đăng ký event handler cho sự kiện người chơi thoát
            GameManager.OnPlayerLeft += HandlePlayerLeft;
        }

        private void HandleGameStateUpdate(GameState gameState)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateGameState(gameState)));
            }
            else
            {
                UpdateGameState(gameState);
            }
        }
        public async Task UpdateGameState(GameState gameState)
        {
            //gameState = await GameManager.GetLatestGameState();
            File.AppendAllText("log.txt", $"Tôi là {Session.UserName} thấy người chơi " +
                $"{players[currentPlayerIndex]} đi tới ô {players[currentPlayerIndex].TileIndex}\n");
            try
            {
                // Cập nhật currentPlayerIndex
                if (currentPlayerIndex != gameState.CurrentPlayerIndex)
                {
                    currentPlayerIndex = gameState.CurrentPlayerIndex;
                    UpdateTurnDisplay();
                    chatbox?.UpdatePlayer(players[currentPlayerIndex]);
                    if(Action != null) Action.UpdateCurrentPlayerIndex(currentPlayerIndex);
                }

                // Cập nhật thông tin cho tất cả người chơi
                for (int i = 0; i < players.Count; i++)
                {
                    var remotePlayer = gameState.Players[i];
                    var localPlayer = players[i];
                    
                    // Kiểm tra xem có thay đổi không
                    if (localPlayer.Money != remotePlayer.Money ||
                        localPlayer.IsInJail != remotePlayer.IsInJail ||
                        localPlayer.IsBankrupt != remotePlayer.IsBankrupt ||
                        localPlayer.OutPrison != remotePlayer.OutPrison ||
                        localPlayer.ReduceHalfMoney != remotePlayer.ReduceHalfMoney ||
                        localPlayer.DoubleMoney != remotePlayer.DoubleMoney ||
                        localPlayer.DoubleDices != remotePlayer.DoubleDices ||
                        localPlayer.JailTurnCount != remotePlayer.JailTurnCount)
                    {
                        // Cập nhật thông tin người chơi
                        localPlayer.Money = remotePlayer.Money;
                        localPlayer.IsInJail = remotePlayer.IsInJail;
                        localPlayer.IsBankrupt = remotePlayer.IsBankrupt;
                        localPlayer.OutPrison = remotePlayer.OutPrison;
                        localPlayer.ReduceHalfMoney = remotePlayer.ReduceHalfMoney;
                        localPlayer.DoubleMoney = remotePlayer.DoubleMoney;
                        localPlayer.DoubleDices = remotePlayer.DoubleDices;
                        localPlayer.JailTurnCount = remotePlayer.JailTurnCount;

                        // Cập nhật UI của người chơi có thay đổi
                        UpdatePlayer.UpdatePlayerPanelUI(GetPlayerPanel(localPlayer.ID), localPlayer);
                    }

                    // Cập nhật vị trí người chơi nếu có thay đổi
                    if (localPlayer.TileIndex != remotePlayer.TileIndex)
                    {
                        // Tính số bước cần di chuyển
                        int steps = (remotePlayer.TileIndex - localPlayer.TileIndex + 40) % 40;
                        if (steps > 0)
                        {
                            // Di chuyển từng bước cho tất cả người chơi
                            _ = MovePlayerStepByStep(localPlayer, steps, 40).ContinueWith(_ => {
                                // Chỉ cập nhật TileIndex sau khi hoàn thành animation
                                localPlayer.TileIndex = remotePlayer.TileIndex;
                                UpdatePlayerMarkerPosition(localPlayer, remotePlayer.TileIndex);
                            });
                            
                        }
                    }
                }

                // Cập nhật tiles có thay đổi
                for (int i = 0; i < tiles.Count; i++)
                {
                    var remoteTile = gameState.Tiles[i];
                    var localTile = tiles[i];
                    
                    // Kiểm tra xem có thay đổi không
                    if (localTile.PlayerId != remoteTile.PlayerId ||
                        localTile.LandPrice != remoteTile.LandPrice ||
                        localTile.HousePrice != remoteTile.HousePrice ||
                        localTile.HotelPrice != remoteTile.HotelPrice ||
                        localTile.Level != remoteTile.Level ||
                        localTile.RentPrice != remoteTile.RentPrice ||
                        localTile.Monopoly != remoteTile.Monopoly)
                    {
                        // Cập nhật thông tin ô đất
                        localTile.PlayerId = remoteTile.PlayerId;
                        localTile.LandPrice = remoteTile.LandPrice;
                        localTile.HousePrice = remoteTile.HousePrice; 
                        localTile.HotelPrice = remoteTile.HotelPrice;
                        localTile.Level = remoteTile.Level;
                        localTile.RentPrice = remoteTile.RentPrice;
                        localTile.Monopoly = remoteTile.Monopoly;

                        // Chỉ cập nhật UI của ô đất có thay đổi
                        UpdateTile.UpdateTileDisplayUI(i, players[currentPlayerIndex]);
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error updating game state: {ex.Message}\n");
            }
        }


        private async void MainForm_Load(object sender, EventArgs e)
        {
            InitializeTurnLabel();
            countdown = new CountdownClock(panelTimer, GameOver);
            int indexPanel = 0;
            int indexPlayer = 1;
            List<Panel> playerPanels = new List<Panel> { panel41, panel42, panel43, panel44 };
            
            foreach (string playerName in GameManager.Players)
            {
                Player player = new Player(indexPlayer, 2000, playerName);
                if (playerName == Session.UserName) // Nếu là người chơi hiện tại
                {
                    Session.PlayerInGameId = indexPlayer;
                    Session.Color = GetPlayerColor(indexPlayer - 1);
                }
                players.Add(player);
                UpdatePlayer.UpdatePlayerPanelUI(playerPanels[indexPanel++], player);
                indexPlayer++;
            }

            for (int i = 0; i < panels.Length && i < tiles.Count; i++){
               panels[i].Tag = tiles[i];
               UpdateTile.UpdateTileDisplayUI(i, players[currentPlayerIndex]);
            }
            foreach (var player in players){
                player.TileIndex = 0;
                Initialize.InitializePlayerMarkerUI(player);
            }
            InitializeChatBox();
            UpdateTurnDisplay();
            try 
            {
                GameState gameStates = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                await GameManager.UpdateGameState(gameStates);
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error in MainForm_Load: {ex.Message}\n");
            }

            //_ = Task.Run(async () =>
            //{
            //    await Task.Delay(3000);
            //    countdown.Start(GameManager.PlayTime);
            //});

            countdown.Start(GameManager.PlayTime);
        }
        private async void button1_Click(object sender, EventArgs e){
            var handler = new DiceRollHandler(players, panels, this, currentPlayerIndex, tiles);
            await handler.RollDiceAndMoveAsync();
        }
        // VÙNG GỌI HÀM
        public void NextTurn(){
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            UpdateTurnDisplay();
            chatbox!.UpdatePlayer(players[currentPlayerIndex]);
            if(Action != null) Action.UpdateCurrentPlayerIndex(currentPlayerIndex);
            //button1.Enabled = true;

            var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
            GameManager.UpdateGameState(gameState);
        }

        private void UpdateTurnDisplay()
        {
            if (lblCurrentTurn != null)
            {
                Player currentPlayer = players[currentPlayerIndex];
                lblCurrentTurn.Text = $"Lượt chơi: {currentPlayer.Name}";
                
                // Có thể thêm hiệu ứng hoặc màu sắc để làm nổi bật
                lblCurrentTurn.ForeColor = GetPlayerColor(currentPlayerIndex);

                button1.Enabled = (Session.PlayerInGameId == currentPlayer.ID);

                //Thông báo lượt chơi
                if (Session.PlayerInGameId == currentPlayer.ID)
                {
                    AddToGameLog($"Đến lượt của bạn!", LogType.Notification);
                }
                else
                {
                    AddToGameLog($"Đến lượt của {currentPlayer.Name}", LogType.System);
                }
            }
        }

        private Color GetPlayerColor(int playerIndex)
        {
            // Màu sắc tương ứng với từng người chơi
            return playerIndex switch
            {
                0 => Color.Blue,
                1 => Color.YellowGreen,
                2 => Color.Red,
                3 => Color.Green,
                _ => Color.Black
            };
        }
        public Panel GetPlayerPanel(int playerId){
            return playerId switch
            {
                1 => panel41,
                2 => panel42,
                3 => panel43,
                4 => panel44,
                _ => panel1
            };
        }
        public void UpdateTileDisplay(int tileIndex, Player player)
        { 
            UpdateTile.UpdateTileDisplayUI(tileIndex, player);

            if (Session.PlayerInGameId != player.ID)
            {
                var tile = tiles[tileIndex];
                if (tile.Level > 0)
                {
                    string buildingType = tile.Level == 5 ? "khách sạn" : "nhà";
                    AddToGameLog($"{player.Name} đã xây {buildingType} trên {tile.Name}", LogType.System);
                }
            }
        }           
        public void UpdatePlayerPanel(Player player){ UpdatePlayer.UpdatePlayerPanelUI(GetPlayerPanel(player.ID), player); }
        public void DisableRollButton(){ button1.Enabled = false; }
        public int CalculateRent(Tile tile, int playerId){ return rentCalculator.CalculateRent(tile , playerId); }
        public int CalculatePlayerAssets(Player player){ return BankManager.CalculatePlayerAssets(player); }
        public void UpdateCompanyRent(int playerId){ UpdateTile.UpdateCompanyRent(playerId); }
        public void UpdateBusStationRent(int playerId){ UpdateTile.UpdateBusStationRent(playerId); }
        public void SubtractMoney (int money, Player player)
        {
            Property Money = new Property(player,this);
            Money.SubtractMoney(money);


            //Ghi log người khác
            if (Session.PlayerInGameId != player.ID)
            {
                AddToGameLog($"{player.Name} đã trả {money}$", LogType.System);
            }
        }
        public void AddMoney(int money, Player player)
        {
            Property Money = new Property(player,this);
            Money.AddMoney(money);

            //Ghi log cho người khác
            if (Session.PlayerInGameId != player.ID)
            {
                AddToGameLog($"{player.Name} đã nhận {money}$", LogType.System);
            }
        }
        public void ShowTileActionForm(Tile tile, Player currentPlayer)
        { 
            if (Action != null)
            {
                if (Session.PlayerInGameId == currentPlayer.ID)
                {
                    Action.ShowTileActionForm(tile, currentPlayer);
                }
                else
                {
                    string actionMessage = GetTileActionMessage(tile, currentPlayer);
                    AddToGameLog(actionMessage, LogType.System);
                }
            }
        }
        public void UpdatePlayerMarkerPosition(Player player, int newIndex){ Initialize.UpdatePlayerMarkerPosition(player,newIndex); }
        public void CheckPlayerBankruptcy(Player player){ BankManager.CheckPlayerBankruptcy(player); }
        public void ForceSellAssets(Player player){ BankManager.ForceSellAssets(player); }
        public void HandleStart(Player player){ Action?.HandleStart(player); }
        public async Task MovePlayerStepByStep(Player player, int steps, int totalTiles){ await Initialize.MovePlayerStepByStep(player, steps, totalTiles); }
        public Image GetHouseImage(int level, Player player, Tile tile, List<Player> players){return UpdateTile.GetHouseImage(level, player, players, tile);
        }

        public void AddToGameLog(string message, LogType type = LogType.System)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddToGameLog(message, type)));
                return;
            }

            switch (type)
            {
                case LogType.System:
                    chatbox?.AddSystemMessage(message);
                    break;
                case LogType.Notification:
                    chatbox?.AddNotificationMessage(message);
                    break;
                case LogType.Warning:
                    chatbox?.AddWarningMessage(message);
                    break;
                case LogType.Error:
                    chatbox?.AddErrorMessage(message);
                    break;
            }
        }

        // Enum để phân loại log
        public enum LogType
        {
            System,
            Notification,
            Warning,
            Error
        }

        // Thêm hàm để nhận tin nhắn từ Firebase
        public void HandleChatMessageReceived(string senderName, string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleChatMessageReceived(senderName, message)));
                return;
            }

            // Nếu nhận được thông báo người chơi thoát
            if (senderName == "SYSTEM" && message.StartsWith("LEFT:"))
            {
                string leaverName = message.Substring(5);
                HandlePlayerLeft(leaverName);
                return;
            }

            // Còn lại là tin nhắn bình thường
            chatbox?.ReceiveMessage(senderName, message);
        }


        //Xử lý ghi log cho người chơi khác liên quan đến ô đất
        private string GetTileActionMessage(Tile tile, Player player)
        {
            // Kiểm tra loại ô đất dựa vào Monopoly
            switch (tile.Monopoly)
            {
                case "0": // Ô đặc biệt
                    switch (tile.Name)
                    {
                        case "Thuế thu nhập":
                        case "Thuế đặc biệt":
                            return $"{player.Name} phải đóng thuế {tile.LandPrice}$";
                        case "Nhà tù":
                            return $"{player.Name} đang ở trong tù";
                        case "Ô bắt đầu":
                            return $"{player.Name} đi qua ô Start và nhận 200$";
                        case "Khí vận":
                        case "Cơ hội":
                            return "";
                        case "Đi thẳng vào tù":
                            return $"{player.Name} bị vào tù";
                        default:
                            return $"{player.Name} đã đến {tile.Name}";
                    }
                case "9": // Bến xe
                    if (tile.PlayerId.HasValue && tile.PlayerId.Value > 0)
                    {
                        var owner = players.FirstOrDefault(p => p.ID == tile.PlayerId.Value);
                        if (owner != null)
                        {
                            return $"{player.Name} phải trả tiền thuê {tile.RentPrice}$ cho {owner.Name} tại bến xe {tile.Name}";
                        }
                    }
                    return $"{player.Name} đã đến bến xe {tile.Name}";
                case "10": // Công ty
                    if (tile.PlayerId.HasValue && tile.PlayerId.Value > 0)
                    {
                        var owner = players.FirstOrDefault(p => p.ID == tile.PlayerId.Value);
                        if (owner != null)
                        {
                            return $"{player.Name} phải trả tiền thuê {tile.RentPrice}$ cho {owner.Name} tại công ty {tile.Name}";
                        }
                    }
                    return $"{player.Name} đã đến công ty {tile.Name}";
                default: // Ô đất thường
                    if (tile.PlayerId.HasValue)
                    {
                        if (tile.PlayerId.Value == player.ID)
                        {
                            return $"{player.Name} đã sở hữu {tile.Name}";
                        }
                        else if (tile.PlayerId.Value > 0)
                        {
                            var owner = players.FirstOrDefault(p => p.ID == tile.PlayerId.Value);
                            if (owner != null)
                            {
                                string buildingInfo = tile.Level > 0 
                                    ? $" (có {tile.Level} {(tile.Level == 5 ? "khách sạn" : "nhà")})" 
                                    : "";
                                return $"{player.Name} phải trả tiền thuê {tile.RentPrice}$ cho {owner.Name} tại {tile.Name}{buildingInfo}";
                            }
                        }
                    }
                    else if (tile.LandPrice > 0)
                    {
                        return $"{player.Name} có thể mua {tile.Name} với giá {tile.LandPrice}$";
                    }
                    return $"{player.Name} đã đến {tile.Name}";
            }
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!GameManager.IsGameStarted) return;

            var ask = MessageBox.Show(
                "Bạn có chắc muốn thoát game? Điều này sẽ kết thúc game cho tất cả người chơi.",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (ask == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            countdown?.Stop();
            GameManager.OnPlayerLeft -= HandlePlayerLeft;

            await GameManager.NotifyPlayerLeft(GameManager.CurrentRoomId, Session.UserName);
            Session.LeaveRoom();

            this.Hide();
            if (_joinRoomInstance == null || _joinRoomInstance.IsDisposed)
            {
                _joinRoomInstance = new JoinRoom();
                _joinRoomInstance.StartPosition = FormStartPosition.CenterScreen;
                _joinRoomInstance.Show();
            }
            else
            {
                _joinRoomInstance.WindowState = FormWindowState.Normal;
                _joinRoomInstance.BringToFront();
                _joinRoomInstance.Activate();
            }

            e.Cancel = true;
        }

        private bool _playerLeftHandled = false;

        private void HandlePlayerLeft(string playerName)
        {
            if (GlobalFlags.PlayerLeftHandled) return;
            GlobalFlags.PlayerLeftHandled = true;

            if (InvokeRequired)
            {
                Invoke(() => HandlePlayerLeft(playerName));
                return;
            }

            countdown?.Stop();
            GameManager.OnPlayerLeft -= HandlePlayerLeft;

            MessageBox.Show($"{playerName} đã thoát khỏi trò chơi. Trò chơi kết thúc!", "Thông báo");

            this.Hide();

            if (_joinRoomInstance == null || _joinRoomInstance.IsDisposed)
            {
                _joinRoomInstance = new JoinRoom();
                _joinRoomInstance.StartPosition = FormStartPosition.CenterScreen;
                _joinRoomInstance.Show();
            }
            else
            {
                _joinRoomInstance.WindowState = FormWindowState.Normal;
                _joinRoomInstance.BringToFront();
                _joinRoomInstance.Activate();
            }
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            
            // Hủy đăng ký event khi form đóng
            GameManager.OnPlayerLeft -= HandlePlayerLeft;
            
            // Đảm bảo tất cả timer và event được dọn dẹp
            if (countdown != null)
            {
                countdown.Stop();
            }
                
            // Reset session
            Session.LeaveRoom();
        }
    }
}