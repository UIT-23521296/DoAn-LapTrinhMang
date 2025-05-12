namespace MonopolyWinForms.Login_Signup
{
    partial class Forgot
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
            label3 = new Label();
            btn_send = new Button();
            tb_email = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13F);
            label3.Location = new Point(135, 145);
            label3.Name = "label3";
            label3.Size = new Size(64, 30);
            label3.TabIndex = 14;
            label3.Text = "Email";
            // 
            // btn_send
            // 
            btn_send.Font = new Font("Segoe UI", 13F);
            btn_send.Location = new Point(307, 228);
            btn_send.Name = "btn_send";
            btn_send.Size = new Size(194, 44);
            btn_send.TabIndex = 13;
            btn_send.Text = "GỬI EMAIL";
            btn_send.UseVisualStyleBackColor = true;
            btn_send.Click += btn_send_Click;
            // 
            // tb_email
            // 
            tb_email.BorderStyle = BorderStyle.FixedSingle;
            tb_email.Font = new Font("Segoe UI", 13F);
            tb_email.Location = new Point(222, 145);
            tb_email.Name = "tb_email";
            tb_email.Size = new Size(379, 36);
            tb_email.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(259, 60);
            label1.Name = "label1";
            label1.Size = new Size(308, 46);
            label1.TabIndex = 8;
            label1.Text = "QUÊN MẬT KHẨU";
            // 
            // Forgot
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(btn_send);
            Controls.Add(tb_email);
            Controls.Add(label1);
            Name = "Forgot";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Forgot";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label3;
        private Button btn_send;
        private TextBox tb_email;
        private Label label1;
    }
}