using MonopolyWinForms.Login_Signup;
using MonopolyWinForms.Services;
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
        private System.Windows.Forms.Timer refreshTimer;
        private Label lblStatus;
        private Dictionary<string, DateTime> lastRoomUpdate = new Dictionary<string, DateTime>();

        public JoinRoom()
        {
            InitializeComponent();
            SetupUI();
            StartRoomRefreshTimer();
        }

        private void StartRoomRefreshTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 500;
            refreshTimer.Tick += async (s, e) =>
            {
                await LoadRoomsFromFirebase();
            };
            refreshTimer.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            base.OnFormClosing(e);
        }

        private void SetupUI()
        {
            // Form cơ bản
            this.Text = "Chọn phòng";
            this.Size = new Size(1140, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Panel chính
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            this.Controls.Add(mainPanel);

            // Tiêu đề với hiệu ứng gradient
            Label lblTitle = new Label
            {
                Text = "CHỌN PHÒNG CHƠI",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 100,
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            mainPanel.Controls.Add(lblTitle);

            // Label trạng thái
            lblStatus = new Label
            {
                Text = "Đang tải danh sách phòng...",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(52, 152, 219),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };
            mainPanel.Controls.Add(lblStatus);

            // Panel cho DataGridView
            Panel gridPanel = new Panel
            {
                Location = new Point(50, 150),
                Size = new Size(1000, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(gridPanel);

            // DataGridView với thiết kế mới
            dgvRooms = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Font = new Font("Segoe UI", 12),
                RowTemplate = { Height = 50 },
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                RowHeadersVisible = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(224, 224, 224)
            };

            // Tùy chỉnh header
            dgvRooms.EnableHeadersVisualStyles = false;
            dgvRooms.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvRooms.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRooms.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgvRooms.ColumnHeadersHeight = 50;

            // Thêm cột
            dgvRooms.Columns.Add("RoomName", "Tên phòng");
            dgvRooms.Columns.Add("HostName", "Chủ phòng");
            dgvRooms.Columns.Add("PlayerCount", "Số người");
            dgvRooms.Columns.Add("CreatedAt", "Thời gian tạo");

            // Căn giữa và đặt width cố định cho cột
            dgvRooms.Columns["RoomName"].Width = 300;
            dgvRooms.Columns["HostName"].Width = 300;
            dgvRooms.Columns["PlayerCount"].Width = 200;
            dgvRooms.Columns["CreatedAt"].Width = 200;

            foreach (DataGridViewColumn column in dgvRooms.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                column.ReadOnly = true;
                column.Resizable = DataGridViewTriState.False;
            }

            gridPanel.Controls.Add(dgvRooms);

            // Panel cho các nút
            Panel buttonPanel = new Panel
            {
                Location = new Point(50, 670),
                Size = new Size(1040, 80),
                BackColor = Color.Transparent
            };
            mainPanel.Controls.Add(buttonPanel);

            // Nút Tạo phòng với thiết kế mới
            btnCreateRoom = new Button
            {
                Text = "Tạo phòng",
                Size = new Size(200, 60),
                Location = new Point(320, 10),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCreateRoom.FlatAppearance.BorderSize = 0;
            btnCreateRoom.Click += BtnCreateRoom_Click;
            buttonPanel.Controls.Add(btnCreateRoom);

            // Nút Vào phòng với thiết kế mới
            btnJoinRoom = new Button
            {
                Text = "Vào phòng",
                Size = new Size(200, 60),
                Location = new Point(620, 10),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnJoinRoom.FlatAppearance.BorderSize = 0;
            btnJoinRoom.Click += BtnJoinRoom_Click;
            buttonPanel.Controls.Add(btnJoinRoom);

            // Thêm hiệu ứng hover cho các nút
            btnCreateRoom.MouseEnter += (s, e) => btnCreateRoom.BackColor = Color.FromArgb(39, 174, 96);
            btnCreateRoom.MouseLeave += (s, e) => btnCreateRoom.BackColor = Color.FromArgb(46, 204, 113);
            btnJoinRoom.MouseEnter += (s, e) => btnJoinRoom.BackColor = Color.FromArgb(41, 128, 185);
            btnJoinRoom.MouseLeave += (s, e) => btnJoinRoom.BackColor = Color.FromArgb(52, 152, 219);
        }

        private void BtnCreateRoom_Click(object sender, EventArgs e)
        {
            Create_Room createRoomForm = new Create_Room();
            createRoomForm.Show();
            this.Hide();
        }

        private async void BtnJoinRoom_Click(object sender, EventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập trước khi tham gia phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvRooms.SelectedRows.Count > 0)
            {
                string roomName = dgvRooms.SelectedRows[0].Cells["RoomName"].Value.ToString();
                var firebase = new FirebaseService();
                var rooms = await firebase.GetAllRoomsAsync();
                string roomId = rooms?.FirstOrDefault(r => r.Value.RoomName == roomName).Key;

                if (roomId != null)
                {
                    var room = await firebase.GetRoomAsync(roomId);
                    
                    if (room.CurrentPlayers >= room.MaxPlayers)
                    {
                        MessageBox.Show("Phòng đã đầy.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (room.IsStarted)
                    {
                        MessageBox.Show("Phòng đã bắt đầu chơi.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (Session.CurrentRoomId != null)
                    {
                        MessageBox.Show("Bạn đang trong một phòng khác. Vui lòng thoát phòng hiện tại trước.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        room.PlayerDisplayNames.Add(Session.UserName);
                        room.PlayerIds.Add(Session.UserId);
                        await firebase.CreateRoomAsync(roomId, room);

                        Session.JoinRoom(roomId, false);

                        Waiting_Room_Client waitingRoomClientForm = new Waiting_Room_Client(roomId);
                        waitingRoomClientForm.Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tham gia phòng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phòng này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phòng để vào.");
            }
        }

        private async Task LoadRoomsFromFirebase()
        {
            try
            {
                var firebase = new FirebaseService();
                var rooms = await firebase.GetAllRoomsAsync();

                if (rooms != null)
                {
                    var currentRooms = new Dictionary<string, DateTime>();
                    var newRooms = new List<string>();

                    // Lọc và sắp xếp phòng
                    var availableRooms = rooms
                        .Where(r => !r.Value.IsStarted && r.Value.CurrentPlayers < r.Value.MaxPlayers)
                        .OrderByDescending(r => r.Value.CreatedAt)
                        .ToList();

                    // Kiểm tra phòng mới
                    foreach (var room in availableRooms)
                    {
                        currentRooms[room.Key] = room.Value.CreatedAt;
                        if (!lastRoomUpdate.ContainsKey(room.Key))
                        {
                            newRooms.Add(room.Key);
                        }
                    }

                    // Cập nhật UI
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() => UpdateRoomsUI(availableRooms, newRooms)));
                    }
                    else
                    {
                        UpdateRoomsUI(availableRooms, newRooms);
                    }

                    // Cập nhật trạng thái
                    lastRoomUpdate = currentRooms;
                    lblStatus.Text = $"Đã tải {availableRooms.Count} phòng";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi khi tải danh sách phòng";
                MessageBox.Show("Lỗi khi tải phòng: " + ex.Message);
            }
        }

        private void UpdateRoomsUI(List<KeyValuePair<string, RoomInfo>> rooms, List<string> newRooms)
        {
            // Ghi nhớ dòng đang được chọn (nếu có)
            string selectedRoomName = null;
            if (dgvRooms.SelectedRows.Count > 0)
            {
                selectedRoomName = dgvRooms.SelectedRows[0].Cells["RoomName"].Value.ToString();
            }

            dgvRooms.Rows.Clear();

            foreach (var room in rooms)
            {
                int rowIndex = dgvRooms.Rows.Add(
                    room.Value.RoomName,
                    room.Value.HostIP,
                    $"{room.Value.CurrentPlayers}/{room.Value.MaxPlayers}",
                    room.Value.CreatedAt.ToString("HH:mm:ss dd/MM/yyyy")
                );

                // Đánh dấu phòng mới
                if (newRooms.Contains(room.Key))
                {
                    dgvRooms.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(230, 255, 230);
                    dgvRooms.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 255, 200);
                }

                // Nếu là dòng đang được chọn trước đó thì chọn lại
                if (room.Value.RoomName == selectedRoomName)
                {
                    dgvRooms.Rows[rowIndex].Selected = true;
                }
            }

            // Nếu không có dòng nào được chọn thì xóa chọn
            if (dgvRooms.SelectedRows.Count == 0 && dgvRooms.Rows.Count > 0)
            {
                dgvRooms.ClearSelection();
            }
        }

    }
}
