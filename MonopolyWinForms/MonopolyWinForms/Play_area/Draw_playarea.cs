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
            Board = new Board("D:\\UIT\\lap trinh mang\\doan\\DoAn-LapTrinhMang\\MonopolyWinForms\\MonopolyWinForms\\Play_area\\Tiles.txt");  // Đảm bảo file Tiles.txt nằm trong thư mục đúng
            Board.GenerateBoard(this, Board.TileList);  // Hiển thị bàn cờ lên form
        }
    }
}
