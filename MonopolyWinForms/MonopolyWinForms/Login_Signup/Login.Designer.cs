namespace MonopolyWinForms.Login_Signup
{
    partial class Login
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            label1 = new Label();
            tb_login = new TextBox();
            tb_password = new TextBox();
            label2 = new Label();
            btn_login = new Button();
            btn_signup = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Bahnschrift SemiBold", 30F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 30);
            label1.Name = "label1";
            label1.Size = new Size(800, 65);
            label1.TabIndex = 0;
            label1.Text = "ĐĂNG NHẬP";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tb_login
            // 
            tb_login.BackColor = Color.White;
            tb_login.BorderStyle = BorderStyle.FixedSingle;
            tb_login.Font = new Font("Segoe UI", 13F);
            tb_login.Location = new Point(220, 113);
            tb_login.Name = "tb_login";
            tb_login.PlaceholderText = "Email";
            tb_login.Size = new Size(380, 36);
            tb_login.TabIndex = 1;
            // 
            // tb_password
            // 
            tb_password.BackColor = Color.White;
            tb_password.BorderStyle = BorderStyle.FixedSingle;
            tb_password.Font = new Font("Segoe UI", 13F);
            tb_password.Location = new Point(220, 176);
            tb_password.Name = "tb_password";
            tb_password.PasswordChar = '●';
            tb_password.PlaceholderText = "Mật khẩu";
            tb_password.Size = new Size(380, 36);
            tb_password.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Cursor = Cursors.Hand;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Underline);
            label2.ForeColor = Color.FromArgb(0, 150, 255);
            label2.Location = new Point(220, 230);
            label2.Name = "label2";
            label2.Size = new Size(129, 23);
            label2.TabIndex = 3;
            label2.Text = "Quên mật khẩu";
            // 
            // btn_login
            // 
            btn_login.BackColor = Color.Goldenrod;
            btn_login.FlatAppearance.BorderSize = 0;
            btn_login.FlatStyle = FlatStyle.Flat;
            btn_login.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btn_login.ForeColor = Color.Black;
            btn_login.Location = new Point(149, 275);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(200, 50);
            btn_login.TabIndex = 4;
            btn_login.Text = "ĐĂNG NHẬP";
            btn_login.UseVisualStyleBackColor = false;
            btn_login.Click += btn_login_Click;
            // 
            // btn_signup
            // 
            btn_signup.BackColor = Color.MediumSeaGreen;
            btn_signup.FlatAppearance.BorderSize = 0;
            btn_signup.FlatStyle = FlatStyle.Flat;
            btn_signup.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btn_signup.ForeColor = Color.Black;
            btn_signup.Location = new Point(437, 275);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new Size(200, 50);
            btn_signup.TabIndex = 5;
            btn_signup.Text = "ĐĂNG KÝ";
            btn_signup.UseVisualStyleBackColor = false;
            btn_signup.Click += btn_signup_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background2;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(800, 360);
            Controls.Add(label1);
            Controls.Add(tb_login);
            Controls.Add(tb_password);
            Controls.Add(label2);
            Controls.Add(btn_login);
            Controls.Add(btn_signup);
            Font = new Font("Segoe UI", 12F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private Label label1;
        private Label label2;
        private TextBox tb_password;
        private TextBox tb_login;
        private Button btn_login;
        private Button btn_signup;
        private Label label3;
        private Label label4;
    }
}
