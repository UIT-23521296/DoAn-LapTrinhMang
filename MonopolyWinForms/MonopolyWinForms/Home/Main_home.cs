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
            this.Hide();                       // Ẩn Main_home
            var joinRoom = new JoinRoom();
            joinRoom.FormClosed += (s, args) => this.Show();   
            joinRoom.Show();                 
            // Khi JoinRoom bị đóng, hiện lại form này
            //joinRoomForm.FormClosed += (s, args) =>
            //{
            //    if (!this.IsDisposed && this.IsHandleCreated)
            //    {
            //        this.Show();
            //    }
            //};

            //joinRoomForm.Show(); // Hiện JoinRoom
        }

        private void btn_rule_Click(object sender, EventArgs e)
        {
            using (var ruleForm = new Rule())
            {
                ruleForm.ShowDialog();
            }
        }
    }
}
