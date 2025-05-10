using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonopolyWinForms.Login_Signup;

namespace MonopolyWinForms.Login_Signup
{
    public partial class Login : Form
    {
        private Form signupForm;
        public Login(Form signupForm)
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#D9D9D9");
            btn_login.BackColor = ColorTranslator.FromHtml("#FED626");  // Đổi màu nền của nút login
            btn_signup.BackColor = ColorTranslator.FromHtml("#33B68F");  // Đổi màu nền của nút login
            tb_login.BackColor = ColorTranslator.FromHtml("#ACACAC");
            tb_password.BackColor = ColorTranslator.FromHtml("#ACACAC");
            label3.BackColor = ColorTranslator.FromHtml("#ACACAC");
            label4.BackColor = ColorTranslator.FromHtml("#ACACAC");
        }

        private void tb_password_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_login_Click(object sender, EventArgs e)
        {

        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            this.Hide();                 // Ẩn form Login hiện tại
            Signup signupForm = new Signup(this);
            signupForm.Show();          // Mở form Signup      
        }
    }
}
