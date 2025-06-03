using MonopolyWinForms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MonopolyWinForms.Login_Signup;

namespace MonopolyWinForms.Room
{
    public partial class Waiting_Room_Client: Form
    {
        private string roomId;
        private FirebaseService firebase;
        private System.Windows.Forms.Timer refreshTimer;
        private bool isReady = false;

        public Waiting_Room_Client(string roomId)
        {
            InitializeComponent();
            this.roomId = roomId;
            firebase = new FirebaseService();

            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 3000;
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();

            this.FormClosing += Waiting_Room_Client_FormClosing;

            // Đăng ký event handler
            GameManager.OnPlayerLeft += HandlePlayerLeft;
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            var room = await firebase.GetRoomAsync(roomId);
            if (room == null)
            {
                refreshTimer.Stop();
                this.Close();
                return;
            }

            if (room.IsStarted)
            {
                refreshTimer.Stop();

                //Khởi tạo game cho client
                GameManager.StartGame(Session.CurrentRoomId, room.PlayerDisplayNames, room.PlayTime);
                Form mainFrom = new MainForm();
                mainFrom.Show();
                this.Hide();
                return;
            }

            if (room.PlayerDisplayNames.Count <= 1)
            {
                refreshTimer.Stop();
                this.Close();
                return;
            }

            txb_RoomName.Text = room.RoomName;
            UpdatePlayerNames(room.PlayerDisplayNames);
        }

        private void UpdatePlayerNames(List<string> playerNames)
        {
            TextBox[] playerTextBoxes = { txb_player1, txb_player2, txb_player3, txb_player4 };

            for (int i = 0; i < playerTextBoxes.Length; i++)
            {
                if (i < playerNames.Count)
                    playerTextBoxes[i].Text = playerNames[i];
                else
                    playerTextBoxes[i].Text = "";
            }
        }

        private async void  Waiting_Room_Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dừng timer
            refreshTimer.Stop();

            // Nếu game đã bắt đầu, không cần xử lý thoát phòng
            if (GameManager.IsGameStarted)
            {
                return;
            }

            // Xử lý thoát phòng như cũ
            if (Session.CurrentRoomId != null)
            {
                try
                {
                    var room = await firebase.GetRoomAsync(Session.CurrentRoomId);
                    if (room != null)
                    {
                        if (room.PlayerDisplayNames.Contains(Session.UserName))
                        {
                            room.PlayerDisplayNames.Remove(Session.UserName);
                            
                            if (room.PlayerDisplayNames.Count == 0)
                            {
                                await firebase.DeleteRoomAsync(Session.CurrentRoomId);
                            }
                            else
                            {
                                await firebase.CreateRoomAsync(Session.CurrentRoomId, room);
                            }
                        }
                    }
                    Session.LeaveRoom();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thoát phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            JoinRoom joinRoomForm = new JoinRoom();
            joinRoomForm.Show();
        }

        private void btn_Out_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btn_Ready_Click(object sender, EventArgs e)
        {
            try
            {
                var room = await firebase.GetRoomAsync(roomId);
                if (room != null)
                {
                    isReady = !isReady;
                    btn_Ready.Text = isReady ? "Hủy sẵn sàng" : "Sẵn sàng";
                    btn_Ready.BackColor = isReady ? Color.Green : Color.SkyBlue;

                    // Cập nhật trạng thái sẵn sàng lên Firebase
                    if (isReady)
                    {
                        if (!room.ReadyPlayers.Contains(Session.UserName))
                        {
                            room.ReadyPlayers.Add(Session.UserName);
                        }
                    }
                    else
                    {
                        room.ReadyPlayers.Remove(Session.UserName);
                    }
                    await firebase.CreateRoomAsync(roomId, room);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật trạng thái: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandlePlayerLeft(string playerName)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandlePlayerLeft(playerName)));
                return;
            }

            // Kiểm tra xem form đã đóng chưa
            if (this.IsDisposed || !this.IsHandleCreated)
                return;

            // Hiển thị thông báo
            MessageBox.Show(
                $"{playerName} đã thoát phòng. Phòng sẽ đóng.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            // Dừng timer refresh
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
            }

            // Đóng form và quay về màn hình danh sách phòng
            this.Hide();
            var joinRoomForm = new JoinRoom();
            joinRoomForm.Show();
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            GameManager.OnPlayerLeft -= HandlePlayerLeft;
        }
    }
}
