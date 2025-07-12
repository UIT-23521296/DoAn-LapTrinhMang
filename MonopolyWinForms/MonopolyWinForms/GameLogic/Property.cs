using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    public class Property
    {
        private Player player;
        private MainForm mainForm;
        public Property(Player player, MainForm mainForm)
        {
            this.player = player;
            this.mainForm = mainForm;
        }
        public void AddMoney(int amount)
        {
            player.Money += amount;
            
        }
        public void SubtractMoney(int amount)
        {
            player.Money -= amount;
            mainForm.UpdatePlayerPanel(player);
            if (player.Money < 0)
            {
                mainForm.CheckPlayerBankruptcy(player);
            }
        }
    }
}
