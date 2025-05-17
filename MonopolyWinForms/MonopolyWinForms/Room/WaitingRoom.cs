using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MonopolyWinForms.Room
{
    public class WaitingRoomForm : Form
    {
        private Label lblRoomName;
        private Label lblHostName;
        private Label lblPlayers;
        private ListBox lstPlayers;
        private Button btnStartGame;
        private Button btnLeaveRoom;

        private string roomName;
        private string hostName;
        private int maxPlayers;
        private string currentUserName;

        public WaitingRoomForm(string roomName, string hostName, int maxPlayers, List<string> currentPlayers)
        {
            this.roomName = roomName;
            this.hostName = hostName;
            this.maxPlayers = maxPlayers;
            this.currentUserName = Environment.UserName;

            InitializeUI();
            UpdatePlayersList(currentPlayers);
        }

        private void InitializeUI()
        {
            this.Text = "Phòng chờ";
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int margin = 20;
            int labelHeight = 25;
            int spacing = 10;

            lblRoomName = new Label()
            {
                Text = $"Phòng: {roomName}",
                Location = new Point(margin, margin),
                Size = new Size(350, labelHeight),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            this.Controls.Add(lblRoomName);

            lblHostName = new Label()
            {
                Text = $"Chủ phòng: {hostName}",
                Location = new Point(margin, lblRoomName.Bottom + spacing),
                Size = new Size(350, labelHeight),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblHostName);

            lblPlayers = new Label()
            {
                Text = $"Danh sách người chơi (0/{maxPlayers}):",
                Location = new Point(margin, lblHostName.Bottom + spacing * 2),
                Size = new Size(350, labelHeight),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblPlayers);

            lstPlayers = new ListBox()
            {
                Location = new Point(margin, lblPlayers.Bottom + spacing),
                Size = new Size(350, 200),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lstPlayers);

            btnStartGame = new Button()
            {
                Text = "Bắt đầu chơi",
                Location = new Point(margin, lstPlayers.Bottom + spacing * 2),
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 12)
            };
            btnStartGame.Click += BtnStartGame_Click;
            this.Controls.Add(btnStartGame);

            btnLeaveRoom = new Button()
            {
                Text = "Rời phòng",
                Location = new Point(margin + 180, lstPlayers.Bottom + spacing * 2),
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 12)
            };
            btnLeaveRoom.Click += BtnLeaveRoom_Click;
            this.Controls.Add(btnLeaveRoom);

            // Nếu không phải host thì disable nút Start
            if (currentUserName != hostName)
                btnStartGame.Enabled = false;
        }

        public void UpdatePlayersList(List<string> currentPlayers)
        {
            lstPlayers.Items.Clear();
            foreach (var player in currentPlayers)
            {
                lstPlayers.Items.Add(player);
            }
            lblPlayers.Text = $"Danh sách người chơi ({currentPlayers.Count}/{maxPlayers}):";
        }

        private void BtnStartGame_Click(object sender, EventArgs e)
        {
            // TODO: Thêm logic bắt đầu game (gửi lệnh, chuyển form...)
            MessageBox.Show("Bắt đầu chơi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLeaveRoom_Click(object sender, EventArgs e)
        {
            // TODO: Thêm logic rời phòng (gửi lệnh, cập nhật...)
            this.Close();
        }
    }
}
