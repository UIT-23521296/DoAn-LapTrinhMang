using MonopolyWinForms.GameLogic;
using MonopolyWinForms.Services;
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
        private List<Tile> tiles;
        private List<Player> players;
        private int currentPlayerIndex;
        private Monopoly monopoly;
        private MainForm mainform;
        public BuyBus(int playerID, Tile tile, Monopoly monopoly, MainForm mainform, List<Tile> tiles, List<Player> players, int currentPlayerIndex)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.playerID = playerID;
            this.tile = tile;
            this.monopoly = monopoly;
            this.mainform = mainform;
            UpdateRentDisplay();
            this.tiles = tiles;
            this.players = players;
            this.currentPlayerIndex = currentPlayerIndex;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (tile.PlayerId == null)
            {
                tile.PlayerId = playerID;
                tile.Level = 1;
                UpdateRentDisplay();
                mainform.UpdateBusStationRent(playerID);
                var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
                await GameManager.UpdateGameState(gameState);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private async void UpdateRentDisplay()
        {
            int playerBuses = monopoly.CountBusesOwned(playerID);
            int Price = tile.LandPrice;
            int rent = 50 + 50 * playerBuses;
            label2.Text = $"Rent rate: ${rent}";
            label3.Text = $"The price: ${Price}";
            var gameState = new GameState(GameManager.CurrentRoomId, currentPlayerIndex, players, tiles);
            await GameManager.UpdateGameState(gameState);
        }
    }
}
