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
            btn_setting.BackColor = ColorTranslator.FromHtml("#33B68F");
            btn_quit.BackColor = ColorTranslator.FromHtml("#DC2025");
        }

        private void btn_quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
