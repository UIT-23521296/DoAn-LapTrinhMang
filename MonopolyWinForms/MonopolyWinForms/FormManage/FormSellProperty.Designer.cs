namespace MonopolyWinForms.FormManage
{
    partial class FormSellProperty
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSellProperty));
            btnSell = new Button();
            listBoxTiles = new ListBox();
            SuspendLayout();
            // 
            // btnSell
            // 
            btnSell.BackColor = Color.IndianRed;
            btnSell.FlatStyle = FlatStyle.Flat;
            btnSell.Location = new Point(31, 189);
            btnSell.Margin = new Padding(2);
            btnSell.Name = "btnSell";
            btnSell.Size = new Size(328, 40);
            btnSell.TabIndex = 3;
            btnSell.Text = ".";
            btnSell.UseVisualStyleBackColor = true;
            // 
            // listBoxTiles
            // 
            listBoxTiles.FormattingEnabled = true;
            listBoxTiles.Location = new Point(31, 31);
            listBoxTiles.Margin = new Padding(2);
            listBoxTiles.Name = "listBoxTiles";
            listBoxTiles.Size = new Size(329, 144);
            listBoxTiles.TabIndex = 2;
            // 
            // FormSellProperty
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 235, 221);
            ClientSize = new Size(422, 250);
            Controls.Add(btnSell);
            Controls.Add(listBoxTiles);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSellProperty";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Bán tài sản";
            ResumeLayout(false);
        }

        #endregion

        private Button btnSell;
        private ListBox listBoxTiles;
    }
}