using MonopolyWinForms.GameLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonopolyWinForms.Play_area
{
    public partial class Draw_playarea: Form
    {
        private Board Board;
        public Draw_playarea()
        {
            InitializeComponent();
            string path = Path.Combine(Application.StartupPath, "Assets", "Tiles.txt");
            Board = new Board(path);
            Board.GenerateBoard(this, Board.TileList);  // Hiển thị bàn cờ lên form
        }
    }
}
