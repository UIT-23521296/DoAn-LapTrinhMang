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
    public partial class BuyBus : Form
    {
        private int playerID;
        private Tile tile;
        private Monopoly monopoly;

        //Thêm cơ chế kiểm tra có đủ tiền không

        public BuyBus(int playerID, Tile tile, List<Tile> allTiles)
        {
            InitializeComponent();
            this.playerID = playerID;
            this.tile = tile;
            this.monopoly = new Monopoly(allTiles);  // Khởi tạo Monopoly với danh sách tất cả các ô đất

            // Cập nhật giá thuê khi mở form
            UpdateRentDisplay();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tile.PlayerId == null)
            {
                tile.PlayerId = playerID; // Gán người chơi hiện tại làm chủ
                tile.Level = 1;           // Đặt level = 1 sau khi mua

                // Cập nhật giá thuê sau khi mua
                UpdateRentDisplay();

                this.Close(); // Đóng form sau khi mua
            }
        }

        // Phương thức cập nhật giá thuê
        private void UpdateRentDisplay()
        {
            int playerBuses = monopoly.CountBusesOwned(playerID);

            int Price = tile.LandPrice;
            int rent = 50 * playerBuses;

            label2.Text = $"Giá thuê: ${rent}";
            label3.Text = $"The price: ${Price}";
        }
    }
}
