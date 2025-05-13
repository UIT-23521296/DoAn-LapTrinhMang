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

namespace MonopolyWinForms.BuyLand_Home
{
    public partial class BuyCompany : Form
    {
        private int playerID;
        private Tile tile;
        public BuyCompany(int playerID, Tile tile)
        {
            InitializeComponent();
            this.playerID = playerID;
            this.tile = tile;
        }
    }
}
