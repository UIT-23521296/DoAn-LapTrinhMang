using buyLand_Home;
using MonopolyWinForms.BuyLand_Home;
using MonopolyWinForms.FormManage;
using System.Security.Policy;

namespace MonopolyWinForms.GameLogic
{
    public class TileActionHandler
    {
        private List<Player> players;
        private List<Tile> tiles;
        private Panel[] panels;
        private int currentPlayerIndex;
        private Dictionary<int, Panel> playerMarkers;
        private Monopoly monopoly;
        private MainForm mainForm;
        private Random random;
        private BankruptcyManager BankManager;
        public TileActionHandler(List<Player> players, List<Tile> tiles, Panel[] panels, int currentPlayerIndex, Monopoly monopoly,
            MainForm mainForm, Random random, Dictionary<int, Panel> playerMarkers)
        {
            this.players = players;
            this.tiles = tiles;
            this.panels = panels;
            this.currentPlayerIndex = currentPlayerIndex;
            this.monopoly = monopoly;
            this.mainForm = mainForm;
            this.random = random;
            this.playerMarkers = playerMarkers;
            this.BankManager = new BankruptcyManager(players, tiles, panels, playerMarkers, mainForm);
        }
        public void UpdateCurrentPlayerIndex(int newIndex)
        {
            this.currentPlayerIndex = newIndex;
        }
        public void ShowTileActionForm(Tile tile, Player currentPlayer)
        {
            if (tile.PlayerId != null && tile.PlayerId != currentPlayer.ID)
            {
                int rent = mainForm.CalculateRent(tile, currentPlayer.ID);
                if (currentPlayer.DoubleMoney >= 1)
                {
                    int money = rent * 2;
                    currentPlayer.Money -= (money);
                    int owner = tile.PlayerId.Value;
                    mainForm.AddMoney(money, players[owner - 1]);
                    currentPlayer.DoubleMoney--;
                    mainForm.UpdatePlayerPanel(currentPlayer);
                    mainForm.UpdatePlayerPanel(players[owner - 1]);
                    MessageBox.Show($"Bạn phải trả ${rent * 2} tiền thuê cho {players[owner - 1].Name}!", "Trả tiền thuê");
                    return;
                }
                else if (currentPlayer.ReduceHalfMoney >= 1)
                {
                    int money = rent / 2;
                    currentPlayer.Money -= money;
                    int owner = tile.PlayerId.Value;
                    mainForm.AddMoney(money, players[owner - 1]);
                    currentPlayer.ReduceHalfMoney--;
                    mainForm.UpdatePlayerPanel(currentPlayer);
                    mainForm.UpdatePlayerPanel(players[owner - 1]);
                    MessageBox.Show($"Bạn phải trả ${rent / 2} tiền thuê cho {players[owner - 1].Name}!", "Trả tiền thuê");
                    return;
                }else{
                    currentPlayer.Money -= rent;
                    int owner = tile.PlayerId.Value;
                    mainForm.AddMoney(rent, players[owner - 1]);
                    mainForm.UpdatePlayerPanel(currentPlayer);
                    mainForm.UpdatePlayerPanel(players[owner - 1]);
                    MessageBox.Show($"Bạn phải trả ${rent} tiền thuê cho {players[owner - 1].Name}!", "Trả tiền thuê");
                    return;
                }
            }
            switch (tile.Monopoly)
            {
                case "0":
                    HandleSpecialTile(tile, currentPlayer);
                    break;
                case "9":
                    HandleBusStationTile(tile, currentPlayer);
                    break;
                case "10":
                    HandleCompanyTile(tile, currentPlayer);
                    break;
                default:
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
                case "Thuế đặc biệt":
                    HandleSpecialTax(currentPlayer);
                    break;
                case "Ô bắt đầu":
                    HandleStart(currentPlayer);
                    break;
                case "Đi thẳng vào tù":
                    HandleGoToJail(currentPlayer);
                    break;
            }
            mainForm.UpdatePlayerPanel(currentPlayer);
        }
        private void DrawChanceCard(Player player)
        {
            var path = "Co_hoi.txt";
            var cards = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            if (cards.Length > 0){
                string card = cards[random.Next(cards.Length)];
                ProcessCardEffect(player, card, "Khí vận");
            }
        }
        private void DrawCommunityChestCard(Player player)
        {
            var path = "Co_hoi.txt";
            var cards = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            if (cards.Length > 0){
                string card = cards[random.Next(cards.Length)];
                ProcessCardEffect(player, card, "Cơ hội");
            }
        }
        private async void ProcessCardEffect(Player player, string card, string deckType)
        {
            string message = $"{deckType}: {card}";
            MessageBox.Show(message, "Thẻ Bài");
            bool movePlayer = false;
            int moveToIndex = -1;
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
                    movePlayer = true;
                    break;
                case "Bán 1 căn nhà":
                    var sellPropertyForm = new FormSellProperty(players[currentPlayerIndex], tiles, mainForm);
                    if (sellPropertyForm.CanOpen)
                        sellPropertyForm.ShowDialog();
                    break;
                case "Phá nhà":
                    var destroyHouseForm = new FormDestroyHouse(players[currentPlayerIndex], tiles, mainForm);
                    if (destroyHouseForm.CanOpen)
                        destroyHouseForm.ShowDialog();
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
            }
            if (movePlayer && moveToIndex >= 0)
            {
                int currentIndex = player.TileIndex;
                int totalTiles = tiles.Count;
                int steps = (moveToIndex - currentIndex + totalTiles) % totalTiles;
                bool passStart = (currentIndex + steps) > totalTiles;
                await mainForm.MovePlayerStepByStep(player, steps, totalTiles);
                if (passStart)
                {
                    mainForm.HandleStart(player);
                }
            }
        }
        public void HandleStart(Player player)
        {
            mainForm.AddMoney(200,player);
            MessageBox.Show("Bạn đi qua ô bắt đầu, nhận $200!", "Nhận tiền");
            mainForm.UpdatePlayerPanel(player);
        }
        private void HandleIncomeTax(Player player)
        {
            int money = mainForm.CalculatePlayerAssets(player);
            int tax = Math.Min(200, (money + player.Money) / 10);
            mainForm.SubtractMoney(tax, player);
            MessageBox.Show($"Thuế thu nhập: Bạn phải trả ${tax}", "Thuế");
        }
        private void HandleSpecialTax(Player player)
        {
            int money = mainForm.CalculatePlayerAssets(player);
            int tax = (int)((money + player.Money) * 15 / 100);
            mainForm.SubtractMoney(tax, player);
            MessageBox.Show($"Thuế đặc biệt: trả 15% tổng tài sản", "Thuế");
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
            mainForm.UpdatePlayerMarkerPosition(player, jailIndex);
            player.IsInJail = true;
            player.JailTurnCount = 0;
        }
        private void HandleBusStationTile(Tile tile, Player currentPlayer)
        {
            if (tile.PlayerId == null){
                if (currentPlayer.Money < tile.LandPrice){
                    MessageBox.Show("Không đủ tiền mua bến xe!", "Thông báo");
                    return;
                }using (var buyBusForm = new BuyBus(currentPlayer.ID, tile, monopoly, mainForm)){
                    if (buyBusForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= tile.LandPrice;
                        mainForm.UpdatePlayerPanel(currentPlayer);
                        mainForm.UpdateBusStationRent(currentPlayer.ID);
                        mainForm.UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                    }
                }
            }
        }
        private void HandleCompanyTile(Tile tile, Player currentPlayer)
        {
            if (tile.PlayerId == null)
            {
                if (currentPlayer.Money < tile.LandPrice){
                    MessageBox.Show("Không đủ tiền!", "Thông báo");
                    return;
                }using (var buyCompanyForm = new BuyCompany(currentPlayer.ID, tile, monopoly, mainForm)){
                    if (buyCompanyForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= tile.LandPrice;
                        mainForm.UpdatePlayerPanel(currentPlayer);
                        mainForm.UpdateCompanyRent(currentPlayer.ID);
                        mainForm.UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
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
                }using (var landForm = new BuyHome_Land(currentPlayer, tile, monopoly, mainForm)){
                    if (landForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= tile.LandPrice;
                        mainForm.UpdatePlayerPanel(currentPlayer);
                        mainForm.UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                    }
                }
            }else if (tile.PlayerId == currentPlayer.ID){
                int upgradeCost = tile.Level == 4 ? tile.HotelPrice : tile.HousePrice;
                if (currentPlayer.Money < upgradeCost){
                    MessageBox.Show("Không đủ tiền nâng cấp!", "Thông báo");
                    return;
                }using (var upgradeForm = new BuyHome_Land(currentPlayer, tile, monopoly, mainForm)){
                    if (upgradeForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= upgradeCost;
                        mainForm.UpdatePlayerPanel(currentPlayer);
                         mainForm.UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                    }
                }
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
    }
}