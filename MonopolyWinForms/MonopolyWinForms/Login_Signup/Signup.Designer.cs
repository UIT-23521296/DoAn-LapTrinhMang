namespace MonopolyWinForms.Login_Signup
{
    partial class Signup
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
            label4 = new Label();
            label3 = new Label();
            btn_login = new Button();
            btn_singup = new Button();
            tb_login = new TextBox();
            tb_password = new TextBox();
            label1 = new Label();
            label2 = new Label();
            tb_password2 = new TextBox();
            label5 = new Label();
            tb_email = new TextBox();
            SuspendLayout();
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 13F);
            label4.Location = new Point(113, 156);
            label4.Name = "label4";
            label4.Size = new Size(103, 30);
            label4.TabIndex = 15;
            label4.Text = "Mật khẩu";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13F);
            label3.Location = new Point(60, 93);
            label3.Name = "label3";
            label3.Size = new Size(156, 30);
            label3.TabIndex = 14;
            label3.Text = "Tên đăng nhập";
            // 
            // btn_login
            // 
            btn_login.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btn_login.Font = new Font("Segoe UI", 13F);
            btn_login.Location = new Point(434, 333);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(260, 44);
            btn_login.TabIndex = 13;
            btn_login.Text = "ĐÃ CÓ TÀI KHOẢN";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += btn_login_Click;
            // 
            // btn_singup
            // 
            btn_singup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btn_singup.Font = new Font("Segoe UI", 13F);
            btn_singup.Location = new Point(133, 333);
            btn_singup.Name = "btn_singup";
            btn_singup.Size = new Size(260, 44);
            btn_singup.TabIndex = 12;
            btn_singup.Text = "ĐĂNG KÝ";
            btn_singup.UseVisualStyleBackColor = true;
            btn_singup.Click += btn_singup_Click;
            // 
            // tb_login
            // 
            tb_login.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tb_login.BorderStyle = BorderStyle.FixedSingle;
            tb_login.Font = new Font("Segoe UI", 13F);
            tb_login.Location = new Point(231, 93);
            tb_login.Name = "tb_login";
            tb_login.Size = new Size(379, 36);
            tb_login.TabIndex = 11;
            // 
            // tb_password
            // 
            tb_password.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tb_password.BorderStyle = BorderStyle.FixedSingle;
            tb_password.Font = new Font("Segoe UI", 13F);
            tb_password.Location = new Point(231, 156);
            tb_password.Name = "tb_password";
            tb_password.Size = new Size(379, 36);
            tb_password.TabIndex = 10;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(342, 44);
            label1.Name = "label1";
            label1.Size = new Size(172, 46);
            label1.TabIndex = 8;
            label1.Text = "ĐĂNG KÝ";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13F);
            label2.Location = new Point(29, 211);
            label2.Name = "label2";
            label2.Size = new Size(187, 30);
            label2.TabIndex = 17;
            label2.Text = "Nhập lại mật khẩu";
            // 
            // tb_password2
            // 
            tb_password2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tb_password2.BorderStyle = BorderStyle.FixedSingle;
            tb_password2.Font = new Font("Segoe UI", 13F);
            tb_password2.Location = new Point(231, 211);
            tb_password2.Name = "tb_password2";
            tb_password2.Size = new Size(379, 36);
            tb_password2.TabIndex = 16;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 13F);
            label5.Location = new Point(152, 267);
            label5.Name = "label5";
            label5.Size = new Size(64, 30);
            label5.TabIndex = 19;
            label5.Text = "Email";
            // 
            // tb_email
            // 
            tb_email.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tb_email.BorderStyle = BorderStyle.FixedSingle;
            tb_email.Font = new Font("Segoe UI", 13F);
            tb_email.Location = new Point(231, 267);
            tb_email.Name = "tb_email";
            tb_email.Size = new Size(379, 36);
            tb_email.TabIndex = 18;
            // 
            // Signup
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label5);
            Controls.Add(tb_email);
            Controls.Add(label2);
            Controls.Add(tb_password2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(btn_login);
            Controls.Add(btn_singup);
            Controls.Add(tb_login);
            Controls.Add(tb_password);
            Controls.Add(label1);
            Name = "Signup";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Signup";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label4;
        private Label label3;
        private Button btn_login;
        private Button btn_singup;
        private TextBox tb_login;
        private TextBox tb_password;
        private Label label1;
        private Label label2;
        private TextBox tb_password2;
        private Label label5;
        private TextBox tb_email;
    }
}