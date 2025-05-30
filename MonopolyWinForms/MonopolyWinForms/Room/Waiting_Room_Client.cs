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
    public partial class Waiting_Room_Client: Form
    {
        private string roomId;
        private FirebaseService firebase;
        private System.Windows.Forms.Timer refreshTimer;


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
        }

        private async void RefreshTimer_Tick(object sender, EventArgs e)
        {
            var room = await firebase.GetRoomAsync(roomId);
            if (room == null)
            {
                refreshTimer.Stop();
                MessageBox.Show("Phòng đã bị đóng hoặc không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            refreshTimer.Stop();

            var room = await firebase.GetRoomAsync(roomId);
            if (room != null)
            {
                string currentUserId = SessionManager.CurrentUserId;
                if (room.PlayerDisplayNames.Contains(currentUserId))
                {
                    room.PlayerDisplayNames.Remove(currentUserId);
                    await firebase.CreateRoomAsync(roomId, room);
                }
            }

            JoinRoom joinRoomForm = new JoinRoom();
            joinRoomForm.Show();
        }

        private void btn_Out_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
