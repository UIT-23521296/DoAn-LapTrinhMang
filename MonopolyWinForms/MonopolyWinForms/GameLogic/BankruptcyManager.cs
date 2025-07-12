using MonopolyWinForms.FormManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonopolyWinForms.Services;

namespace MonopolyWinForms.GameLogic
{
    public class BankruptcyManager
    {
        private List<Player> players;
        private List<Tile> tiles;
        private int currentPlayerIndex;
        private Panel[] panels;
        private Dictionary<int, Panel> playerMarkers;
        private MainForm mainForm;
        public BankruptcyManager(List<Player> players, List<Tile> tiles, Panel[] panels,
                                 Dictionary<int, Panel> playerMarkers, MainForm form, int currentPlayerIndex)
        {
            this.players = players;
            this.tiles = tiles;
            this.panels = panels;
            this.playerMarkers = playerMarkers;
            this.mainForm = form;
            this.currentPlayerIndex = currentPlayerIndex;
        }

        public void CheckPlayerBankruptcy(Player player)
        {
            if (player.Money < 0 && !player.IsBankrupt)
            {
                int totalAssets = CalculatePlayerAssets(player);

                if (player.Money + totalAssets < 0)
                    ProcessBankruptcy(player);
                else
                    ForceSellAssets(player);
            }
        }
        public int CalculatePlayerAssets(Player player)
        {
            int total = 0;
            foreach (var tile in tiles.Where(t => t.PlayerId == player.ID))
            {
                total += tile.LandPrice;
                if (tile.Level > 0)
                {
                    total += tile.HousePrice * (tile.Level < 5 ? tile.Level : 3);
                    if (tile.Level == 5) total += tile.HotelPrice;
                }
            }
            return total;
        }
        private void ProcessBankruptcy(Player player)
        {
            player.DeclareBankruptcy();

            foreach (var tile in tiles.Where(t => t.PlayerId == player.ID))
            {
                tile.PlayerId = null;
                tile.Level = 0;
                mainForm.UpdateTileDisplay(tiles.IndexOf(tile), player);
            }

            if (playerMarkers.ContainsKey(player.ID))
            {
                var marker = playerMarkers[player.ID];
                panels[player.TileIndex].Controls.Remove(marker);
                playerMarkers.Remove(player.ID);
            }

            mainForm.UpdatePlayerPanel(player);
            MessageBox.Show($"Người chơi {player.ID} đã phá sản và rời khỏi game!", "Phá sản");

            players.Remove(player);
            CheckGameEnd();
        }
        private void CheckGameEnd()
        {
            var activePlayers = players.Where(p => !p.IsBankrupt).ToList();

            if (activePlayers.Count == 1)
            {
                GameResultForm resultForm = new GameResultForm(players, tiles);
                resultForm.ShowDialog();

                mainForm.DisableRollButton();
            }
        }
        public void ForceSellAssets(Player player)
        {
            var ownedTiles = tiles.Where(t => t.PlayerId == player.ID && t.Level > 0)
                                  .OrderByDescending(t => t.Level)
                                  .ThenByDescending(t => t.LandPrice)
                                  .ToList();

            if (!ownedTiles.Any())
            {
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

                btnSell.Click += async (s, e) =>
                {
                    if (listBox.SelectedItem == null) return;

                    dynamic selected = listBox.SelectedItem;
                    Tile tile = selected.Tile;
                    //int sellValue = selected.Value;

                    // Gọi hàm async đúng cách và lấy giá trị trả về
                    int refund = await tile.SellLandAndHouses(currentPlayerIndex, players, tiles);
                    player.Money += refund;

                    mainForm.UpdateTileDisplay(tiles.IndexOf(tile), player);
                    mainForm.UpdatePlayerPanel(player);

                    // Cập nhật trạng thái game lên server ngay sau khi bán tài sản
                    var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                    await GameManager.UpdateGameState(gameState);      // ⬅️ await, KHÔNG GetResult

                    if (player.Money >= 0)
                    {
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    else
                    {
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
                            int v = CalculateSellValue(t);
                            listBox.Items.Add(new
                            {
                                Tile = t,
                                DisplayText = $"{t.Name} (Cấp {t.Level}) - Bán được ${v}",
                                Value = v
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
            int value = tile.LandPrice / 2;
            if (tile.Level > 0)
            {
                if (tile.Level == 5)
                    value += (tile.HousePrice * 3 + tile.HotelPrice) / 2;
                else
                    value += (tile.HousePrice * tile.Level) / 2;
            }
            return value;
        }
    }
}
