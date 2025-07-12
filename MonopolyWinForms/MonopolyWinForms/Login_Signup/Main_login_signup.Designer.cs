using System.Drawing.Drawing2D;
namespace MonopolyWinForms.Login_Signup
{
    partial class Main_login_signup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_login_signup));
            btn_login = new Button();
            btn_signup = new Button();
            btn_quit = new Button();
            login_img = new PictureBox();
            titleLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)login_img).BeginInit();
            SuspendLayout();
            // 
            // btn_login
            // 
            btn_login.BackColor = Color.Gold;
            btn_login.FlatAppearance.BorderSize = 0;
            btn_login.FlatStyle = FlatStyle.Flat;
            btn_login.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btn_login.ForeColor = Color.Black;
            btn_login.Location = new Point(920, 150);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(360, 80);
            btn_login.TabIndex = 1;
            btn_login.Text = "🔐 ĐĂNG NHẬP";
            btn_login.UseVisualStyleBackColor = false;
            btn_login.Click += btn_login_Click;
            // 
            // btn_signup
            // 
            btn_signup.BackColor = Color.MediumSeaGreen;
            btn_signup.FlatAppearance.BorderSize = 0;
            btn_signup.FlatStyle = FlatStyle.Flat;
            btn_signup.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btn_signup.ForeColor = Color.White;
            btn_signup.Location = new Point(920, 296);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new Size(360, 80);
            btn_signup.TabIndex = 2;
            btn_signup.Text = "📝 ĐĂNG KÝ";
            btn_signup.UseVisualStyleBackColor = false;
            btn_signup.Click += btn_signup_Click;
            // 
            // btn_quit
            // 
            btn_quit.BackColor = Color.IndianRed;
            btn_quit.FlatAppearance.BorderSize = 0;
            btn_quit.FlatStyle = FlatStyle.Flat;
            btn_quit.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btn_quit.ForeColor = Color.White;
            btn_quit.Location = new Point(920, 451);
            btn_quit.Name = "btn_quit";
            btn_quit.Size = new Size(360, 80);
            btn_quit.TabIndex = 3;
            btn_quit.Text = "❌ THOÁT GAME";
            btn_quit.UseVisualStyleBackColor = false;
            btn_quit.Click += btn_quit_Click;
            // 
            // login_img
            // 
            login_img.Image = Properties.Resources.login_img;
            login_img.Location = new Point(99, 40);
            login_img.Name = "login_img";
            login_img.Size = new Size(659, 512);
            login_img.SizeMode = PictureBoxSizeMode.StretchImage;
            login_img.TabIndex = 4;
            login_img.TabStop = false;
            // 
            // titleLabel
            // 
            titleLabel.Font = new Font("Arial", 32F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(200, 0, 0);
            titleLabel.Location = new Point(900, 40);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(400, 80);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "CỜ TỶ PHÚ";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Main_login_signup
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 250, 240);
            ClientSize = new Size(1400, 574);
            Controls.Add(titleLabel);
            Controls.Add(btn_login);
            Controls.Add(btn_signup);
            Controls.Add(btn_quit);
            Controls.Add(login_img);
            Font = new Font("Segoe UI", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Main_login_signup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cờ tỷ phú";
            ((System.ComponentModel.ISupportInitialize)login_img).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btn_login;
        private Button btn_signup;
        private Button btn_quit;
        private PictureBox login_img;
        private Label titleLabel;
    }
}