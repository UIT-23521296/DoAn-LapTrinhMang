namespace MonopolyWinForms.Home
{
    partial class Main_home
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_home));
            login_img = new PictureBox();
            btn_quit = new Button();
            btn_play = new Button();
            btn_rule = new Button();
            ((System.ComponentModel.ISupportInitialize)login_img).BeginInit();
            SuspendLayout();
            // 
            // login_img
            // 
            login_img.Image = Properties.Resources.login_img;
            login_img.Location = new Point(89, 70);
            login_img.Margin = new Padding(6, 6, 6, 6);
            login_img.Name = "login_img";
            login_img.Size = new Size(780, 616);
            login_img.SizeMode = PictureBoxSizeMode.StretchImage;
            login_img.TabIndex = 8;
            login_img.TabStop = false;
            // 
            // btn_quit
            // 
            btn_quit.BackColor = Color.FromArgb(220, 53, 69);
            btn_quit.FlatAppearance.BorderSize = 0;
            btn_quit.FlatStyle = FlatStyle.Flat;
            btn_quit.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            btn_quit.ForeColor = Color.White;
            btn_quit.Location = new Point(956, 549);
            btn_quit.Margin = new Padding(6, 6, 6, 6);
            btn_quit.Name = "btn_quit";
            btn_quit.Size = new Size(475, 129);
            btn_quit.TabIndex = 7;
            btn_quit.Text = "THOÁT GAME";
            btn_quit.UseVisualStyleBackColor = false;
            btn_quit.Click += btn_quit_Click;
            // 
            // btn_play
            // 
            btn_play.BackColor = Color.FromArgb(255, 193, 7);
            btn_play.FlatAppearance.BorderSize = 0;
            btn_play.FlatStyle = FlatStyle.Flat;
            btn_play.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            btn_play.ForeColor = Color.Black;
            btn_play.Location = new Point(956, 89);
            btn_play.Margin = new Padding(6, 6, 6, 6);
            btn_play.Name = "btn_play";
            btn_play.Size = new Size(475, 129);
            btn_play.TabIndex = 5;
            btn_play.Text = "CHƠI NGAY";
            btn_play.UseVisualStyleBackColor = false;
            btn_play.Click += btn_play_Click;
            // 
            // btn_rule
            // 
            btn_rule.BackColor = Color.FromArgb(40, 167, 69);
            btn_rule.FlatAppearance.BorderSize = 0;
            btn_rule.FlatStyle = FlatStyle.Flat;
            btn_rule.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            btn_rule.ForeColor = Color.White;
            btn_rule.Location = new Point(956, 311);
            btn_rule.Margin = new Padding(6, 6, 6, 6);
            btn_rule.Name = "btn_rule";
            btn_rule.Size = new Size(475, 129);
            btn_rule.TabIndex = 9;
            btn_rule.Text = "LUẬT CHƠI";
            btn_rule.UseVisualStyleBackColor = false;
            btn_rule.Click += btn_rule_Click;
            // 
            // Main_home
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1610, 752);
            Controls.Add(btn_rule);
            Controls.Add(login_img);
            Controls.Add(btn_quit);
            Controls.Add(btn_play);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Main_home";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cờ tỷ phú";
            ((System.ComponentModel.ISupportInitialize)login_img).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox login_img;
        private Button btn_quit;
        private Button btn_play;
        private Button btn_rule;
    }
}
