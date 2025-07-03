namespace MonopolyWinForms.Room
{
    partial class Waiting_Room_Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Waiting_Room_Client));
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
            btn_Ready = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            SuspendLayout();
            // 
            // txb_RoomName
            // 
            txb_RoomName.BackColor = SystemColors.Menu;
            txb_RoomName.Location = new Point(420, 40);
            txb_RoomName.Name = "txb_RoomName";
            txb_RoomName.ReadOnly = true;
            txb_RoomName.Size = new Size(300, 27);
            txb_RoomName.TabIndex = 0;
            txb_RoomName.TabStop = false;
            // 
            // txb_player1
            // 
            txb_player1.BackColor = SystemColors.Menu;
            txb_player1.Location = new Point(20, 170);
            txb_player1.Name = "txb_player1";
            txb_player1.ReadOnly = true;
            txb_player1.Size = new Size(200, 27);
            txb_player1.TabIndex = 1;
            txb_player1.TabStop = false;
            // 
            // txb_player2
            // 
            txb_player2.BackColor = SystemColors.Menu;
            txb_player2.Location = new Point(320, 170);
            txb_player2.Name = "txb_player2";
            txb_player2.ReadOnly = true;
            txb_player2.Size = new Size(200, 27);
            txb_player2.TabIndex = 2;
            txb_player2.TabStop = false;
            // 
            // txb_player3
            // 
            txb_player3.BackColor = SystemColors.Menu;
            txb_player3.Location = new Point(620, 170);
            txb_player3.Name = "txb_player3";
            txb_player3.ReadOnly = true;
            txb_player3.Size = new Size(200, 27);
            txb_player3.TabIndex = 3;
            txb_player3.TabStop = false;
            // 
            // txb_player4
            // 
            txb_player4.BackColor = SystemColors.Menu;
            txb_player4.Location = new Point(920, 170);
            txb_player4.Name = "txb_player4";
            txb_player4.ReadOnly = true;
            txb_player4.Size = new Size(200, 27);
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
            btn_Out.Location = new Point(160, 640);
            btn_Out.Name = "btn_Out";
            btn_Out.Size = new Size(200, 50);
            btn_Out.TabIndex = 9;
            btn_Out.Text = "Thoát";
            btn_Out.UseVisualStyleBackColor = true;
            btn_Out.Click += btn_Out_Click;
            // 
            // btn_Ready
            // 
            btn_Ready.Location = new Point(760, 640);
            btn_Ready.Name = "btn_Ready";
            btn_Ready.Size = new Size(200, 50);
            btn_Ready.TabIndex = 10;
            btn_Ready.Text = "Sẵn sàng";
            btn_Ready.UseVisualStyleBackColor = true;
            btn_Ready.Click += btn_Ready_Click;
            // 
            // Waiting_Room_Client
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1136, 753);
            Controls.Add(btn_Ready);
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
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Waiting_Room_Client";
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
        private Button btn_Ready;
    }
}