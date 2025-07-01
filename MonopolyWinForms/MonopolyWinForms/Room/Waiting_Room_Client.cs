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
using MonopolyWinForms.GameLogic;
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

        private async void Waiting_Room_Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Dừng timer refresh trước
                if (refreshTimer != null)
                {
                    refreshTimer.Stop();
                    refreshTimer.Dispose();
                }

                // Nếu đang trong phòng
                if (Session.CurrentRoomId != null)
                {
                    var room = await firebase.GetRoomAsync(Session.CurrentRoomId);
                    if (room != null)
                    {
                        // Xóa người chơi khỏi danh sách
                        room.PlayerDisplayNames.Remove(Session.UserName);
                        room.PlayerIds.Remove(Session.UserId);
                        room.ReadyPlayers.Remove(Session.UserName);

                        // Nếu không còn ai trong phòng, xóa phòng
                        if (room.PlayerDisplayNames.Count == 0)
                        {
                            await firebase.DeleteRoomAsync(Session.CurrentRoomId);
                        }
                        else
                        {
                            // Cập nhật thông tin phòng
                            await firebase.CreateRoomAsync(Session.CurrentRoomId, room);
                        }
                    }

                    // Reset session
                    Session.LeaveRoom();
                }

                // Đóng form và quay về màn hình danh sách phòng
                this.Hide();

                var existing = Application.OpenForms.OfType<JoinRoom>().FirstOrDefault();
                if (existing == null)
                {
                    var joinRoom = new JoinRoom();
                    joinRoom.Show(); // Không dùng BeginInvoke
                }
                else
                {
                    existing.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý thoát phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private bool _playerLeftHandled = false;

        private void HandlePlayerLeft(string playerName)
        {
            if (GlobalFlags.PlayerLeftHandled) return;
            GlobalFlags.PlayerLeftHandled = true;

            if (InvokeRequired)
            {
                Invoke(() => HandlePlayerLeft(playerName));
                return;
            }

            GameManager.OnPlayerLeft -= HandlePlayerLeft;
            MessageBox.Show($"{playerName} đã thoát khỏi trò chơi. Trò chơi kết thúc!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Rời phòng – KHÔNG restart
            Session.LeaveRoom();
            this.Hide();

            var jr = Application.OpenForms.OfType<JoinRoom>().FirstOrDefault();
            if (jr == null) new JoinRoom().Show();
            else jr.Activate();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            GameManager.OnPlayerLeft -= HandlePlayerLeft;
        }
    }
}
