namespace MonopolyWinForms.Room
{
    partial class Waiting_Room_Host
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Waiting_Room_Host));
            txb_RoomName = new TextBox();
            txb_player1 = new TextBox();
            txb_player2 = new TextBox();
            txb_player3 = new TextBox();
            txb_player4 = new TextBox();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            btn_Out = new Button();
            btn_Play = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            SuspendLayout();
            // 
            // txb_RoomName
            // 
            txb_RoomName.BackColor = SystemColors.Menu;
            txb_RoomName.BorderStyle = BorderStyle.None;
            txb_RoomName.Enabled = false;
            txb_RoomName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            txb_RoomName.Location = new Point(183, 19);
            txb_RoomName.Name = "txb_RoomName";
            txb_RoomName.ReadOnly = true;
            txb_RoomName.Size = new Size(286, 36);
            txb_RoomName.TabIndex = 0;
            txb_RoomName.TabStop = false;
            // 
            // txb_player1
            // 
            txb_player1.BackColor = Color.FromArgb(250, 250, 250);
            txb_player1.BorderStyle = BorderStyle.FixedSingle;
            txb_player1.Enabled = false;
            txb_player1.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            txb_player1.ForeColor = Color.Black;
            txb_player1.Location = new Point(20, 170);
            txb_player1.Name = "txb_player1";
            txb_player1.ReadOnly = true;
            txb_player1.Size = new Size(200, 32);
            txb_player1.TabIndex = 1;
            txb_player1.TabStop = false;
            // 
            // txb_player2
            // 
            txb_player2.BackColor = Color.FromArgb(250, 250, 250);
            txb_player2.BorderStyle = BorderStyle.FixedSingle;
            txb_player2.Enabled = false;
            txb_player2.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            txb_player2.Location = new Point(320, 170);
            txb_player2.Name = "txb_player2";
            txb_player2.ReadOnly = true;
            txb_player2.Size = new Size(200, 32);
            txb_player2.TabIndex = 2;
            txb_player2.TabStop = false;
            // 
            // txb_player3
            // 
            txb_player3.BackColor = Color.FromArgb(250, 250, 250);
            txb_player3.BorderStyle = BorderStyle.FixedSingle;
            txb_player3.Enabled = false;
            txb_player3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            txb_player3.Location = new Point(620, 170);
            txb_player3.Name = "txb_player3";
            txb_player3.ReadOnly = true;
            txb_player3.Size = new Size(200, 32);
            txb_player3.TabIndex = 3;
            txb_player3.TabStop = false;
            // 
            // txb_player4
            // 
            txb_player4.BackColor = Color.FromArgb(250, 250, 250);
            txb_player4.BorderStyle = BorderStyle.FixedSingle;
            txb_player4.Enabled = false;
            txb_player4.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            txb_player4.Location = new Point(920, 170);
            txb_player4.Name = "txb_player4";
            txb_player4.ReadOnly = true;
            txb_player4.Size = new Size(200, 32);
            txb_player4.TabIndex = 4;
            txb_player4.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.player1;
            pictureBox1.Location = new Point(20, 220);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(200, 300);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.player2;
            pictureBox2.Location = new Point(320, 220);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(200, 300);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.player3;
            pictureBox3.Location = new Point(620, 220);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(200, 300);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 7;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = Properties.Resources.player4;
            pictureBox4.Location = new Point(920, 220);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(200, 300);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 8;
            pictureBox4.TabStop = false;
            // 
            // btn_Out
            // 
            btn_Out.BackColor = Color.FromArgb(255, 105, 97); // #FF6961 (Coral Soft Red)
            btn_Out.FlatStyle = FlatStyle.Flat;
            btn_Out.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn_Out.ForeColor = Color.Black;
            btn_Out.Location = new Point(160, 640);
            btn_Out.Name = "btn_Out";
            btn_Out.Size = new Size(200, 50);
            btn_Out.TabIndex = 9;
            btn_Out.Text = "Thoát";
            btn_Out.UseVisualStyleBackColor = false;
            btn_Out.Click += btn_Out_Click;
            // 
            // btn_Play
            // 
            btn_Play.BackColor = Color.FromArgb(144, 238, 144); // #90EE90 (Light Green)
            btn_Play.FlatStyle = FlatStyle.Flat;
            btn_Play.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn_Play.ForeColor = Color.Black;
            btn_Play.Location = new Point(760, 640);
            btn_Play.Name = "btn_Play";
            btn_Play.Size = new Size(200, 50);
            btn_Play.TabIndex = 10;
            btn_Play.Text = "Chơi";
            btn_Play.UseVisualStyleBackColor = false;
            btn_Play.Click += btn_Play_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label1.Location = new Point(20, 18);
            label1.Name = "label1";
            label1.Size = new Size(143, 37);
            label1.TabIndex = 11;
            label1.Text = "ID phòng:";
            // 
            // Waiting_Room_Host
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(251, 248, 244);
            ClientSize = new Size(1136, 753);
            Controls.Add(label1);
            Controls.Add(btn_Play);
            Controls.Add(btn_Out);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(txb_player4);
            Controls.Add(txb_player3);
            Controls.Add(txb_player2);
            Controls.Add(txb_player1);
            Controls.Add(txb_RoomName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Waiting_Room_Host";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Phòng chờ";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txb_RoomName;
        private TextBox txb_player1;
        private TextBox txb_player2;
        private TextBox txb_player3;
        private TextBox txb_player4;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private Button btn_Out;
        private Button btn_Play;
        private Label label1;
    }
}