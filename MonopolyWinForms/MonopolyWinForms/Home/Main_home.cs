using MonopolyWinForms.Room;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonopolyWinForms.Home
{
    public partial class Main_home : Form
    {
        public Main_home()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#FBF8F4");
            btn_play.BackColor = ColorTranslator.FromHtml("#FED626");
            btn_rule.BackColor = ColorTranslator.FromHtml("#33B68F");
            btn_quit.BackColor = ColorTranslator.FromHtml("#DC2025");
        }

        private void btn_quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại

            JoinRoom joinRoomForm = new JoinRoom();

            // Khi JoinRoom bị đóng, hiện lại form này
            joinRoomForm.FormClosed += (s, args) =>
            {
                this.Show();
            };

            joinRoomForm.Show(); // Hiện JoinRoom
        }
    }
}
