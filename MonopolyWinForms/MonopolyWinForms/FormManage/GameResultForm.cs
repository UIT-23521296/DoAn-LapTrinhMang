using MonopolyWinForms.GameLogic;


namespace MonopolyWinForms.FormManage
{
    public class GameResultForm : Form
    {
        public GameResultForm(List<Player> players, List<Tile> tiles)
        {
            Text = "🏆 Kết quả trò chơi";
            Size = new Size(550, 250);
            StartPosition = FormStartPosition.CenterScreen;

            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true
            };

            listView.Columns.Add("Thứ hạng", 100);
            listView.Columns.Add("Người chơi", 120);
            listView.Columns.Add("Tổng tài sản ($)", 140);
            listView.Columns.Add("Trạng thái", 140);

            // Tính tổng tài sản của mỗi người
            var playerAssets = players.Select(p => new
            {
                Player = p,
                TotalAssets = p.Money + tiles.Where(t => t.PlayerId == p.ID).Sum(t =>
                    t.LandPrice + t.Level * t.HousePrice)
            })
            .OrderByDescending(p => p.TotalAssets)
            .ToList();

            int rank = 1;
            foreach (var p in playerAssets)
            {
                string status = p.Player.IsBankrupt || p.TotalAssets == 0 ? "Phá sản" : "Còn tài sản";
                ListViewItem item = new ListViewItem(rank.ToString());
                item.SubItems.Add(p.Player.ID.ToString());
                item.SubItems.Add(p.TotalAssets.ToString());
                item.SubItems.Add(status);
                listView.Items.Add(item);
                rank++;
            }

            Controls.Add(listView);
        }

        private void InitializeComponent()
        {

        }
    }
}
