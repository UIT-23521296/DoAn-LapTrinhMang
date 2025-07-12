using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonopolyWinForms.Home
{
    public partial class Rule : Form
    {
        private int scrollOffset = 0;
        private Label ruleLabel;
        private Panel scrollPanel;
        public Rule()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = true; // vẫn có nút X
            this.Size = new Size(850, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            // ===== Đặt ảnh nền =====
            string path = Path.Combine(Application.StartupPath, "Assets", "Images", "background.png");
            this.BackgroundImage = Image.FromFile(path);
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // ===== Header =====
            var headerPanel = new Panel
            {
                Height = 70,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent
            };

            var title = new Label
            {
                Text = "Cờ tỷ phú",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(250, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(180, 245, 166, 35)
            };

            title.Location = new Point((headerPanel.Width - title.Width) / 2, 10);
            headerPanel.Controls.Add(title);
            headerPanel.Resize += (s, e) =>
            {
                title.Location = new Point((headerPanel.Width - title.Width) / 2, 10);
            };

            // ===== Nội dung =====
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 10, 20, 10),
                BackColor = Color.Transparent
            };

            scrollPanel = new Panel
            {
                Size = new Size(700, 380),
                Anchor = AnchorStyles.None,
                BackColor = Color.FromArgb(160, 255, 255, 255),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Vô hiệu thanh cuộn mặc định
            scrollPanel.AutoScroll = false;

            var ruleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11),
                ForeColor = ColorTranslator.FromHtml("#2F2F2F"),
                MaximumSize = new Size(680, 0),
                Location = new Point(0, 0),
                Text = @"I. Cách chơi
Mỗi lượt người chơi sẽ được tung 2 xúc xắc, tổng số chấm trên xúc xắc sẽ là số bước nhân vật di chuyển, đi đến ô nào người chơi sẽ thực hiện hành động tương ứng với ô đó, ví dụ: Mua đất, xây nhà, trả tiền thuê nhà,…

II. Mục tiêu
Cố gắng làm các người chơi khác phá sản hoặc trở thành người giàu nhất.

III. Bàn cờ
Trên bàn cờ sẽ có các ô:
- Đất: Có thể mua, xây nhà, thu tiền thuê
- Bến xe: Có thể mua, thu tiền thuê theo số lượng bến xe sở hữu
- Công ty: Có thể mua, thu tiền thuê theo số trên xúc xắc
- Cơ hội / Khí vận: Rút thẻ ngẫu nhiên
- Thuế: Nộp thuế
- Nhà tù: Bị giữ 3 lượt, có thể thoát bằng nhiều cách
- Ô bắt đầu: Nhận tiền thưởng khi đi qua

IV. Lượt chơi
1. Người chơi đổ 2 viên xúc xắc.
2. Di chuyển theo tổng số điểm.
3. Sau khi di chuyển, hành động theo ô tương ứng:
   - Đất trống => có thể mua nếu đủ tiền.
   - Đất có chủ => trả tiền thuê.
   - Ô đặc biệt => thực hiện hành động tương ứng.
4. Nếu đổ ra 2 con xúc xắc giống nhau (gấp đôi):
   - Người chơi được thêm 1 lượt.
   - Nếu ra gấp đôi 3 lần liên tiếp => bị vào tù.

V. Mua đất và xây nhà
1. Lần đầu dừng ở ô đất trống
   - Bạn có quyền mua đất (nếu đủ tiền).
   - Chưa được xây nhà ngay trong lượt này.
2. Lần sau dừng lại trên chính ô đất bạn sở hữu
   - Xây thêm nhà trên đất đó (tối đa 3 căn nhà).
3. Khi đã có 4 căn nhà
   - Ở lần dừng tiếp theo, bạn có thể nâng cấp lên khách sạn.
   - Một ô đất chỉ có 1 khách sạn là tối đa.
4. Tiền thuê tăng theo số lượng nhà/khách sạn
   - Người chơi khác khi dừng tại ô đất của bạn sẽ trả tiền thuê.
   - Càng nhiều nhà (hoặc khách sạn), tiền thuê càng cao.

VI. Tiền thuê
Khi người chơi dừng ở ô đã có chủ, người chơi phải trả tiền thuê:
- Ô đất: Càng nhiều nhà, tiền thuê càng cao.
- Bến xe: Càng nhiều bến xe, tiền thuê càng cao.
- Công ty: Người chơi sẽ đổ xúc xắc và trả tiền theo tổng điểm.
Nếu không đủ tiền mặt, người chơi có thể bán đất cho ngân hàng (50% giá trị).
Nếu tổng tài sản không đủ chi trả => người chơi phá sản.

VII. Nhà tù
Người chơi sẽ bị vào tù khi:
- Rút trúng thẻ “Đi thẳng vào tù”.
- Dừng ở ô “Đi tù”.
Cách ra khỏi tù:
- Đổ gấp đôi để ra.
- Nộp $200 để ra.
- Sử dụng thẻ “Tự do ra tù”.

VIII. Chiến thắng
Người chơi sẽ chiến thắng khi tất cả người chơi còn lại phá sản.
Hoặc nếu hết thời gian, sẽ tính tổng tài sản, người có tổng tài sản cao nhất sẽ thắng."
            };

            scrollPanel.Controls.Add(ruleLabel);
            contentPanel.Controls.Add(scrollPanel);

            // Scroll bằng chuột
            scrollPanel.MouseWheel += (s, e) =>
            {
                scrollOffset -= e.Delta / 4;
                scrollOffset = Math.Max(0, Math.Min(ruleLabel.Height - scrollPanel.Height, scrollOffset));
                ruleLabel.Location = new Point(0, -scrollOffset);
            };

            scrollPanel.MouseEnter += (s, e) => scrollPanel.Focus();
            scrollPanel.TabStop = true;

            // Căn giữa khi resize
            contentPanel.Resize += (s, e) =>
            {
                scrollPanel.Location = new Point(
                    (contentPanel.Width - scrollPanel.Width) / 2,
                    (contentPanel.Height - scrollPanel.Height) / 2
                );
            };

            // ===== Nút đóng =====
            var btnClose = new Button
            {
                Text = "Đóng",
                BackColor = ColorTranslator.FromHtml("#33B68F"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Width = 90,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width, btnClose.Height, 20, 20));
            btnClose.Click += (s, e) => this.Close();

            var bottomPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                BackColor = Color.Transparent
            };
            bottomPanel.Controls.Add(btnClose);
            btnClose.Location = new Point((bottomPanel.Width - btnClose.Width) / 2, 10);
            bottomPanel.Resize += (s, e) =>
            {
                btnClose.Location = new Point((bottomPanel.Width - btnClose.Width) / 2, 10);
            };

            // Thêm tất cả vào form
            this.Controls.Add(contentPanel);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(headerPanel);
        }

        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeft, int nTop, int nRight, int nBottom, int nWidthEllipse, int nHeightEllipse);
    }
}
