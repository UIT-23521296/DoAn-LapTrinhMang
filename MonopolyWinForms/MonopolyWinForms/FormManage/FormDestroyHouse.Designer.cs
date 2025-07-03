namespace MonopolyWinForms.FormManage
{
    partial class FormDestroyHouse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDestroyHouse));
            listBoxTiles = new ListBox();
            btnDestroy = new Button();
            SuspendLayout();
            // 
            // listBoxTiles
            // 
            listBoxTiles.FormattingEnabled = true;
            listBoxTiles.Location = new Point(31, 31);
            listBoxTiles.Margin = new Padding(2);
            listBoxTiles.Name = "listBoxTiles";
            listBoxTiles.Size = new Size(329, 144);
            listBoxTiles.TabIndex = 0;
            // 
            // btnDestroy
            // 
            btnDestroy.BackColor = Color.IndianRed;
            btnDestroy.FlatStyle = FlatStyle.Flat;
            btnDestroy.Location = new Point(31, 189);
            btnDestroy.Margin = new Padding(2);
            btnDestroy.Name = "btnDestroy";
            btnDestroy.Size = new Size(328, 40);
            btnDestroy.TabIndex = 1;
            btnDestroy.Text = ".";
            btnDestroy.UseVisualStyleBackColor = true;
            // 
            // FormDestroyHouse
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 235, 221);
            ClientSize = new Size(422, 250);
            Controls.Add(btnDestroy);
            Controls.Add(listBoxTiles);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            Name = "FormDestroyHouse";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Phá huỷ nhà";
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBoxTiles;
        private Button btnDestroy;
    }
}