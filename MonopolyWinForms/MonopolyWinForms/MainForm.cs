using MonopolyWinForms.FormManage;
using MonopolyWinForms.GameLogic;
namespace MonopolyWinForms
{
    public partial class MainForm : Form
    {
        //Khởi tạo
        public Panel[] panels;
        public int currentPlayerIndex = 0;
        
        private Chatbox? chatbox;
        private BankruptcyManager BankManager;                                                  //Lỗi trừ tiền thuế ko chạy class BankruptcyManager - chưa bik sửa được chưa
        private CountdownClock? countdown;
        private InitializePlayerMarker Initialize;
        public Monopoly monopoly;
        public List<Player> players;        
        private RentCalculator rentCalculator;
        private TileActionHandler? Action;
        public List<Tile> tiles;
        private UpdateTileDisplay UpdateTile;
        private UpdatePlayerPanel UpdatePlayer;

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
            chatbox.Location = new Point(1230, 670); // Tùy chỉnh vị trí
            chatbox.OnSendMessage += HandleChatMessage;
            this.Controls.Add(chatbox);
        }
        private void HandleChatMessage(string message){ Console.WriteLine("Tin nhắn gửi: " + message); }
        private void GameOver(){
            MessageBox.Show("⏰ Hết thời gian! Trò chơi kết thúc!", "Thông báo");
            var resultForm = new GameResultForm(players, tiles);
            resultForm.ShowDialog();
            Application.Exit();
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
            UpdatePlayer = new UpdatePlayerPanel();
            UpdateTile = new UpdateTileDisplay(panels, tiles, this, monopoly);
            UpdatePlayer = new UpdatePlayerPanel();
            Initialize = new InitializePlayerMarker(panels, playerMarkers, this);
            Action = new TileActionHandler(players, tiles, panels, currentPlayerIndex, monopoly, this, random, playerMarkers);
            rentCalculator = new RentCalculator(monopoly);
            BankManager = new BankruptcyManager(players, tiles, panels, playerMarkers, this);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            countdown = new CountdownClock(panelTimer, GameOver);
            countdown.Start(30);
            Player player1 = new Player(1, 2000, "Vũ");
            Player player2 = new Player(2, 2000, "Toàn");
            Player player3 = new Player(3, 2000, "Quang");
            Player player4 = new Player(4, 2000, "Cường");
            players.Add(player1);
            players.Add(player2);
            players.Add(player3);
            players.Add(player4);
            UpdatePlayer.UpdatePlayerPanelUI(panel41, player1);
            UpdatePlayer.UpdatePlayerPanelUI(panel42, player2);
            UpdatePlayer.UpdatePlayerPanelUI(panel43, player3);
            UpdatePlayer.UpdatePlayerPanelUI(panel44, player4);
            for (int i = 0; i < panels.Length && i < tiles.Count; i++){
               panels[i].Tag = tiles[i];
               UpdateTile.UpdateTileDisplayUI(i, players[currentPlayerIndex]);
            }
            foreach (var player in players){
                player.TileIndex = 0;
                Initialize.InitializePlayerMarkerUI(player);
            }
            InitializeChatBox();
        }
        private async void button1_Click(object sender, EventArgs e){
            var handler = new DiceRollHandler(players, panels, this, currentPlayerIndex);
            await handler.RollDiceAndMoveAsync();
        }
        // VÙNG GỌI HÀM
        public void NextTurn(){
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            chatbox!.UpdatePlayer(players[currentPlayerIndex]);
            if(Action != null) Action.UpdateCurrentPlayerIndex(currentPlayerIndex);
            //button1.Enabled = true;
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
        public void UpdateTileDisplay(int tileIndex, Player player) { UpdateTile.UpdateTileDisplayUI(tileIndex, player); }           
        public void UpdatePlayerPanel(Player player){ UpdatePlayer.UpdatePlayerPanelUI(GetPlayerPanel(player.ID), player); }
        public void DisableRollButton(){ button1.Enabled = false; }
        public int CalculateRent(Tile tile, int playerId){ return rentCalculator.CalculateRent(tile , playerId); }
        public int CalculatePlayerAssets(Player player){ return BankManager.CalculatePlayerAssets(player); }
        public void UpdateCompanyRent(int playerId){ UpdateTile.UpdateCompanyRent(playerId); }
        public void UpdateBusStationRent(int playerId){ UpdateTile.UpdateBusStationRent(playerId); }
        public void SubtractMoney (int money, Player player){
            var Money = new Property(player,this);
            Money.SubtractMoney(money); }
        public void AddMoney(int money, Player player){
            var Money = new Property(player,this);
            Money.AddMoney(money); }
        public void ShowTileActionForm(Tile tile, Player currentPlayer){ if (Action != null) Action.ShowTileActionForm(tile,currentPlayer); }
        public void UpdatePlayerMarkerPosition(Player player, int newIndex){ Initialize.UpdatePlayerMarkerPosition(player,newIndex); }
        public void CheckPlayerBankruptcy(Player player){ BankManager.CheckPlayerBankruptcy(player); }
        public void ForceSellAssets(Player player){ BankManager.ForceSellAssets(player); }
        public void HandleStart(Player player){ Action?.HandleStart(player); }
        public async Task MovePlayerStepByStep(Player player, int steps, int totalTiles){ await Initialize.MovePlayerStepByStep(player, steps, totalTiles); }
    }
}