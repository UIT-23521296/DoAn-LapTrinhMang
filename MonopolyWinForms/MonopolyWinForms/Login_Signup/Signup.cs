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
using Newtonsoft.Json;
using System.Net.Http;
using System.Configuration; 
using System.Data.SqlClient;

namespace MonopolyWinForms.Login_Signup
{
    public partial class Signup : Form
    {
        private Form loginForm;
        private readonly string apiKey = ConfigurationManager.AppSettings["FirebaseApiKey"];
        public Signup(Form loginForm)
        {
            InitializeComponent();
            tb_password.UseSystemPasswordChar = true;
            tb_password2.UseSystemPasswordChar = true;
            this.BackColor = ColorTranslator.FromHtml("#D9D9D9");
            btn_singup.BackColor = ColorTranslator.FromHtml("#FED626");  // Đổi màu nền của nút login
            btn_login.BackColor = ColorTranslator.FromHtml("#33B68F");  // Đổi màu nền của nút login
            tb_login.BackColor = ColorTranslator.FromHtml("#ACACAC");
            tb_password.BackColor = ColorTranslator.FromHtml("#ACACAC");
            tb_password2.BackColor = ColorTranslator.FromHtml("#ACACAC");
            tb_email.BackColor = ColorTranslator.FromHtml("#ACACAC");
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login(this);  // Tạo đối tượng Login mới
            loginForm.Show();               // Mở form Login
        }

        private async void btn_singup_Click(object sender, EventArgs e)
        {
            var email = tb_email.Text;
            var password = tb_password.Text;
            var username = tb_login.Text;

            var signupData = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonConvert.SerializeObject(signupData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}",
                    content);

                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse kết quả để lấy localId
                    dynamic data = JsonConvert.DeserializeObject(result);
                    string localId = data.localId;
                    string tokenId = data.idToken;

                    // Tạo object người dùng để lưu vào DB
                    var userInfo = new
                    {
                        username = username,
                        email = email,
                        status = "waiting"
                    };

                    var userJson = JsonConvert.SerializeObject(userInfo);
                    var userContent = new StringContent(userJson, Encoding.UTF8, "application/json");

                    // Firebase Realtime Database URL
                    var firebaseDbUrl = $"https://doanmang-8f5af-default-rtdb.asia-southeast1.firebasedatabase.app/users/{localId}.json?auth={tokenId}";


                    var dbResponse = await client.PutAsync(firebaseDbUrl, userContent);

                    // Kiểm tra lỗi trả về từ Firebase
                    var dbResponseContent = await dbResponse.Content.ReadAsStringAsync();

                    if (dbResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đăng ký thành công!");
                        this.Hide();
                        new Login(this).Show();
                    }
                    else
                    {
                        // In ra chi tiết lỗi từ Firebase
                        MessageBox.Show($"Lỗi: {dbResponseContent}");
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi: " + result);
                }
            }
        }

    }
}
