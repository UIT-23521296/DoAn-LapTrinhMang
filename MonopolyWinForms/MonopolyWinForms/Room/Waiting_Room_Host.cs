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
                    var room = await _firebase.GetRoomAsync(SessionManager.CurrentRoom.RoomId);
                    if (room != null)
                    {
                        if (InvokeRequired)
                        {
                            this.Invoke(new Action(() =>
                            {
                                txb_RoomName.Text = room.RoomName;
                                UpdatePlayerNames(room.PlayerDisplayNames);
                            }));
                        }
                        else
                        {
                            txb_RoomName.Text = room.RoomName;
                            UpdatePlayerNames(room.PlayerDisplayNames);
                        }
                    }
                }
                catch
                {
                    // Bạn có thể log lỗi nếu cần
                }

                await Task.Delay(3000);
            }
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

            var room = SessionManager.CurrentRoom;
            var currentUserId = SessionManager.CurrentUserId;

            if (room != null && room.HostId == currentUserId)
            {
                try
                {
                    await _firebase.DeleteRoomAsync(room.RoomId);
                    // Mở lại form danh sách phòng
                    var roomListForm = new JoinRoom();
                    roomListForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_Out_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form => kích hoạt FormClosing event
        }

    }
}
