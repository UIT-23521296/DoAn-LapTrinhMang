namespace MonopolyWinForms.Login_Signup
{
    partial class Signup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Signup));
            label1 = new Label();
            tb_login = new TextBox();
            tb_password = new TextBox();
            tb_password2 = new TextBox();
            tb_email = new TextBox();
            btn_singup = new Button();
            btn_login = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Bahnschrift SemiBold", 28F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(2, 37);
            label1.Name = "label1";
            label1.Size = new Size(800, 60);
            label1.TabIndex = 0;
            label1.Text = "ĐĂNG KÝ";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tb_login
            // 
            tb_login.BackColor = Color.White;
            tb_login.BorderStyle = BorderStyle.FixedSingle;
            tb_login.Font = new Font("Segoe UI", 13F);
            tb_login.Location = new Point(220, 114);
            tb_login.Name = "tb_login";
            tb_login.PlaceholderText = "Tên đăng nhập";
            tb_login.Size = new Size(400, 36);
            tb_login.TabIndex = 1;
            // 
            // tb_password
            // 
            tb_password.BackColor = Color.White;
            tb_password.BorderStyle = BorderStyle.FixedSingle;
            tb_password.Font = new Font("Segoe UI", 13F);
            tb_password.Location = new Point(220, 236);
            tb_password.Name = "tb_password";
            tb_password.PasswordChar = '●';
            tb_password.PlaceholderText = "Mật khẩu";
            tb_password.Size = new Size(400, 36);
            tb_password.TabIndex = 2;
            // 
            // tb_password2
            // 
            tb_password2.BackColor = Color.White;
            tb_password2.BorderStyle = BorderStyle.FixedSingle;
            tb_password2.Font = new Font("Segoe UI", 13F);
            tb_password2.Location = new Point(220, 298);
            tb_password2.Name = "tb_password2";
            tb_password2.PasswordChar = '●';
            tb_password2.PlaceholderText = "Nhập lại mật khẩu";
            tb_password2.Size = new Size(400, 36);
            tb_password2.TabIndex = 3;
            // 
            // tb_email
            // 
            tb_email.BackColor = Color.White;
            tb_email.BorderStyle = BorderStyle.FixedSingle;
            tb_email.Font = new Font("Segoe UI", 13F);
            tb_email.Location = new Point(220, 173);
            tb_email.Name = "tb_email";
            tb_email.PlaceholderText = "Email";
            tb_email.Size = new Size(400, 36);
            tb_email.TabIndex = 4;
            // 
            // btn_singup
            // 
            btn_singup.BackColor = Color.MediumSeaGreen;
            btn_singup.FlatAppearance.BorderSize = 0;
            btn_singup.FlatStyle = FlatStyle.Flat;
            btn_singup.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btn_singup.ForeColor = Color.Black;
            btn_singup.Location = new Point(183, 363);
            btn_singup.Name = "btn_singup";
            btn_singup.Size = new Size(180, 45);
            btn_singup.TabIndex = 5;
            btn_singup.Text = "ĐĂNG KÝ";
            btn_singup.UseVisualStyleBackColor = false;
            btn_singup.Click += btn_singup_Click;
            // 
            // btn_login
            // 
            btn_login.BackColor = Color.Goldenrod;
            btn_login.FlatAppearance.BorderSize = 0;
            btn_login.FlatStyle = FlatStyle.Flat;
            btn_login.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btn_login.ForeColor = Color.Black;
            btn_login.Location = new Point(448, 363);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(242, 45);
            btn_login.TabIndex = 6;
            btn_login.Text = "ĐÃ CÓ TÀI KHOẢN";
            btn_login.UseVisualStyleBackColor = false;
            btn_login.Click += btn_login_Click;
            // 
            // Signup
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background2;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(800, 420);
            Controls.Add(label1);
            Controls.Add(tb_login);
            Controls.Add(tb_password);
            Controls.Add(tb_password2);
            Controls.Add(tb_email);
            Controls.Add(btn_singup);
            Controls.Add(btn_login);
            Font = new Font("Segoe UI", 12F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Signup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng ký";
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_login;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.TextBox tb_password2;
        private System.Windows.Forms.TextBox tb_email;
        private System.Windows.Forms.Button btn_singup;
        private System.Windows.Forms.Button btn_login;
    }
}
