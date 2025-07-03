namespace MonopolyWinForms.Home
{
    partial class Main_home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_home));
            login_img = new PictureBox();
            btn_quit = new Button();
            btn_setting = new Button();
            btn_play = new Button();
            btn_rule = new Button();
            ((System.ComponentModel.ISupportInitialize)login_img).BeginInit();
            SuspendLayout();
            // 
            // login_img
            // 
            login_img.Image = Properties.Resources.login_img;
            login_img.Location = new Point(94, 56);
            login_img.Margin = new Padding(5);
            login_img.Name = "login_img";
            login_img.Size = new Size(800, 677);
            login_img.SizeMode = PictureBoxSizeMode.StretchImage;
            login_img.TabIndex = 8;
            login_img.TabStop = false;
            // 
            // btn_quit
            // 
            btn_quit.Font = new Font("Segoe UI", 15F);
            btn_quit.Location = new Point(927, 620);
            btn_quit.Margin = new Padding(5);
            btn_quit.Name = "btn_quit";
            btn_quit.Size = new Size(380, 103);
            btn_quit.TabIndex = 7;
            btn_quit.Text = "THOÁT GAME";
            btn_quit.UseVisualStyleBackColor = true;
            btn_quit.Click += btn_quit_Click;
            // 
            // btn_setting
            // 
            btn_setting.Font = new Font("Segoe UI", 15F);
            btn_setting.Location = new Point(927, 248);
            btn_setting.Margin = new Padding(5);
            btn_setting.Name = "btn_setting";
            btn_setting.Size = new Size(380, 103);
            btn_setting.TabIndex = 6;
            btn_setting.Text = "CÀI ĐẶT";
            btn_setting.UseVisualStyleBackColor = true;
            // 
            // btn_play
            // 
            btn_play.BackColor = Color.FromArgb(255, 224, 192);
            btn_play.Font = new Font("Segoe UI", 15F);
            btn_play.Location = new Point(927, 76);
            btn_play.Margin = new Padding(5);
            btn_play.Name = "btn_play";
            btn_play.Size = new Size(380, 103);
            btn_play.TabIndex = 5;
            btn_play.Text = "CHƠI NGAY";
            btn_play.UseVisualStyleBackColor = false;
            btn_play.Click += btn_play_Click;
            // 
            // btn_rule
            // 
            btn_rule.Font = new Font("Segoe UI", 15F);
            btn_rule.Location = new Point(927, 434);
            btn_rule.Margin = new Padding(5);
            btn_rule.Name = "btn_rule";
            btn_rule.Size = new Size(380, 103);
            btn_rule.TabIndex = 9;
            btn_rule.Text = "LUẬT CHƠI";
            btn_rule.UseVisualStyleBackColor = true;
            // 
            // Main_home
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1400, 788);
            Controls.Add(btn_rule);
            Controls.Add(login_img);
            Controls.Add(btn_quit);
            Controls.Add(btn_setting);
            Controls.Add(btn_play);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
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
        private Button btn_setting;
        private Button btn_play;
        private Button btn_rule;
    }
}