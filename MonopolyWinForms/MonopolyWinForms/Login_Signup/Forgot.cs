using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Configuration; 


namespace MonopolyWinForms.Login_Signup
{
    public partial class Forgot : Form
    {

        private readonly string apiKey = ConfigurationManager.AppSettings["FirebaseApiKey"];
        public Forgot()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#D9D9D9");
            btn_send.BackColor = ColorTranslator.FromHtml("#33B68F");
            tb_email.BackColor = ColorTranslator.FromHtml("#ACACAC");
        }

        private async void btn_send_Click(object sender, EventArgs e)
        {
            string email = tb_email.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email.");
                return;
            }

            var resetData = new
            {
                requestType = "PASSWORD_RESET",
                email = email
            };

            var json = JsonConvert.SerializeObject(resetData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                     $"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={apiKey}",
                     content);

                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đã gửi email đặt lại mật khẩu. Vui lòng kiểm tra hộp thư.");
                    this.Close();
                    new Login(this).Show();
                }
                else
                {
                    MessageBox.Show("Gửi email thất bại: " + result);
                }
            }
        }
    }
}
