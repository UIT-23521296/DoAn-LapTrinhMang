using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonopolyWinForms.Home;
using MonopolyWinForms.Login_Signup;
using System.Configuration; // Thêm namespace này
using Newtonsoft.Json;
using System.Configuration;
using MonopolyWinForms.Services;

namespace MonopolyWinForms.Login_Signup
{
    public partial class Login : Form
    {
        private readonly string apiKey = ConfigurationManager.AppSettings["FirebaseApiKey"];
        private Form signupForm;
        public Login(Form signupForm)
        {
            InitializeComponent();
            tb_password.UseSystemPasswordChar = true;
            //btn_login.BackColor = ColorTranslator.FromHtml("#FED626");  // Đổi màu nền của nút login
            //btn_signup.BackColor = ColorTranslator.FromHtml("#33B68F");  // Đổi màu nền của nút login
            //tb_login.BackColor = ColorTranslator.FromHtml("#ACACAC");
            //tb_password.BackColor = ColorTranslator.FromHtml("#ACACAC");
        }

        private void tb_password_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_success()
        {
            Main_home mainPage = new Main_home();

            // Đóng tất cả form trừ form chính
            foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
            {
                if (form != mainPage)
                {
                    form.Hide();
                }
            }

            // Mở form chính
            this.Close();
            mainPage.Show();

            // Khi form chính đóng, thoát app
            mainPage.FormClosed += (s, args) =>
            {
                Session.EndSession();
                Application.Exit();
            };
        }

        private async void btn_login_Click(object sender, EventArgs e)
        {
            string email = tb_login.Text.Trim();
            string password = tb_password.Text.Trim();

            var loginData = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}",
                    content);
                var result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic data = JsonConvert.DeserializeObject(result);
                    string idToken = data.idToken;
                    string localId = data.localId;


                    // Truy vấn thông tin người dùng từ Firebase Realtime Database
                    var userInfoUrl = $"https://doanmang-8f5af-default-rtdb.asia-southeast1.firebasedatabase.app/users/{localId}.json?auth={idToken}";
                    var userInfoResponse = await client.GetAsync(userInfoUrl);
                    var userInfoResult = await userInfoResponse.Content.ReadAsStringAsync();

                    if (userInfoResponse.IsSuccessStatusCode)
                    {
                        dynamic userInfo = JsonConvert.DeserializeObject(userInfoResult);
                        string username = userInfo.username; // Lấy username từ kết quả

                        // Bắt đầu phiên làm việc với username
                        Session.StartSession(localId, username);

                        MessageBox.Show("Đăng nhập thành công!");
                        Login_success();
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thành công, nhưng không lấy được tên hiển thị.");

                    }
                }
                else
                {
                    dynamic errorData = JsonConvert.DeserializeObject(result);
                    string errorMessage = errorData.error.message;

                    if (errorMessage == "EMAIL_NOT_FOUND")
                        MessageBox.Show("Email không tồn tại.");
                    else if (errorMessage == "INVALID_PASSWORD")
                        MessageBox.Show("Mật khẩu sai.");
                    else
                        MessageBox.Show("Lỗi: " + errorMessage);
                }
            }
        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            this.Hide();                 // Ẩn form Login hiện tại
            Signup signupForm = new Signup(this);
            signupForm.Show();          // Mở form Signup      
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Forgot().Show();
        }
    }
}
