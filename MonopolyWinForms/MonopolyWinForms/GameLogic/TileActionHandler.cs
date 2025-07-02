using buyLand_Home;
using MonopolyWinForms.BuyLand_Home;
using MonopolyWinForms.FormManage;
using MonopolyWinForms.Login_Signup;
using MonopolyWinForms.Services;
using MonopolyWinForms.Room;
using MonopolyWinForms;
using System.Security.Policy;
using static MonopolyWinForms.MainForm;
using System.Threading.Tasks;

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
        private readonly MainForm mainForm;
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
            this.mainForm = mainForm; 
            this.BankManager = new BankruptcyManager(players, tiles, panels, playerMarkers, mainForm, currentPlayerIndex);
        }
        public void UpdateCurrentPlayerIndex(int newIndex)
        {
            this.currentPlayerIndex = newIndex;
        }
        public async Task ShowTileActionForm(Tile tile, Player currentPlayer)
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
                    mainForm.CheckPlayerBankruptcy(currentPlayer);
                    MessageBox.Show($"Bạn phải trả ${rent * 2} tiền thuê cho {players[owner - 1].Name}!", "Trả tiền thuê");
                    var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                    await GameManager.UpdateGameState(gameState);
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
                    mainForm.CheckPlayerBankruptcy(currentPlayer);
                    MessageBox.Show($"Bạn phải trả ${rent / 2} tiền thuê cho {players[owner - 1].Name}!", "Trả tiền thuê");
                    var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                    await GameManager.UpdateGameState(gameState);
                    return;
                }else{
                    currentPlayer.Money -= rent;
                    int owner = tile.PlayerId.Value;
                    mainForm.AddMoney(rent, players[owner - 1]);
                    mainForm.UpdatePlayerPanel(currentPlayer);
                    mainForm.UpdatePlayerPanel(players[owner - 1]);
                    mainForm.CheckPlayerBankruptcy(currentPlayer);
                    MessageBox.Show($"Bạn phải trả ${rent} tiền thuê cho {players[owner - 1].Name}!", "Trả tiền thuê");
                    var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                    await GameManager.UpdateGameState(gameState);
                    return;
                }
            }
            switch (tile.Monopoly)
            {
                case "0":
                    await HandleSpecialTile(tile, currentPlayer);
                    break;
                case "9":
                    await HandleBusStationTile(tile, currentPlayer);
                    break;
                case "10":
                    await HandleCompanyTile(tile, currentPlayer);
                    break;
                default:
                    await HandlePropertyTile(tile, currentPlayer);
                    break;
            }
        }
        private async Task HandleSpecialTile(Tile tile, Player currentPlayer)
        {
            switch (tile.Name)
            {
                case "Khí vận":
                    // 1. Gửi trạng thái mới lên server (P1 đã đến ô khí vận)
                    {
                        var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                        await GameManager.UpdateGameState(gameState);
                        await Task.Delay(350); // Đợi các client đồng bộ xong
                        await DrawChanceCard(currentPlayer);
                    }
                    break;
                case "Cơ hội":
                    // 1. Gửi trạng thái mới lên server (P1 đã đến ô cơ hội)
                    {
                        var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                        await GameManager.UpdateGameState(gameState);
                        await Task.Delay(350); // Đợi các client đồng bộ xong
                        await DrawCommunityChestCard(currentPlayer);
                    }
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
                    await HandleGoToJail(currentPlayer);
                    break;
            }
            mainForm.UpdatePlayerPanel(currentPlayer);
        }
        private async Task DrawChanceCard(Player player)
        {
            var path = "Khi_van.txt";
            var cards = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            if (cards.Length > 0){
                string cardLine = cards[random.Next(cards.Length)];
                var parts = cardLine.Split(';');
                string card = parts[0].Trim();
                card = card.Trim('\uFEFF');
                int addMoney = parts.Length > 1 ? int.Parse(parts[1].Trim()) : 0;
                int subMoney = parts.Length > 2 ? int.Parse(parts[2].Trim()) : 0;
                await ProcessKhiVanCardEffect(player, card, addMoney, subMoney);
            }
        }
        private async Task DrawCommunityChestCard(Player player)
        {
            var path = "Co_hoi.txt";
            var cards = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            if (cards.Length > 0){
                string card = cards[random.Next(cards.Length)];
                //string card = cards[8];
                await ProcessCardEffect(player, card, "Cơ hội");
            }
        }
        private async Task ProcessCardEffect(Player player, string card, string deckType)
        {
            string message = $"{deckType}: {card}";

            // Hiển thị MessageBox cho người chơi hiện tại
            if (Session.PlayerInGameId == player.ID)
            {
                MessageBox.Show(message, "Thẻ Bài");
            }

            // Gửi thông tin thẻ lên Firebase để tất cả người chơi nhận được
            try 
            {
                await GameManager.SendChatMessage(
                    GameManager.CurrentRoomId!,
                    "Hệ thống",
                    $"{player.Name} rút được thẻ {deckType}: {card}"
                );
            }
            catch (Exception ex)
            {
                mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
            }

            bool movePlayer = false;
            int moveToIndex = -1;
            switch (card)
            {
                case "Đi thẳng vào tù":
                    await HandleGoToJail(player);
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} bị vào tù do rút thẻ"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    return;
                case "Tự do ra tù":
                    player.AddOutPrisonCard();
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} nhận được thẻ thoát tù"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    break;
                case "Trả gấp đôi tiền thuê cho ô tiếp theo":
                    player.DoubleMoney++;
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} sẽ trả gấp đôi tiền thuê cho ô tiếp theo"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    break;
                case "Giảm 50% tiền thuê cho ô tiếp theo":
                    player.ReduceHalfMoney++;
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} sẽ giảm 50% tiền thuê cho ô tiếp theo"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    break;
                case "Đi đến ô bắt đầu":
                    moveToIndex = 0;
                    movePlayer = true;
                    player.LastMoveType = MoveType.Teleport;
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} được di chuyển đến ô bắt đầu"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    break;
                case "Bán 1 căn nhà":
                    if (Session.PlayerInGameId == player.ID)
                    {
                        var sellPropertyForm = new FormSellProperty(players[currentPlayerIndex], tiles, mainForm, players, currentPlayerIndex);
                        if (sellPropertyForm.CanOpen)
                            sellPropertyForm.ShowDialog();
                    }
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} phải bán 1 căn nhà"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    break;
                case "Phá nhà":
                    if (Session.PlayerInGameId == player.ID)
                    {
                        var destroyHouseForm = new FormDestroyHouse(players[currentPlayerIndex], tiles, mainForm, players, currentPlayerIndex);
                        if (destroyHouseForm.CanOpen)
                            destroyHouseForm.ShowDialog();
                    }
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} phải phá 1 căn nhà"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    break;
                case "Đến ô bến xe tiếp theo":
                    moveToIndex = GetNextBusStation(player.TileIndex);
                    movePlayer = true;
                    player.LastMoveType = MoveType.Teleport;
                    var targetTile = tiles[moveToIndex];

                    // ➡ Nếu bến xe đã có chủ (khác mình) thì bật cờ trả gấp đôi
                    if (targetTile.PlayerId.HasValue && targetTile.PlayerId.Value != player.ID)
                    {
                        player.DoubleMoney++;   // Cờ này sẽ được TileActionHandler xử lý *×2* đúng như hiện tại
                    }
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} được di chuyển đến bến xe tiếp theo và sẽ trả gấp đôi tiền thuê"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    break;
                case "Đến ô công ty tiếp theo":
                    moveToIndex = GetNextCompany(player.TileIndex);
                    movePlayer = true;
                    player.LastMoveType = MoveType.Teleport;
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} được di chuyển đến công ty tiếp theo"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    break;
            }
            if (movePlayer && moveToIndex >= 0)
            {
                int currentIndex = player.TileIndex;
                int totalTiles = tiles.Count;
                int steps = (moveToIndex - currentIndex + totalTiles) % totalTiles;
                bool passStart = (currentIndex + steps) > totalTiles || (currentIndex > moveToIndex);

                //var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                //await GameManager.UpdateGameState(gameState);
                //mainForm.UpdateGameState(gameState);

                player.TileIndex = moveToIndex;
                mainForm.UpdatePlayerMarkerPosition(player, moveToIndex);

                // 3. Cập nhật game state MỘT LẦN DUY NHẤT sau khi di chuyển xong
                //var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                //await GameManager.UpdateGameState(gameState);
                //mainForm.UpdateGameState(gameState);

                //player.TileIndex = moveToIndex;
                // 4. Xử lý đi qua ô bắt đầu nếu có
                if (passStart && moveToIndex != 0)
                {
                    //// Cập nhật game state sau khi nhận tiền
                    //gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                    //await GameManager.UpdateGameState(gameState);
                    //mainForm.UpdateGameState(gameState);
                    mainForm.HandleStart(players[currentPlayerIndex]);
                }
                //File.AppendAllText("log.txt", $"Tôi là {Session.UserName} " +
                //    $"trước xử lý di chuyển với tên là {players[currentPlayerIndex].Name} đang đến ô {players[currentPlayerIndex].TileIndex}");
                //players[currentPlayerIndex].TileIndex = moveToIndex;
                //File.AppendAllText("log.txt", $"Tôi là {Session.UserName} " +
                //    $"đang xử lý di chuyển với tên là {players[currentPlayerIndex].Name} đang đến ô {players[currentPlayerIndex].TileIndex}");
                //await GameManager.UpdateGameState(gameState);
                //mainForm.UpdatePlayerMarkerPosition(player, moveToIndex);
                //mainForm.UpdateGameState(gameState);
                return; // Kết thúc xử lý thẻ
            }
        }
        private async Task ProcessKhiVanCardEffect(Player player, string card, int addMoney, int subMoney)
        {
            // Hiển thị MessageBox cho người chơi hiện tại
            if (Session.PlayerInGameId == player.ID)
            {
                MessageBox.Show($"Khí vận: {card}", "Thẻ Bài");
            }

            // Gửi thông tin thẻ lên Firebase để tất cả người chơi nhận được
            try 
            {
                await GameManager.SendChatMessage(
                    GameManager.CurrentRoomId!,
                    "Hệ thống",
                    $"{player.Name} rút được thẻ Khí vận: {card}"
                );
            }
            catch (Exception ex)
            {
                mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
            }

            switch (card)
            {
                case "Đi thẳng vào tù":
                    await HandleGoToJail(player);
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} bị vào tù do rút thẻ"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    return;
                case "Tự do ra tù":
                    player.AddOutPrisonCard();
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} nhận được thẻ thoát tù"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    break;
                case "Đi đến ô bắt đầu":
                    player.LastMoveType = MoveType.Teleport;                    
                    mainForm.UpdatePlayerMarkerPosition(player, 0);
                    try 
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{player.Name} được di chuyển đến ô bắt đầu"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin thẻ: {ex.Message}", MainForm.LogType.Error);
                    }
                    var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                    await GameManager.UpdateGameState(gameState);
                    return;
                default:
                    if (addMoney > 0)
                    {
                        mainForm.AddMoney(addMoney, player);
                        if (Session.PlayerInGameId == player.ID)
                            MessageBox.Show($"Bạn nhận được ${addMoney}", "Khí vận");
                    }
                    if (subMoney > 0)
                    {
                        mainForm.SubtractMoney(subMoney, player);
                        mainForm.CheckPlayerBankruptcy(player);
                        if (Session.PlayerInGameId == player.ID)
                            MessageBox.Show($"Bạn phải trả ${subMoney}", "Khí vận");
                    }
                    break;
            }
            mainForm.UpdatePlayerPanel(player);
        }
        public void HandleStart(Player player)
        {
            mainForm.AddMoney(200,player);
            if (Session.PlayerInGameId == player.ID)
            {
                MessageBox.Show("Bạn đi qua ô bắt đầu, nhận $200!", "Nhận tiền");
            }
            mainForm.UpdatePlayerPanel(player);
        }
        private void HandleIncomeTax(Player player)
        {
            int money = mainForm.CalculatePlayerAssets(player);
            int tax = Math.Min(200, (money + player.Money) / 10);
            mainForm.SubtractMoney(tax, player);
            mainForm.CheckPlayerBankruptcy(player);
            MessageBox.Show($"Thuế thu nhập: Bạn phải trả ${tax}", "Thuế");
        }
        private void HandleSpecialTax(Player player)
        {
            int money = mainForm.CalculatePlayerAssets(player);
            int tax = (int)((money + player.Money) * 15 / 100);
            mainForm.SubtractMoney(tax, player);
            mainForm.CheckPlayerBankruptcy(player);
            MessageBox.Show($"Thuế đặc biệt: trả 15% tổng tài sản", "Thuế");
        }
        public async Task HandleGoToJail(Player player)
        {
            player.LastMoveType = MoveType.Teleport;
            if (player.OutPrison > 0){
                player.OutPrison--;
                MessageBox.Show("Sử dụng thẻ thoát tù, bạn không phải vào tù", "Thoát Tù");
                return;
            }
            int jailIndex = tiles.FindIndex(t => t.Name == "Nhà tù");
            player.TileIndex = jailIndex;
            player.ResetDoubleDice();
            mainForm.UpdatePlayerMarkerPosition(player, jailIndex);
            player.IsInJail = true;
            player.JailTurnCount = 0;
        }
        private async Task HandleBusStationTile(Tile tile, Player currentPlayer)
        {
            if (tile.PlayerId == null){
                if (currentPlayer.Money < tile.LandPrice){
                    MessageBox.Show("Không đủ tiền mua bến xe!", "Thông báo");
                    return;
                }using (var buyBusForm = new BuyBus(currentPlayer.ID, tile, monopoly, mainForm, tiles, players, currentPlayerIndex)){
                    if (buyBusForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= tile.LandPrice;
                        mainForm.UpdatePlayerPanel(currentPlayer);
                        mainForm.UpdateBusStationRent(currentPlayer.ID);
                        mainForm.UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                        // Cập nhật giá thuê cho tất cả bến xe và công ty
                        mainForm.UpdateTile.UpdateAllRents();
                        var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                        await GameManager.UpdateGameState(gameState);

                        if (!MainForm.GameEnded && monopoly.CheckWin(currentPlayer.ID))
                        {
                            await mainForm.DeclareWinner(currentPlayer);
                            return;
                        }
                    }
                }
            }
        }
        private async Task HandleCompanyTile(Tile tile, Player currentPlayer)
        {
            if (tile.PlayerId == null)
            {
                if (currentPlayer.Money < tile.LandPrice){
                    MessageBox.Show("Không đủ tiền!", "Thông báo");
                    return;
                }using (var buyCompanyForm = new BuyCompany(currentPlayer.ID, tile, monopoly, mainForm, tiles, players, currentPlayerIndex)){
                    if (buyCompanyForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= tile.LandPrice;
                        mainForm.UpdatePlayerPanel(currentPlayer);
                        mainForm.UpdateCompanyRent(currentPlayer.ID);
                        mainForm.UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                        // Cập nhật giá thuê cho tất cả bến xe và công ty
                        mainForm.UpdateTile.UpdateAllRents();
                        var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                        await GameManager.UpdateGameState(gameState);
                        if (!MainForm.GameEnded && monopoly.CheckWin(currentPlayer.ID))
                        {
                            await mainForm.DeclareWinner(currentPlayer);
                            return;
                        }
                    }
                }
            }
        }
        private async Task HandlePropertyTile(Tile tile, Player currentPlayer)
        {
            if (tile.PlayerId == null){
                if (currentPlayer.Money < tile.LandPrice){
                    MessageBox.Show("Không đủ tiền mua đất!", "Thông báo");
                    return;
                }using (var landForm = new BuyHome_Land(currentPlayer, tile, monopoly, mainForm, players, currentPlayerIndex, tiles)){
                    if (landForm.ShowDialog() == DialogResult.OK){
                        currentPlayer.Money -= tile.LandPrice;
                        mainForm.UpdatePlayerPanel(currentPlayer);
                        mainForm.UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                        mainForm.UpdateTile.UpdateAllRents();
                        var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                        await GameManager.UpdateGameState(gameState);
                        if (!MainForm.GameEnded && monopoly.CheckWin(currentPlayer.ID))
                        {
                            await mainForm.DeclareWinner(currentPlayer);
                            return;
                        }
                    }
                }
            }else if (tile.PlayerId == currentPlayer.ID){
                // Nếu đã là khách sạn (level 5) thì không cho mở form nâng cấp nữa
                if (tile.Level >= 5)
                {
                    return;
                }
                using (var upgradeForm = new BuyHome_Land(currentPlayer, tile, monopoly, mainForm, players, currentPlayerIndex, tiles)){
                    if (upgradeForm.ShowDialog() == DialogResult.OK){
                        int totalPrice = upgradeForm.TotalPrice;
                        if (currentPlayer.Money < totalPrice){
                            MessageBox.Show("Không đủ tiền nâng cấp!", "Thông báo");
                            return;
                        }
                        currentPlayer.Money -= totalPrice;
                        mainForm.UpdatePlayerPanel(currentPlayer);
                        mainForm.UpdateTileDisplay(Array.IndexOf(panels, panels.First(p => p.Tag == tile)), currentPlayer);
                        mainForm.UpdateTile.UpdateAllRents();
                        var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                        await GameManager.UpdateGameState(gameState);
                        if (!MainForm.GameEnded && monopoly.CheckWin(currentPlayer.ID))
                        {
                            await mainForm.DeclareWinner(currentPlayer);
                            return;
                        }
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