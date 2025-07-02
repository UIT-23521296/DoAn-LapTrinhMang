using MonopolyWinForms.GameLogic;
using System.Data;
using MonopolyWinForms.Services;

namespace MonopolyWinForms.FormManage
{
    public partial class FormSellProperty : Form
    {
        public List<Tile> ownedTiles;
        public List<Tile> GameStateTiles;
        public List<Player> players;
        public int currentPlayerIndex;
        public Player player;
        private MainForm mainForm;
        public bool CanOpen { get; private set; } = true;
        public FormSellProperty(Player player, List<Tile> tiles, MainForm mainForm, List<Player> players, int currentPlayerIndex)
        {
            InitializeComponent();
            this.player = player;
            this.mainForm = mainForm;
            this.players = players;
            this.GameStateTiles = tiles;
            this.currentPlayerIndex = currentPlayerIndex;
            // Lọc các ô đất của người chơi (có thể có nhà hoặc không)
            ownedTiles = tiles.Where(t => t.PlayerId == player.ID).ToList();
            if (ownedTiles.Count == 0)
            {
                MessageBox.Show("Bạn không sở hữu ô đất nào!", "Thông báo");
                CanOpen = false;
                return;
            }
            // Setup ListBox
            foreach (var tile in ownedTiles)
            {
                listBoxTiles.Items.Add($"{tile.Name} (Cấp nhà: {tile.Level})");
            }
            btnSell.Text = "💰 Bán đất & nhà";
            btnSell.Click += BtnSell_Click;
        }
        private async void BtnSell_Click(object? sender, EventArgs e)
        {
            try
            {
                btnSell.Enabled = false;
                if (listBoxTiles.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn một ô đất!");
                    btnSell.Enabled = true;
                    return;
                }
                var selectedTile = ownedTiles[listBoxTiles.SelectedIndex];
                int refund = selectedTile.SellLandAndHousesLocal();
                mainForm.AddMoney(refund, player);
                mainForm.UpdateTileDisplay(selectedTile.Id - 1, player);
                var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, GameStateTiles);
                await GameManager.UpdateGameState(gameState);
                MessageBox.Show($"Bạn đã bán {selectedTile.Name} và nhận ${refund}", "Đã bán");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi bán nhà: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSell.Enabled = true;
            }
        }
    }
}
