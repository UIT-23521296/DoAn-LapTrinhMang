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
            listBoxTiles = new ListBox();
            btnDestroy = new Button();
            SuspendLayout();
            // 
            // listBoxTiles
            // 
            listBoxTiles.FormattingEnabled = true;
            listBoxTiles.ItemHeight = 25;
            listBoxTiles.Location = new Point(39, 39);
            listBoxTiles.Name = "listBoxTiles";
            listBoxTiles.Size = new Size(410, 179);
            listBoxTiles.TabIndex = 0;
            // 
            // btnDestroy
            // 
            btnDestroy.Location = new Point(39, 236);
            btnDestroy.Name = "btnDestroy";
            btnDestroy.Size = new Size(410, 50);
            btnDestroy.TabIndex = 1;
            btnDestroy.Text = ".";
            btnDestroy.UseVisualStyleBackColor = true;
            // 
            // FormDestroyHouse
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(528, 313);
            Controls.Add(btnDestroy);
            Controls.Add(listBoxTiles);
            Name = "FormDestroyHouse";
            Text = "FormDestroyHouse";
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBoxTiles;
        private Button btnDestroy;
    }
}