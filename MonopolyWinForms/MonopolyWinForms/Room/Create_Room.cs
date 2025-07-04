using MonopolyWinForms.Services;
using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonopolyWinForms.Login_Signup;
using MonopolyWinForms.Properties;

namespace MonopolyWinForms.Room
{
    public partial class Create_Room : Form
    {
        private TextBox txtRoomName;
        private ComboBox cmbPlayTime, cmbMaxPlayers;
        private Button btnCreate;


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
            this.Icon = new Icon(@"E:\Dai Hoc\Lap trinh mang\DoAn-LapTrinhMang\MonopolyWinForms\MonopolyWinForms\Assets\Images\icons8-monopoly-100.ico");
            this.Size = new Size(480, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }
        private void InitializeUI()
        {
            this.BackColor = Color.White;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 5,
                ColumnCount = 2,
                Padding = new Padding(40, 60, 40, 40),
                BackColor = Color.White,
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            this.Controls.Add(layout);

            // Tên phòng
            layout.Controls.Add(CreateLabel("Tên phòng:"), 0, 0);
            txtRoomName = CreateTextbox();
            layout.Controls.Add(txtRoomName, 1, 0);

            // Số người tối đa
            layout.Controls.Add(CreateLabel("Số người tối đa:"), 0, 1);
            cmbMaxPlayers = CreateComboBox(new object[] { "2", "3", "4" });
            layout.Controls.Add(cmbMaxPlayers, 1, 1);

            // Thời gian chơi
            layout.Controls.Add(CreateLabel("Thời gian chơi (phút):"), 0, 2);
            cmbPlayTime = CreateComboBox(new object[] { "30", "45", "60" });
            layout.Controls.Add(cmbPlayTime, 1, 2);

            // Nút tạo phòng
            btnCreate = new Button
            {
                Text = "Tạo phòng",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Width = 180,
                Height = 45,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.None
            };
            btnCreate.FlatAppearance.BorderSize = 0;
            btnCreate.Click += async (s, e) => await BtnCreate_Click();

            // Chèn nút vào layout, căn giữa
            layout.SetColumnSpan(btnCreate, 2);
            layout.Controls.Add(btnCreate, 0, 4);
        }

        private Label CreateLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11),
                Anchor = AnchorStyles.Left,
                AutoSize = true
            };
        }

        private TextBox CreateTextbox()
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Height = 30,
                Margin = new Padding(0, 5, 0, 5),
                Dock = DockStyle.Fill
            };
        }

        private ComboBox CreateComboBox(object[] items)
        {
            var combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill
            };
            combo.Items.AddRange(items);
            combo.SelectedIndex = 0;
            return combo;
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
                    HostId = Session.UserId,
                    MaxPlayers = int.Parse(cmbMaxPlayers.SelectedItem.ToString()),
                    PlayTime = int.Parse(cmbPlayTime.SelectedItem.ToString()),
                    ReadyPlayers = new List<string> { Session.UserName },
                    PlayerDisplayNames = new List<string> { Session.UserName },
                    PlayerIds = new List<string> { Session.UserId },
                    IsStarted = false,
                    CreatedAt = DateTime.UtcNow.ToString("o")
                };


                var firebase = new FirebaseService();
                await firebase.CreateRoomAsync(roomId, roomInfo);

                // Cập nhật session
                Session.JoinRoom(roomId, true);

                var lobby = new Waiting_Room_Host();
                this.Tag = "Redirected"; // Thông báo cho JoinRoom biết là đã chuyển hướng
                lobby.Show();
                this.Close(); // Tắt Create_Room
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
