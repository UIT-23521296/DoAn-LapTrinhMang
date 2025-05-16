using MonopolyWinForms.GameLogic;
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
        private Monopoly monopoly;
        private MainForm mainForm;
        public BuyCompany(int playerID, Tile tile, Monopoly monopoly, MainForm mainForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.playerID = playerID;
            this.tile = tile;
            this.monopoly = monopoly;
            this.mainForm = mainForm;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (tile.PlayerId == null)
            {
                tile.PlayerId = playerID;
                tile.Level = 1;
                // Cập nhật giá thuê sau khi mua
                UpdateRentDisplay();
                mainForm.UpdateCompanyRent(playerID);

                this.Close();
            }
        }
        private void UpdateRentDisplay()
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
        }
    }
}
