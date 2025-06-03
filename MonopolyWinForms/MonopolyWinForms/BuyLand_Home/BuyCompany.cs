using MonopolyWinForms.GameLogic;
using MonopolyWinForms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonopolyWinForms.BuyLand_Home
{
    public partial class BuyCompany : Form
    {
        private int playerID;
        private Tile tile;
        private List<Tile> tiles;
        private List<Player> players;
        private int currentPlayerIndex;
        private Monopoly monopoly;
        private MainForm mainform;
        public BuyCompany(int playerID, Tile tile, Monopoly monopoly, MainForm mainform, List<Tile> tiles, List<Player> players, int currentPlayerIndex)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.playerID = playerID;
            this.tile = tile;
            this.monopoly = monopoly;
            this.mainform = mainform;
            this.tiles = tiles;
            this.players = players;
            this.currentPlayerIndex = currentPlayerIndex;
            LoadCompanyImage();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadCompanyImage()
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images");

            if (tile.Name == "Công ty Điện lực")
            {
                string imagePath = Path.Combine(basePath, "cty_dien.png");
                if (File.Exists(imagePath))
                    pictureBox1.Image = Image.FromFile(imagePath);
                label1.TextAlign = ContentAlignment.MiddleCenter;
                label1.Text = $"Công ty Điện lực";
            }
            else if (tile.Name == "Công ty Cấp nước")
            {
                string imagePath = Path.Combine(basePath, "cty_nuoc.png");
                if (File.Exists(imagePath))
                    pictureBox1.Image = Image.FromFile(imagePath);
                label1.TextAlign = ContentAlignment.MiddleCenter;
                label1.Text = $"Công ty Cấp nước";
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            UpdateRentDisplay();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (tile.PlayerId == null)
            {
                tile.PlayerId = playerID;
                tile.Level = 1;

                //Cập nhật UI
                UpdateRentDisplay();
                mainform.UpdateCompanyRent(playerID);

                //Cập nhật game state 
                var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                await GameManager.UpdateGameState(gameState);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private async void UpdateRentDisplay()
        {
            int playerCompanies = monopoly.CountCompaniesOwned(playerID);
            int Price = tile.LandPrice;
            int rent = 25; // mặc định là 25
            if (playerCompanies == 1)
            {
                rent = 25 * 4;
            }
            else if (playerCompanies >= 2)
            {
                rent = 25 * 10;
            }
            label2.Text = $"Rent rate: ${rent} * {"number of dices"}";
            label3.Text = $"The price: ${Price}";
            var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
            await GameManager.UpdateGameState(gameState);
        }
    }
}
