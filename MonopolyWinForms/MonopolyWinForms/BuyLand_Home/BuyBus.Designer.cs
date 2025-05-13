using buyLand_Home;

namespace MonopolyWinForms.BuyLand_Home
{
    partial class BuyBus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuyBus));
            label1 = new Label();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            label3 = new Label();
            panel1 = new Panel();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F);
            label1.Location = new Point(428, 9);
            label1.Name = "label1";
            label1.Size = new Size(114, 45);
            label1.TabIndex = 0;
            label1.Text = "Bến xe";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(101, 73);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(262, 253);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13F);
            label2.Location = new Point(411, 362);
            label2.Name = "label2";
            label2.Size = new Size(161, 36);
            label2.TabIndex = 5;
            label2.Text = "Rent rate: $0";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 13F);
            button1.Location = new Point(145, 362);
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
            button2.Location = new Point(692, 362);
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
            label3.Location = new Point(398, 442);
            label3.Name = "label3";
            label3.Size = new Size(190, 36);
            label3.TabIndex = 9;
            label3.Text = "The price: $200";
            // 
            // panel1
            // 
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(617, 73);
            panel1.Name = "panel1";
            panel1.Size = new Size(300, 253);
            panel1.TabIndex = 12;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(24, 196);
            label8.Name = "label8";
            label8.Size = new Size(104, 25);
            label8.TabIndex = 13;
            label8.Text = "4 Bus: $200";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(24, 156);
            label7.Name = "label7";
            label7.Size = new Size(104, 25);
            label7.TabIndex = 13;
            label7.Text = "3 Bus: $150";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(24, 117);
            label6.Name = "label6";
            label6.Size = new Size(104, 25);
            label6.TabIndex = 13;
            label6.Text = "2 Bus: $100";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(24, 82);
            label5.Name = "label5";
            label5.Size = new Size(94, 25);
            label5.TabIndex = 1;
            label5.Text = "1 Bus: $50";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label4.Location = new Point(24, 15);
            label4.Name = "label4";
            label4.Size = new Size(256, 56);
            label4.TabIndex = 0;
            label4.Text = "Rent depend on your number of Bus";
            // 
            // BuyBus
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
            Name = "BuyBus";
            Text = "BuyBus";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
    }
}