using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    public class DiceRollHandler
    {
        private List<Player> players;
        private Panel[] panels;
        private Random random;
        private int currentPlayerIndex;
        private MainForm mainForm;
        public DiceRollHandler(List<Player> players, Panel[] panels, MainForm mainForm, int currentPlayerIndex)
        {
            this.players = players;
            this.panels = panels;
            this.random = new Random();
            this.mainForm = mainForm;
            this.currentPlayerIndex = currentPlayerIndex;
        }
        public async Task RollDiceAndMoveAsync()
        {
            var player = players[currentPlayerIndex];
            int dice1 = random.Next(1, 7);
            int dice2 = random.Next(1, 7);
            int totalSteps = dice1 + dice2;
            MessageBox.Show($"Bạn tung được: {dice1} và {dice2} (Tổng: {totalSteps})", "Kết quả xúc xắc");
            bool isDouble = dice1 == dice2;
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
                        mainForm.NextTurn();
                        return;
                    }
                }
            }
            int totalTiles = panels.Length;
            bool passStart = (player.TileIndex + totalSteps) > totalTiles;

            await mainForm.MovePlayerStepByStep(player, totalSteps, totalTiles);

            if (passStart)
            {
                mainForm.HandleStart(player);
            }

            if (isDouble)
            {
                if (player.DoubleDices == 2 || player.IsInJail)
                {
                    player.ResetDoubleDice();
                    mainForm.NextTurn();
                    return;
                }
                else
                {
                    player.DoubleDices++;
                }
            }
            else
            {
                player.ResetDoubleDice();
                mainForm.NextTurn();
            }
        }
    }
}
