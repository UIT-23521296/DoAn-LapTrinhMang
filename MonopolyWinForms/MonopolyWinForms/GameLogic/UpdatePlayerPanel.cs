using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    public class UpdatePlayerPanel
    {
        public UpdatePlayerPanel() { }
        public void UpdatePlayerPanelUI(Panel playerPanel, Player player)
        {
            playerPanel.Controls.Clear();

            PictureBox pic = new PictureBox
            {
                Size = new Size(50, 50),
                Location = new Point(5, 5),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = player.GetAvatar()
            };
            playerPanel.Controls.Add(pic);

            Label nameLabel = new Label
            {
                Text = player.Name,
                Location = new Point(60, 5),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Black
            };
            playerPanel.Controls.Add(nameLabel);

            Label moneyLabel = new Label
            {
                Text = $"Tiền: ${player.Money}",
                Location = new Point(60, 30),
                AutoSize = true,
                Font = new Font("Arial", 9),
                ForeColor = Color.Green
            };
            playerPanel.Controls.Add(moneyLabel);
        }
    }
}
