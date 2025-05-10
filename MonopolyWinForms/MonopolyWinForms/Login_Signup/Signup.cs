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
    public partial class Signup : Form
    {
        private Form loginForm;
        public Signup(Form loginForm)
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#D9D9D9");
            btn_login.BackColor = ColorTranslator.FromHtml("#FED626");  // Đổi màu nền của nút login
            btn_signup.BackColor = ColorTranslator.FromHtml("#33B68F");  // Đổi màu nền của nút login
            tb_login.BackColor = ColorTranslator.FromHtml("#ACACAC");
            tb_password.BackColor = ColorTranslator.FromHtml("#ACACAC");
            tb_password2.BackColor = ColorTranslator.FromHtml("#ACACAC");
            tb_email.BackColor = ColorTranslator.FromHtml("#ACACAC");
            label3.BackColor = ColorTranslator.FromHtml("#ACACAC");
            label4.BackColor = ColorTranslator.FromHtml("#ACACAC");
            label2.BackColor = ColorTranslator.FromHtml("#ACACAC");
            label5.BackColor = ColorTranslator.FromHtml("#ACACAC");
        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login(this);  // Tạo đối tượng Login mới
            loginForm.Show();               // Mở form Login
        }
    }
}
