using System;
using System.Windows.Forms;

namespace buyLand_Home
{
    public partial class BuyHome_Land : Form
    {
        private static int? ownerId = null;
        private int PlayerId;
        private static int upgradeLevel = 0;

        // Constructor mặc định để tránh lỗi khi không truyền playerId
        public BuyHome_Land() : this(0) { }

        // Constructor chính có tham số playerId
        public BuyHome_Land(int playerId)
        {
            InitializeComponent();
            PlayerId = playerId;

            checkBox1.CheckedChanged += CheckBox_CheckedChanged;
            checkBox2.CheckedChanged += CheckBox_CheckedChanged;
            checkBox3.CheckedChanged += CheckBox_CheckedChanged;
            checkBox4.CheckedChanged += CheckBox_CheckedChanged;
            checkBox5.CheckedChanged += CheckBox_CheckedChanged;

            if (ownerId == null)
            {
                EnableOnlyLandPurchase();
            }
            else if (ownerId == PlayerId)
            {
                UpdateCheckBoxStatus();
            }

            UpdatePrice();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is not CheckBox checkBox) return;

            if (ownerId == null && checkBox == checkBox1 && checkBox1.Checked)
            {
                ownerId = PlayerId;
                upgradeLevel = 1;
                EnableOnlyHousePurchase();
            }

            if (ownerId != PlayerId)
            {
                checkBox.Checked = false;
                return;
            }

            if (checkBox2.Checked && checkBox3.Checked && checkBox4.Checked)
            {
                upgradeLevel = 3;
                EnableHotelPurchase();
            }

            if (checkBox5.Checked)
            {
                upgradeLevel = 4;
            }

            UpdatePrice();
        }

        private void UpdatePrice()
        {
            int totalPrice = 0;

            if (checkBox1.Checked) totalPrice += 100;
            if (checkBox2.Checked) totalPrice += 50;
            if (checkBox3.Checked) totalPrice += 50;
            if (checkBox4.Checked) totalPrice += 50;
            if (checkBox5.Checked) totalPrice += 100;

            label2.Text = $"Rent rate: ${totalPrice / 2}";
            label3.Text = $"The price: ${totalPrice}";
        }

        private void UpdateCheckBoxStatus()
        {
            switch (upgradeLevel)
            {
                case 1:
                    EnableOnlyHousePurchase();
                    break;
                case 3:
                    EnableHotelPurchase();
                    break;
                case 4:
                    EnableAllIfOwner();
                    break;
            }
        }

        private void EnableOnlyLandPurchase()
        {
            checkBox1.Enabled = true;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            checkBox5.Enabled = false;
        }

        private void EnableOnlyHousePurchase()
        {
            checkBox1.Enabled = false;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
            checkBox4.Enabled = true;
            checkBox5.Enabled = false;
        }

        private void EnableHotelPurchase()
        {
            checkBox1.Enabled = false;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
            checkBox4.Enabled = true;
            checkBox5.Enabled = true;
        }

        private void EnableAllIfOwner()
        {
            checkBox1.Enabled = false;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;
            checkBox4.Enabled = true;
            checkBox5.Enabled = true;
        }
    }
}
