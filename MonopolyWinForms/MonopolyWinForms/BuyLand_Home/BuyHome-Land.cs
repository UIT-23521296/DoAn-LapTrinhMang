using Microsoft.VisualBasic.Devices;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace buyLand_Home
{
    public partial class BuyHome_Land : Form
    {
        private bool Land;
        private bool House1;
        private bool House2;
        private bool House3;
        private bool Hotel;
        private int playerID;
        private int? ownerID;

        public BuyHome_Land(bool Land, bool House1, bool House2, bool House3, bool Hotel, int playerID, int? ownerID)
        {
            InitializeComponent();
            this.playerID = playerID;
            this.ownerID = ownerID;

            this.Land = Land;
            this.House1 = House1;
            this.House2 = House2;
            this.House3 = House3;
            this.Hotel = Hotel;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BuyLand_Home_Load(object sender, EventArgs e)
        {
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

            // Nếu chưa có chủ, cho mua Land
            if (ownerID == null)
            {
                checkBox1.Enabled = true;
                checkBox1.Checked = true;
                return;
            }

            // Nếu người chơi là chủ
            if (ownerID == playerID)
            {
                if (Land)
                {
                    checkBox1.Checked = true;

                    // Cho phép mua House1,2,3 nếu chưa có
                    checkBox2.Enabled = true;
                    checkBox3.Enabled = true;
                    checkBox4.Enabled = true;

                    if (House1) checkBox2.Checked = true;
                    if (House2) checkBox3.Checked = true;
                    if (House3) checkBox4.Checked = true;

                    // Nếu đã có House3 thì cho phép mua Hotel
                    if (House3)
                    {
                        checkBox5.Enabled = true;
                        if (Hotel) checkBox5.Checked = true;
                    }
                }
            }
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
            int totalPrice = 0;

            if (checkBox1.Checked) totalPrice += 200;  // Giá đất
            if (checkBox2.Checked) totalPrice += 100;  // Nhà cấp 1
            if (checkBox3.Checked) totalPrice += 150;  // Nhà cấp 2
            if (checkBox4.Checked) totalPrice += 200;  // Nhà cấp 3
            if (checkBox5.Checked) totalPrice += 300;  // Khách sạn

            // Cập nhật giá thuê và giá mua
            label2.Text = $"Rent rate: ${totalPrice / 2}";   // Giá thuê là một nửa giá trị của tổng giá trị
            label3.Text = $"The price: ${totalPrice}";      // Giá tiền là tổng giá trị
        }
    }
}
