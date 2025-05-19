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
            if (player.Money < amount)
            {
                mainForm.CheckPlayerBankruptcy(player);
            }
            else player.Money -= amount;
        }
    }
}
