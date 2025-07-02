using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonopolyWinForms.Services;
using MonopolyWinForms.Room;
using MonopolyWinForms.Login_Signup;

namespace MonopolyWinForms.GameLogic
{
    public class DiceRollHandler
    {
        private List<Player> players;
        private List<Tile> tiles;
        private Panel[] panels;
        private Random random;
        private int currentPlayerIndex;
        private MainForm mainForm;
        public DiceRollHandler(List<Player> players, Panel[] panels, MainForm mainForm, int currentPlayerIndex, List<Tile> tiles)
        {
            this.players = players;
            this.panels = panels;
            this.random = new Random();
            this.mainForm = mainForm;
            this.currentPlayerIndex = currentPlayerIndex;
            this.tiles = tiles;
        }
        public async Task RollDiceAndMoveAsync()
        {
            var player = players[currentPlayerIndex];
            int dice1 = random.Next(1, 7);
            int dice2 = random.Next(1, 7);
            //int dice1 = 0;
            //int dice2 = 2;
            int totalSteps = dice1 + dice2;
            bool isDouble = dice1 == dice2;
            if (Session.PlayerInGameId == player.ID)
            {
                MessageBox.Show($"Bạn tung được: {dice1} và {dice2} (Tổng: {totalSteps})", "Kết quả xúc xắc");
            }
            
            //Gửi log cho tất cả người chơi
            try
            {
                await GameManager.SendChatMessage(
                    GameManager.CurrentRoomId!,
                    "Hệ thống",
                    $"{players[currentPlayerIndex].Name} lắc được {dice1} và {dice2} (tổng: {totalSteps})" +
                    (isDouble ? " - Được lắc tiếp!" : "")
                );
            }
            catch (Exception ex)
            {
                mainForm.AddToGameLog($"Lỗi gửi thông tin xúc xắc: {ex.Message}", MainForm.LogType.Error);
            }
            
            
            if (player.IsInJail)
            {
                if (isDouble)
                {
                    player.IsInJail = false;
                    player.JailTurnCount = 0;
                }
                else
                {
                    player.JailTurnCount++;
                    if (player.JailTurnCount >= 3)
                    {
                        MessageBox.Show("Bạn đã ở tù 3 lượt. Trả $100 để ra tù và tiếp tục đi.", "Thoát tù sau 3 lượt");
                        if (player.Money < 100)
                        {
                            mainForm.ForceSellAssets(player);   
                            mainForm.SubtractMoney(100, player);
                        }
                        else
                        {
                            mainForm.SubtractMoney(100, player);
                        }
                        player.IsInJail = false;
                        player.JailTurnCount = 0;
                    }
                    else
                    {
                        await mainForm.NextTurn();
                        return;
                    }
                }
            }
            int totalTiles = panels.Length;
            bool passStart = (player.TileIndex + totalSteps) > totalTiles;

            player.LastMoveType = MoveType.Step;
            await mainForm.MovePlayerStepByStep(player, totalSteps, totalTiles);

            // Nếu sau khi di chuyển, player bị vào tù (do tile hoặc card), thì kết thúc lượt luôn, không xử lý lắc đôi
            if (player.IsInJail)
            {
                player.ResetDoubleDice();
                var jailState = new GameState(GameManager.CurrentRoomId,
                                  currentPlayerIndex, players, tiles);
                await GameManager.UpdateGameState(jailState);
                await mainForm.NextTurn();
                return;
            }

            if (isDouble)
            {
                if (player.DoubleDices == 2)
                {
                    player.ResetDoubleDice();
                    await mainForm.NextTurn();

                    if (Session.PlayerInGameId == player.ID)
                    {
                        MessageBox.Show("Bạn quá hên, nhường lượt cho người khác", "Lắc đôi 2 lần liên tiếp");
                    }

                    try
                    {
                        await GameManager.SendChatMessage(
                            GameManager.CurrentRoomId!,
                            "Hệ thống",
                            $"{players[currentPlayerIndex].Name} lắc được đôi 2 lần liên tiếp. Đổi lượt"
                        );
                    }
                    catch (Exception ex)
                    {
                        mainForm.AddToGameLog($"Lỗi gửi thông tin xúc xắc: {ex.Message}", MainForm.LogType.Error);
                    }
                    return;
                }
                else
                {
                    player.DoubleDices++;
                }

                var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                await GameManager.UpdateGameState(gameState);
            }
            else
            {
                player.ResetDoubleDice();
                await mainForm.NextTurn();
            }
        }
    }
}
