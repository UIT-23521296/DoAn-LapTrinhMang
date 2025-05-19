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
            btnSell = new Button();
            listBoxTiles = new ListBox();
            SuspendLayout();
            // 
            // btnSell
            // 
            btnSell.Location = new Point(39, 236);
            btnSell.Name = "btnSell";
            btnSell.Size = new Size(410, 50);
            btnSell.TabIndex = 3;
            btnSell.Text = ".";
            btnSell.UseVisualStyleBackColor = true;
            // 
            // listBoxTiles
            // 
            listBoxTiles.FormattingEnabled = true;
            listBoxTiles.ItemHeight = 25;
            listBoxTiles.Location = new Point(39, 39);
            listBoxTiles.Name = "listBoxTiles";
            listBoxTiles.Size = new Size(410, 179);
            listBoxTiles.TabIndex = 2;
            // 
            // FormSellProperty
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(528, 313);
            Controls.Add(btnSell);
            Controls.Add(listBoxTiles);
            Name = "FormSellProperty";
            Text = "FormSellProperty";
            ResumeLayout(false);
        }

        #endregion

        private Button btnSell;
        private ListBox listBoxTiles;
    }
}