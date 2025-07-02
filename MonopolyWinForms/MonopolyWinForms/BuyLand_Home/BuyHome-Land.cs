using Microsoft.VisualBasic.Devices;
using MonopolyWinForms;
using MonopolyWinForms.GameLogic;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using MonopolyWinForms.Services;

namespace buyLand_Home
{
    public partial class BuyHome_Land : Form
    {
        private Player player;
        private Tile tile;
        private List<Player> players;
        private MainForm mainform;
        private Monopoly monopoly;
        private int currentPlayerIndex;
        private List<Tile> tiles;
        public int TotalPrice { get; private set; }
        public BuyHome_Land(Player player, Tile tile, Monopoly monopoly, MainForm mainform, List<Player> players, int currentPlayerIndex, List<Tile> tiles)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.player = player;
            this.tile = tile;
            this.monopoly = monopoly;
            this.mainform = mainform;
            this.players = players;
            this.currentPlayerIndex = currentPlayerIndex;
            this.tiles = tiles;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BuyLand_Home_Load(object sender, EventArgs e)
        {
            UpdateImages();
            label1.Text = tile.Name;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;

            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            checkBox5.Enabled = false;
            // Hiển thị theo cấp độ đã có sẵn
            if (tile.Level >= 1) checkBox1.Checked = true;
            if (tile.Level >= 2) checkBox2.Checked = true;
            if (tile.Level >= 3) checkBox3.Checked = true;
            if (tile.Level >= 4) checkBox4.Checked = true;
            // Nếu chưa có chủ và level = 0 → có thể mua đất
            if (tile.PlayerId == null && tile.Level == 0)
            {
                checkBox1.Enabled = player.Money >= tile.LandPrice;
                checkBox1.Checked = checkBox1.Enabled;
            }
            // Nếu người chơi là chủ sở hữu
            if (tile.PlayerId == player.ID)
            {
                int money = player.Money;
                if (tile.Level >= 1) checkBox1.Checked = true;
                if (tile.Level >= 2) checkBox2.Checked = true;
                if (tile.Level >= 3) checkBox3.Checked = true;
                if (tile.Level >= 4) checkBox4.Checked = true;
                // Cấp 2
                if (tile.Level == 1)
                {
                    if (money >= tile.HousePrice)
                    {
                        checkBox2.Enabled = true;
                        checkBox2.Checked = true;
                        money -= tile.HousePrice;

                        if (money >= tile.HousePrice)
                        {
                            checkBox3.Enabled = true;
                            checkBox3.Checked = true;
                            money -= tile.HousePrice;

                            if (money >= tile.HousePrice)
                            {
                                checkBox4.Enabled = true;
                                checkBox4.Checked = true;
                                money -= tile.HousePrice;
                            }
                        }
                    }
                }
                else if (tile.Level == 2)
                {
                    if (money >= tile.HousePrice)
                    {
                        checkBox3.Enabled = true;
                        checkBox3.Checked = true;
                        money -= tile.HousePrice;

                        if (money >= tile.HousePrice)
                        {
                            checkBox4.Enabled = true;
                            checkBox4.Checked = true;
                            money -= tile.HousePrice;
                        }
                    }
                }
                else if (tile.Level == 3)
                {
                    if (money >= tile.HousePrice)
                    {
                        checkBox4.Enabled = true;
                        checkBox4.Checked = true;
                        money -= tile.HousePrice;
                    }
                }
                if (tile.Level == 4 && checkBox2.Checked && checkBox3.Checked && checkBox4.Checked && money >= tile.HotelPrice)
                {
                    checkBox5.Enabled = true;
                    checkBox5.Checked = true;
                }
            }
            UpdatePrice();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                UpdatePrice();
            }
            UpdatePrice();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                if (!checkBox2.Checked) checkBox2.Checked = true;
                UpdatePrice();
            }
            else
            {
                checkBox4.Checked = false;
                UpdatePrice();
            }
            UpdatePrice();
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                if (!checkBox2.Checked) checkBox2.Checked = true;
                if (!checkBox3.Checked) checkBox3.Checked = true;
                UpdatePrice();
            }
            UpdatePrice();
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }
        private void UpdatePrice()
        {
            label1.Text = tile.Name;
            int totalPrice = 0;

            if (tile == null) return;
            if (checkBox1.Checked) totalPrice += tile.LandPrice;
            if (checkBox2.Checked) totalPrice += tile.HousePrice;
            if (checkBox3.Checked) totalPrice += tile.HousePrice;
            if (checkBox4.Checked) totalPrice += tile.HousePrice;
            if (checkBox5.Checked) totalPrice += tile.HotelPrice;

            label2.Text = $"Rent rate: ${totalPrice / 2}";
            label3.Text = $"The price: ${totalPrice}";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int newLevel = 0;

            if (checkBox1.Checked) newLevel = 1;
            if (checkBox2.Checked) newLevel = 2;
            if (checkBox3.Checked) newLevel = 3;
            if (checkBox4.Checked) newLevel = 4;
            if (checkBox5.Checked) newLevel = 5;

            // Tính tổng tiền cần trả
            TotalPrice = 0;
            if (checkBox1.Checked) TotalPrice += tile.LandPrice;
            if (checkBox2.Checked) TotalPrice += tile.HousePrice;
            if (checkBox3.Checked) TotalPrice += tile.HousePrice;
            if (checkBox4.Checked) TotalPrice += tile.HousePrice;
            if (checkBox5.Checked) TotalPrice += tile.HotelPrice;

            if (newLevel > tile.Level)
            {
                tile.Level = newLevel;

                // Nếu đang mua đất (tức từ level 0 lên level 1) thì gán luôn chủ sở hữu
                if (tile.PlayerId == null && newLevel >= 1)
                {
                    tile.PlayerId = player.ID;
                }
            }
            // Bỏ phần cập nhật game state ở đây để tránh race condition
            // var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
            // await GameManager.UpdateGameState(gameState);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void UpdateImages()
        {

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;

            pictureBox1.Image = mainform.GetHouseImage(1, player, tile, players);
            pictureBox2.Image = mainform.GetHouseImage(2, player, tile, players);
            pictureBox3.Image = mainform.GetHouseImage(3, player, tile, players);
            pictureBox4.Image = mainform.GetHouseImage(4, player, tile, players);
            pictureBox5.Image = mainform.GetHouseImage(5, player, tile, players);
        }
    }
}
