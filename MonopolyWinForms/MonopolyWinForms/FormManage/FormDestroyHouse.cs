using MonopolyWinForms.GameLogic;
using System.Data;

namespace MonopolyWinForms.FormManage
{
    public partial class FormDestroyHouse : Form
    {
        public List<Tile> ownedTiles;
        public Player player;
        private MainForm mainForm;
        public bool CanOpen { get; private set; } = true;
        public FormDestroyHouse(Player player, List<Tile> tiles, MainForm mainForm)
        {
            InitializeComponent();
            this.player = player;
            this.mainForm = mainForm;
            // Lọc các ô đất của người chơi có nhà > 0
            ownedTiles = tiles.Where(t => t.PlayerId == player.ID && t.Level > 0).ToList();
            if (ownedTiles.Count == 0)
            {
                MessageBox.Show("Bạn không có ô đất nào có nhà để phá!", "Thông báo");
                CanOpen = false;
                return;
            }
            // Setup ListBox
            foreach (var tile in ownedTiles)
            {
                listBoxTiles.Items.Add($"{tile.Name} (Cấp nhà: {tile.Level})");
            }
            btnDestroy.Text = "🔨 Phá 1 cấp nhà";
            btnDestroy.Click += btnDestroy_Click;
            this.mainForm = mainForm;
        }
        private void btnDestroy_Click(object? sender, EventArgs e)
        {
            if (listBoxTiles.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn một ô đất!");
                return;
            }
            var selectedTile = ownedTiles[listBoxTiles.SelectedIndex];
            selectedTile.DestroyOneHouseLevel();
            string msg = $"Đã phá 1 cấp nhà tại ô {selectedTile.Name}. Cấp hiện tại: {selectedTile.Level}";
            if (selectedTile.Level == 0)
            {
                selectedTile.PlayerId = null;
                msg += "\nĐã mất quyền sở hữu ô này.";
            }
            mainForm.UpdateTileDisplay(selectedTile.Id - 1, player);
            listBoxTiles.Items[listBoxTiles.SelectedIndex] = $"{selectedTile.Name} (Cấp nhà: {selectedTile.Level})";
            MessageBox.Show(msg, "Kết quả");
            this.Close();
        }
    }
}
