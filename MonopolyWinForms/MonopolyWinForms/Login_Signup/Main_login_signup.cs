using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonopolyWinForms.Login_Signup
{
    public partial class Main_login_signup : Form
    {
        public Main_login_signup()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#FBF8F4");
            btn_login.BackColor = ColorTranslator.FromHtml("#FED626");  // Đổi màu nền của nút login
            btn_signup.BackColor = ColorTranslator.FromHtml("#33B68F");  // Đổi màu nền của nút login
            btn_quit.BackColor = ColorTranslator.FromHtml("#DC2025");
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            Login formLogin = new Login(this);

            formLogin.Show();
        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            Signup formSingup = new Signup(this);

            formSingup.Show();
        }

        private void btn_quit_Click(object sender, EventArgs e)
        {
           
        }
    }
}
