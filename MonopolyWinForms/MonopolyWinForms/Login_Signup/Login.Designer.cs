namespace MonopolyWinForms.Login_Signup
{
    partial class Login
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
            label1 = new Label();
            label2 = new Label();
            tb_password = new TextBox();
            tb_login = new TextBox();
            btn_login = new Button();
            btn_signup = new Button();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(302, 52);
            label1.Name = "label1";
            label1.Size = new Size(227, 46);
            label1.TabIndex = 0;
            label1.Text = "ĐĂNG NHẬP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Cursor = Cursors.Hand;
            label2.Font = new Font("Segoe UI", 9F);
            label2.Location = new Point(215, 246);
            label2.Name = "label2";
            label2.Size = new Size(109, 20);
            label2.TabIndex = 1;
            label2.Text = "Quên mật khẩu";
            label2.Click += label2_Click;
            // 
            // tb_password
            // 
            tb_password.BorderStyle = BorderStyle.FixedSingle;
            tb_password.Font = new Font("Segoe UI", 13F);
            tb_password.Location = new Point(215, 194);
            tb_password.Name = "tb_password";
            tb_password.Size = new Size(379, 36);
            tb_password.TabIndex = 2;
            tb_password.TextChanged += tb_password_TextChanged;
            // 
            // tb_login
            // 
            tb_login.BorderStyle = BorderStyle.FixedSingle;
            tb_login.Font = new Font("Segoe UI", 13F);
            tb_login.Location = new Point(215, 126);
            tb_login.Name = "tb_login";
            tb_login.Size = new Size(379, 36);
            tb_login.TabIndex = 3;
            // 
            // btn_login
            // 
            btn_login.Font = new Font("Segoe UI", 13F);
            btn_login.Location = new Point(128, 294);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(196, 44);
            btn_login.TabIndex = 4;
            btn_login.Text = "ĐĂNG NHẬP";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += btn_login_Click;
            // 
            // btn_signup
            // 
            btn_signup.Font = new Font("Segoe UI", 13F);
            btn_signup.Location = new Point(447, 294);
            btn_signup.Name = "btn_signup";
            btn_signup.Size = new Size(194, 44);
            btn_signup.TabIndex = 5;
            btn_signup.Text = "ĐĂNG KÝ";
            btn_signup.UseVisualStyleBackColor = true;
            btn_signup.Click += btn_signup_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13F);
            label3.Location = new Point(128, 126);
            label3.Name = "label3";
            label3.Size = new Size(64, 30);
            label3.TabIndex = 6;
            label3.Text = "Email";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 13F);
            label4.Location = new Point(96, 194);
            label4.Name = "label4";
            label4.Size = new Size(103, 30);
            label4.TabIndex = 7;
            label4.Text = "Mật khẩu";
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(btn_signup);
            Controls.Add(btn_login);
            Controls.Add(tb_login);
            Controls.Add(tb_password);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
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