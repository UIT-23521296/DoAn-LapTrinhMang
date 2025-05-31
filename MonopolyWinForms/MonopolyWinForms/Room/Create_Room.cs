using MonopolyWinForms.Services;
using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonopolyWinForms.Login_Signup;

namespace MonopolyWinForms.Room
{
    public partial class Create_Room : Form
    {
        private TextBox txtRoomName, txtHostIP;
        private ComboBox cmbPlayTime, cmbMaxPlayers;
        private Button btnCreate;

        private const int FixedPort = 8800; // Port TCP cố định

        public Create_Room()
        {
            InitializeForm();
            InitializeUI();
            this.AcceptButton = btnCreate;

            // Gán user ID khi mở form (ví dụ lấy username Windows)
            SessionManager.CurrentUserId = Environment.UserName;
        }

        private void InitializeForm()
        {
            this.Text = "Tạo phòng chơi";
            this.Size = new Size(480, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void InitializeUI()
        {
            int top = 30, spacing = 80;

            AddLabel("Tên phòng:", top);
            txtRoomName = AddTextBox(top);
            top += spacing;

            AddLabel("IP Chủ phòng:", top);
            txtHostIP = AddTextBox(top);
            txtHostIP.ReadOnly = true;
            txtHostIP.Text = GetLocalIPAddress();
            top += spacing;

            AddLabel("Cổng (port):", top);
            var txtPort = AddTextBox(top);
            txtPort.ReadOnly = true;
            txtPort.Text = FixedPort.ToString();
            top += spacing;

            AddLabel("Số người tối đa:", top);
            cmbMaxPlayers = new ComboBox
            {
                Location = new Point(200, top),
                Size = new Size(180, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 12)
            };
            cmbMaxPlayers.Items.AddRange(new object[] { "2", "3", "4" });
            cmbMaxPlayers.SelectedIndex = 0;
            this.Controls.Add(cmbMaxPlayers);
            top += spacing;

            AddLabel("Thời gian chơi (phút):", top);
            cmbPlayTime = new ComboBox
            {
                Location = new Point(200, top),
                Size = new Size(180, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 12)
            };
            cmbPlayTime.Items.AddRange(new object[] { "30", "45", "60" });
            cmbPlayTime.SelectedIndex = 0;
            this.Controls.Add(cmbPlayTime);
            top += spacing;

            btnCreate = new Button
            {
                Text = "Tạo phòng",
                Location = new Point(130, top + 10),
                Size = new Size(150, 45),
                Font = new Font("Segoe UI", 14)
            };
            btnCreate.Click += async (s, e) => await BtnCreate_Click();
            this.Controls.Add(btnCreate);
        }

        private void AddLabel(string text, int top)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(30, top),
                Size = new Size(160, 30),
                Font = new Font("Segoe UI", 12)
            };
            this.Controls.Add(label);
        }

        private TextBox AddTextBox(int top)
        {
            var textBox = new TextBox
            {
                Location = new Point(200, top),
                Size = new Size(180, 30),
                Font = new Font("Segoe UI", 12)
            };
            this.Controls.Add(textBox);
            return textBox;
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "127.0.0.1";
        }

        private async Task BtnCreate_Click()
        {
            if (!Session.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập trước khi tạo phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string roomName = txtRoomName.Text.Trim();
            
            // Validate room name
            if (string.IsNullOrEmpty(roomName))
            {
                MessageBox.Show("Vui lòng nhập tên phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string roomId = Guid.NewGuid().ToString();
                var roomInfo = new RoomInfo
                {
                    RoomId = roomId,
                    RoomName = roomName,
                    HostId = Session.UserId,  // Sử dụng UserId từ Session
                    HostIP = GetLocalIPAddress(),
                    Port = FixedPort,
                    MaxPlayers = int.Parse(cmbMaxPlayers.SelectedItem.ToString()),
                    PlayTime = int.Parse(cmbPlayTime.SelectedItem.ToString()),
                    ReadyPlayers = new List<string> {Session.UserName},
                    PlayerDisplayNames = new List<string> { Session.UserName },  // Sử dụng UserName từ Session
                    IsStarted = false,
                    CreatedAt = DateTime.UtcNow
                };

                var firebase = new FirebaseService();
                await firebase.CreateRoomAsync(roomId, roomInfo);

                // Cập nhật session
                Session.JoinRoom(roomId, true);

                var lobby = new Waiting_Room_Host();
                lobby.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
