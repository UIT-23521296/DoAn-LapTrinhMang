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
using MonopolyWinForms.Login_Signup;

namespace MonopolyWinForms.Room
{
    public partial class Waiting_Room_Host : Form
    {
        private CancellationTokenSource _cts;
        private FirebaseService _firebase;

        public Waiting_Room_Host()
        {
            InitializeComponent();
            this.FormClosing += Waiting_Room_Host_FormClosing;
            this.Load += Waiting_Room_Host_Load;
        }

        private async void Waiting_Room_Host_Load(object sender, EventArgs e)
        {
            _firebase = new FirebaseService();
            _cts = new CancellationTokenSource();
            await StartPollingPlayers(_cts.Token);
        }

        private async Task StartPollingPlayers(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var room = await _firebase.GetRoomAsync(Session.CurrentRoomId);
                    if (room != null)
                    {
                        if (InvokeRequired)
                        {
                            this.Invoke(new Action(() =>
                            {
                                UpdateRoomInfo(room);
                            }));
                        }
                        else
                        {
                            UpdateRoomInfo(room);
                        }
                    }
                }
                catch
                {
                    // Log lỗi nếu cần
                }

                await Task.Delay(500);
            }
        }

        private void UpdateRoomInfo(RoomInfo room)
        {
            txb_RoomName.Text = room.RoomName;
            UpdatePlayerNames(room.PlayerDisplayNames);
            
            // Cập nhật trạng thái nút Bắt đầu
            btn_Play.Enabled = room.CurrentPlayers >= 2 && 
                              room.PlayerDisplayNames.All(p => room.ReadyPlayers.Contains(p));
        }

        private void UpdatePlayerNames(List<string> playerNames)
        {
            TextBox[] playerTextBoxes = { txb_player1, txb_player2, txb_player3, txb_player4 };

            for (int i = 0; i < playerTextBoxes.Length; i++)
            {
                if (i < playerNames.Count)
                    playerTextBoxes[i].Text = playerNames[i];
                else
                    playerTextBoxes[i].Text = ""; // Trống nếu chưa có người chơi
            }
        }

        private async void Waiting_Room_Host_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cts?.Cancel();

            if (Session.IsHost && Session.CurrentRoomId != null)
            {
                try
                {
                    var room = await _firebase.GetRoomAsync(Session.CurrentRoomId);
                    if (room != null)
                    {
                        // Nếu còn người chơi khác trong phòng
                        if (room.PlayerDisplayNames.Count > 1)
                        {
                            // Chuyển quyền host cho người chơi đầu tiên khác host hiện tại
                            string newHostName = room.PlayerDisplayNames.FirstOrDefault(p => p != Session.UserName);
                            if (newHostName != null)
                            {
                                room.HostId = newHostName;
                                room.HostIP = "localhost"; // Hoặc lấy IP của người chơi mới
                                await _firebase.CreateRoomAsync(Session.CurrentRoomId, room);
                            }
                        }
                        else
                        {
                            // Nếu không còn ai trong phòng, xóa phòng
                            await _firebase.DeleteRoomAsync(Session.CurrentRoomId);
                        }
                    }
                    Session.LeaveRoom();
                    var roomListForm = new JoinRoom();
                    roomListForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xử lý thoát phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_Out_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form => kích hoạt FormClosing event
        }

        private async void btn_Play_Click(object sender, EventArgs e)
        {
            try
            {
                var room = await _firebase.GetRoomAsync(Session.CurrentRoomId);
                if (room != null)
                {
                    // Kiểm tra điều kiện bắt đầu
                    if (room.CurrentPlayers < 2)
                    {
                        MessageBox.Show("Cần ít nhất 2 người chơi để bắt đầu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (!room.PlayerDisplayNames.All(p => room.ReadyPlayers.Contains(p)))
                    {
                        MessageBox.Show("Tất cả người chơi phải sẵn sàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Cập nhật trạng thái phòng
                    room.IsStarted = true;
                    await _firebase.CreateRoomAsync(Session.CurrentRoomId, room);

                    // Mở form game
                    // TODO: Mở form game mới
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi bắt đầu game: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
