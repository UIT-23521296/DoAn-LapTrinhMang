namespace MonopolyWinForms.BuyLand_Home
{
    partial class BuyCompany
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
            pictureBox1 = new PictureBox();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            label3 = new Label();
            panel1 = new Panel();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 16F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(979, 45);
            label1.TabIndex = 0;
            label1.Text = "Công ty";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(101, 73);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(262, 253);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 13F);
            label2.Location = new Point(273, 374);
            label2.Name = "label2";
            label2.Size = new Size(465, 45);
            label2.TabIndex = 5;
            label2.Text = "Rent rate:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 13F);
            button1.Location = new Point(114, 362);
            button1.Name = "button1";
            button1.Size = new Size(153, 61);
            button1.TabIndex = 6;
            button1.Text = "Mua";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 13F);
            button2.Location = new Point(744, 358);
            button2.Name = "button2";
            button2.Size = new Size(153, 61);
            button2.TabIndex = 7;
            button2.Text = "Bỏ qua";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13F);
            label3.Location = new Point(400, 437);
            label3.Name = "label3";
            label3.Size = new Size(127, 36);
            label3.TabIndex = 9;
            label3.Text = "The price:";
            // 
            // panel1
            // 
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(617, 73);
            panel1.Name = "panel1";
            panel1.Size = new Size(300, 253);
            panel1.TabIndex = 12;
            // 
            // label6
            // 
            label6.Location = new Point(24, 159);
            label6.Name = "label6";
            label6.Size = new Size(256, 56);
            label6.TabIndex = 13;
            label6.Text = "2 Company: $100 * number of dices";
            // 
            // label5
            // 
            label5.Location = new Point(24, 82);
            label5.Name = "label5";
            label5.Size = new Size(256, 56);
            label5.TabIndex = 1;
            label5.Text = "1 Company: $25 * number of dices";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label4.Location = new Point(24, 15);
            label4.Name = "label4";
            label4.Size = new Size(256, 56);
            label4.TabIndex = 0;
            label4.Text = "Rent depend on your number of Company";
            // 
            // BuyCompany
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1003, 531);
            Controls.Add(panel1);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "BuyCompany";
            Text = "BuyBus";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button button1;
        private Button button2;
        private Label label3;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Label label4;
        private Label label6;
        private Label label5;
    }
}