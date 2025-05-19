using MonopolyWinForms.GameLogic;
using System.Data;

namespace MonopolyWinForms.FormManage
{
    public partial class FormSellProperty : Form
    {
        public List<Tile> ownedTiles;
        public Player player;
        private MainForm mainForm;
        public bool CanOpen { get; private set; } = true;
        public FormSellProperty(Player player, List<Tile> tiles, MainForm mainForm)
        {
            InitializeComponent();
            this.player = player;
            this.mainForm = mainForm;
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
        private void BtnSell_Click(object? sender, EventArgs e)
        {
            if (listBoxTiles.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn một ô đất!");
                return;
            }
            var selectedTile = ownedTiles[listBoxTiles.SelectedIndex];
            int refund = selectedTile.SellLandAndHouses();
            mainForm.AddMoney(refund,player);
            selectedTile.PlayerId = null;
            mainForm.UpdateTileDisplay(selectedTile.Id - 1, player);
            MessageBox.Show($"Bạn đã bán {selectedTile.Name} và nhận ${refund}", "Đã bán");
            this.Close();
        }
    }
}
