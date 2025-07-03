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
        public UpdateTileDisplay UpdateTile;
        private UpdatePlayerPanel UpdatePlayer;
        private static bool _globalPlayerLeftHandled = false;
        private static JoinRoom? _joinRoomInstance = null;
        private Panel pnlCurrentTurn;
        private Label lblTurnStatic;   // "Lượt chơi:"
        private Label lblTurnName;     // Tên người chơi

        public static bool GameEnded { get; private set; } = false;
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
        private Dictionary<int, bool> isMoving = new Dictionary<int, bool>();
        //Chatbox
        private void InitializeChatBox(){
            chatbox = new Chatbox(players[currentPlayerIndex]);
            //chatbox.Location = new Point(970, 548);
            chatbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            chatbox.Location = new Point(this.ClientSize.Width - chatbox.Width - 206, this.ClientSize.Height - chatbox.Height + 95);
            chatbox.OnSendMessage += HandleChatMessage;
            this.Controls.Add(chatbox);
        }
        private void InitializeTurnLabel()
        {
            pnlCurrentTurn = new Panel
            {
                //Location = new Point(965, 11),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 701 - 10, 11),
                Size = new Size(330, 42),
                BackColor = ColorTranslator.FromHtml("#EEF7FA"),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(6, 8, 6, 8)  // trên-dưới = 8px
            };

            // Label cố định
            lblTurnStatic = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.Black,
                Text = "Lượt chơi:"
            };

            // Label tên người chơi
            lblTurnName = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,   // sẽ đổi động
                Text = " ..."
            };

            // Sắp xếp 2 label nằm ngang
            lblTurnName.Left = lblTurnStatic.Width + 10;   // cách 10px
            lblTurnName.Top = 0;

            pnlCurrentTurn.Controls.Add(lblTurnStatic);
            pnlCurrentTurn.Controls.Add(lblTurnName);

            // THÊM PANEL vào form (không thêm label đơn lẻ)
            this.Controls.Add(pnlCurrentTurn);
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (chatbox != null)
            {
                chatbox.Location = new Point(this.ClientSize.Width - chatbox.Width - 10, this.ClientSize.Height - chatbox.Height - 10);
            }

            if (pnlCurrentTurn != null)
            {
                pnlCurrentTurn.Location = new Point(this.ClientSize.Width - pnlCurrentTurn.Width - 10, 11);
            }
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
            this.Resize += MainForm_Resize;
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
                        // Nếu đang chạy hiệu ứng thì bỏ qua cập nhật vị trí
                        if (isMoving.ContainsKey(localPlayer.ID) && isMoving[localPlayer.ID]) continue;

                        if (remotePlayer.LastMoveType == MoveType.Teleport)
                        {
                            UpdatePlayerMarkerPosition(localPlayer, remotePlayer.TileIndex);
                            localPlayer.TileIndex = remotePlayer.TileIndex;
                        }
                        else
                        {
                            int steps = (remotePlayer.TileIndex - localPlayer.TileIndex + 40) % 40;
                            if (steps > 0)
                            {
                                isMoving[localPlayer.ID] = true;
                                _ = MovePlayerStepByStep(localPlayer, steps, 40).ContinueWith(_ => {
                                    localPlayer.TileIndex = remotePlayer.TileIndex;
                                    UpdatePlayerMarkerPosition(localPlayer, remotePlayer.TileIndex);
                                    isMoving[localPlayer.ID] = false;
                                });
                            }
                        }
                    }
                }
                if (GameEnded) return;

                foreach (var localPlayer in players)
                {
                    if (monopoly.CheckWin(localPlayer.ID))
                    {
                        await DeclareWinner(localPlayer);
                        return;
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

                // Cập nhật giá thuê bến xe và công ty cho tất cả người chơi
                UpdateTile.UpdateAllRents();
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", $"Error updating game state: {ex.Message}\n");
            }
        }


        private async void MainForm_Load(object sender, EventArgs e)
        {
            InitializeTurnLabel();
            this.BackColor = ColorTranslator.FromHtml("#FBF8F4");
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
        public async Task NextTurn(){
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            UpdateTurnDisplay();
            if (chatbox != null)
            {
                chatbox.UpdatePlayer(players[currentPlayerIndex]);
            }
            if (Action != null) Action.UpdateCurrentPlayerIndex(currentPlayerIndex);
            //button1.Enabled = true;

            var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
            try
            {
                await GameManager.UpdateGameState(gameState);
            }
            catch (Exception ex)
            {
                // ghi log & (nếu muốn) retry nhẹ
                AddToGameLog($"Lỗi cập nhật lượt: {ex.Message}", LogType.Error);
                // Có thể bật lại nút lắc cho chính mình để thử lại
                if (Session.PlayerInGameId == players[currentPlayerIndex].ID)
                    button1.Enabled = true;
            }
        }

        private void UpdateTurnDisplay()
        {
            Player currentPlayer = players[currentPlayerIndex];

            // cập nhật tên & màu
            lblTurnName.Text = " " + currentPlayer.Name;  // thêm cách ở đầu
            lblTurnName.ForeColor = GetPlayerColor(currentPlayerIndex);

            button1.Enabled = (Session.PlayerInGameId == currentPlayer.ID);

            // log
            if (Session.PlayerInGameId == currentPlayer.ID)
                AddToGameLog("Đến lượt của bạn!", LogType.Notification);
            else
                AddToGameLog($"Đến lượt của {currentPlayer.Name}", LogType.System);
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
        public async Task ShowTileActionForm(Tile tile, Player currentPlayer)
        { 
            if (Action != null)
            {
                if (Session.PlayerInGameId == currentPlayer.ID)
                {
                    await Action.ShowTileActionForm(tile, currentPlayer);
                }
                //else
                //{
                //    string actionMessage = GetTileActionMessage(tile, currentPlayer);
                //    AddToGameLog(actionMessage, LogType.System);
                //}
            }
        }
        public void UpdatePlayerMarkerPosition(Player player, int newIndex){ Initialize.UpdatePlayerMarkerPosition(player,newIndex); }
        public void CheckPlayerBankruptcy(Player player){ BankManager.CheckPlayerBankruptcy(player); }
        public void ForceSellAssets(Player player){ BankManager.ForceSellAssets(player); }
        public void HandleStart(Player player){ Action?.HandleStart(player); }
        public async Task MovePlayerStepByStep(Player player, int steps, int totalTiles){ await Initialize.MovePlayerStepByStep(player, steps, totalTiles); }
        public Image GetHouseImage(int level, Player player, Tile tile, List<Player> players){return UpdateTile.GetHouseImage(level, player, players, tile);
        }

        public async void AddToGameLog(string message, LogType type = LogType.System)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddToGameLog(message, type)));
                return;
            }

            //switch (type)
            //{
            //    case LogType.System:
            //        chatbox?.AddSystemMessage(message);
            //        break;
            //    case LogType.Notification:
            //        chatbox?.AddNotificationMessage(message);
            //        break;
            //    case LogType.Warning:
            //        chatbox?.AddWarningMessage(message);
            //        break;
            //    case LogType.Error:
            //        chatbox?.AddErrorMessage(message);
            //        break;
            //}

            // Đẩy log lên Firebase (nếu là SYSTEM và đang trong game)
            if (type == LogType.System && GameManager.IsGameStarted && !string.IsNullOrEmpty(GameManager.CurrentRoomId))
            {
                await GameManager.SendChatMessage(GameManager.CurrentRoomId, "Hệ thống", message);
            }
        }

        public void AddDiceLog(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddDiceLog(msg)));
                return;
            }

            richTextBox1.Clear();

            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}{Environment.NewLine}");
            richTextBox1.ScrollToCaret();
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

            if (senderName == "Xúc sắc")
            {
                AddDiceLog(message);
                return;
            }

            // Còn lại là tin nhắn bình thường
            chatbox?.ReceiveMessage(senderName, message);
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
        public async Task DeclareWinner(Player winner)
        {
            if (GameEnded) return;          // tránh gọi lặp
            GameEnded = true;

            countdown?.Stop();
            button1.Enabled = false;

            MessageBox.Show($"{winner.Name} đã chiến thắng trò chơi!", "Chiến thắng");

            await GameManager.SendChatMessage(
                GameManager.CurrentRoomId!,
                "Hệ thống",
                $"{winner.Name} đã chiến thắng trò chơi!"
            );

            await GameManager.CleanupGameData(GameManager.CurrentRoomId);

            var resultForm = new GameResultForm(players, tiles);
            resultForm.ShowDialog();

            this.Hide();
            (new JoinRoom()).Show();
        }
    }
}