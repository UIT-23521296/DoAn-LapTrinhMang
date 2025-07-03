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
            panel1 = new Panel();
            label2 = new Label();
            label3 = new Label();
            button1 = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Palatino Linotype", 18F, FontStyle.Bold);
            label1.ForeColor = Color.DarkSlateBlue;
            label1.Location = new Point(282, 15);
            label1.Name = "label1";
            label1.Size = new Size(177, 41);
            label1.TabIndex = 0;
            label1.Text = "🚏 BẾN XE";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.ben_xe;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(60, 70);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(220, 200);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(495, 70);
            panel1.Name = "panel1";
            panel1.Size = new Size(245, 200);
            panel1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Palatino Linotype", 12F, FontStyle.Bold);
            label2.ForeColor = Color.Teal;
            label2.Location = new Point(300, 300);
            label2.Name = "label2";
            label2.Size = new Size(158, 27);
            label2.TabIndex = 3;
            label2.Text = "📈 Rent rate: $0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Palatino Linotype", 12F, FontStyle.Bold);
            label3.ForeColor = Color.SaddleBrown;
            label3.Location = new Point(300, 335);
            label3.Name = "label3";
            label3.Size = new Size(181, 27);
            label3.TabIndex = 4;
            label3.Text = "💰 The price: $200";
            // 
            // button1
            // 
            button1.BackColor = Color.MediumSeaGreen;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(120, 310);
            button1.Name = "button1";
            button1.Size = new Size(110, 45);
            button1.TabIndex = 5;
            button1.Text = "✅ Mua";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.IndianRed;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            button2.ForeColor = Color.White;
            button2.Location = new Point(570, 310);
            button2.Name = "button2";
            button2.Size = new Size(110, 45);
            button2.TabIndex = 6;
            button2.Text = "❌ Bỏ qua";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // BuyBus
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(245, 235, 221);
            ClientSize = new Size(800, 420);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(panel1);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(button2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "BuyBus";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mua Bến Xe";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        // === helper local method INSIDE Designer.cs ===
        // Cho phép gắn thêm label động từ ngoài
        private void AddRentLabels()
        {
            Label titleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(10, 10),
                Text = "📋 Thông tin thuê:"
            };
            panel1.Controls.Add(titleLabel);

            string[] texts = { "1 Bus: $50", "2 Bus: $100", "3 Bus: $150", "4 Bus: $200" };
            for (int i = 0; i < texts.Length; i++)
            {
                Label lbl = new Label
                {
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10F),
                    Location = new Point(20, 45 + i * 30),
                    Text = texts[i]
                };
                panel1.Controls.Add(lbl);
            }
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