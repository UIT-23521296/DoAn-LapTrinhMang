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
            btn_login = new Button();
            btn_signup = new Button();
            btn_quit = new Button();
            login_img = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)login_img).BeginInit();
            SuspendLayout();
            // 
            // btn_login
            // 
            btn_login.BackColor = Color.FromArgb(255, 224, 192);
            btn_login.Font = new Font("Segoe UI", 15F);
            btn_login.Location = new Point(908, 164);
            btn_login.Margin = new Padding(5);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(380, 103);
            btn_login.TabIndex = 0;
            btn_login.Text = "ĐĂNG NHẬP";
            btn_login.UseVisualStyleBackColor = false;
            btn_login.Click += btn_login_Click;
            // 
            // btn_signup
            // 
            btn_signup.Font = new Font("Segoe UI", 15F);
            btn_signup.Location = new Point(908, 341);
            btn_signup.Margin = new Padding(5);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new Size(380, 103);
            btn_signup.TabIndex = 1;
            btn_signup.Text = "ĐĂNG KÝ";
            btn_signup.UseVisualStyleBackColor = true;
            btn_signup.Click += btn_signup_Click;
            // 
            // btn_quit
            // 
            btn_quit.Location = new Point(908, 522);
            btn_quit.Margin = new Padding(5);
            btn_quit.Name = "btn_quit";
            btn_quit.Size = new Size(380, 103);
            btn_quit.TabIndex = 2;
            btn_quit.Text = "THOÁT GAME";
            btn_quit.UseVisualStyleBackColor = true;
            btn_quit.Click += btn_quit_Click;
            // 
            // login_img
            // 
            login_img.Image = Properties.Resources.login_img;
            login_img.Location = new Point(75, 54);
            login_img.Margin = new Padding(5);
            login_img.Name = "login_img";
            login_img.Size = new Size(800, 677);
            login_img.SizeMode = PictureBoxSizeMode.StretchImage;
            login_img.TabIndex = 4;
            login_img.TabStop = false;
            // 
            // Main_login_signup
            // 
            AutoScaleDimensions = new SizeF(14F, 35F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Cornsilk;
            ClientSize = new Size(1400, 788);
            Controls.Add(login_img);
            Controls.Add(btn_quit);
            Controls.Add(btn_signup);
            Controls.Add(btn_login);
            Font = new Font("Segoe UI", 15F);
            Margin = new Padding(5);
            Name = "Main_login_signup";
            Text = "Main_login_signup";
            ((System.ComponentModel.ISupportInitialize)login_img).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btn_login;
        private Button btn_signup;
        private Button btn_quit;
        private PictureBox login_img;
    }
}