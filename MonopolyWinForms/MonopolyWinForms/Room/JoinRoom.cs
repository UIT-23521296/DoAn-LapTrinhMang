using MonopolyWinForms.Login_Signup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonopolyWinForms.Room
{
    public partial class JoinRoom: Form
    {
        private DataGridView dgvRooms;
        private Button btnCreateRoom;
        private Button btnJoinRoom;

        public JoinRoom()
        {
            InitializeComponent();
            SetupUI();
            LoadRoomsFromFirebase();

        }

        private void SetupUI()
        {
            // Form cơ bản
            this.Text = "Chọn phòng";
            this.Size = new Size(1140, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.LightGray;

            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = "CHỌN PHÒNG CHƠI",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 100,
                ForeColor = Color.DarkSlateGray
            };
            this.Controls.Add(lblTitle);

            // DataGridView
            dgvRooms = new DataGridView
            {
                Location = new Point(250, 150),
                Size = new Size(620, 450),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Font = new Font("Segoe UI", 14),
                RowTemplate = { Height = 40 },
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false, // KHÓA resize cột
                AllowUserToResizeRows = false     // KHÓA resize hàng
            };

            // Thêm cột
            dgvRooms.Columns.Add("RoomName", "Tên phòng");
            dgvRooms.Columns.Add("HostName", "Chủ phòng");
            dgvRooms.Columns.Add("PlayerCount", "Số người");

            // Căn giữa và đặt width cố định cho cột, KHÓA resize cột
            dgvRooms.Columns["RoomName"].Width = 250;
            dgvRooms.Columns["HostName"].Width = 217;
            dgvRooms.Columns["PlayerCount"].Width = 150;

            foreach (DataGridViewColumn column in dgvRooms.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                column.ReadOnly = true;
                column.Resizable = DataGridViewTriState.False;  // KHÓA resize cột
            }

            // Dữ liệu mẫu
            dgvRooms.Rows.Add("phòng 1", "Chủ phòng 1", "3/4");
            dgvRooms.Rows.Add("phòng 2", "Chủ phòng 2", "1/4");
            dgvRooms.Rows.Add("phòng 3", "Chủ phòng 3", "2/4");
            dgvRooms.Rows.Add("phòng 4", "Chủ phòng 4", "4/4");

            this.Controls.Add(dgvRooms);

            // Nút Tạo phòng
            btnCreateRoom = new Button
            {
                Text = "Tạo phòng",
                Size = new Size(180, 60),
                Location = new Point(300, 630),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            btnCreateRoom.FlatAppearance.BorderSize = 0;
            btnCreateRoom.Click += BtnCreateRoom_Click;
            this.Controls.Add(btnCreateRoom);

            // Nút Vào phòng
            btnJoinRoom = new Button
            {
                Text = "Vào phòng",
                Size = new Size(180, 60),
                Location = new Point(640, 630),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.SkyBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnJoinRoom.FlatAppearance.BorderSize = 0;
            btnJoinRoom.Click += BtnJoinRoom_Click;
            this.Controls.Add(btnJoinRoom);
        }

        private void BtnCreateRoom_Click(object sender, EventArgs e)
        {
            Create_Room createRoomForm = new Create_Room();
            createRoomForm.Show(); // mở không chặn form hiện tại
        }

        private void BtnJoinRoom_Click(object sender, EventArgs e)
        {
            if (dgvRooms.SelectedRows.Count > 0)
            {
                string roomName = dgvRooms.SelectedRows[0].Cells["RoomName"].Value.ToString();
                MessageBox.Show($"Đang vào {roomName}");
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng để vào.");
            }
        }
        private async void LoadRoomsFromFirebase()
        {
            dgvRooms.Rows.Clear();

            try
            {
                var firebase = new FirebaseService();
                var rooms = await firebase.GetAllRoomsAsync();

                if (rooms != null)
                {
                    foreach (var kv in rooms)
                    {
                        var room = kv.Value;
                        dgvRooms.Rows.Add(
                            room.RoomName,
                            room.HostIP,
                            $"{room.CurrentPlayers}/{room.MaxPlayers}"
                        );
                    }
                }
                else
                {
                    MessageBox.Show("Không có phòng nào hoặc không thể tải danh sách phòng từ Firebase.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải phòng: " + ex.Message);
            }
        }


    }
}
