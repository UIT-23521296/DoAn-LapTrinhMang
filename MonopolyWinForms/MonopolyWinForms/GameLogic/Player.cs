using System;
using System.Drawing;  // nhớ thêm namespace này nếu dùng Color, Image
using System.IO;
using System.Windows.Forms;

namespace MonopolyWinForms.GameLogic
{
    public class Player
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Money { get; set; }
        public int OutPrison { get; set; }
        public int ReduceHalfMoney { get; set; }
        public int DoubleMoney { get; set; }
        public int DoubleDices { get; set; }
        public int TileIndex { get; set; }
        public bool IsBankrupt { get; set; }
        public Color Color { get; set; }
        public Player(int id, int initialMoney)
        {
            ID = id;
            Money = initialMoney;
            OutPrison = 0;
            ReduceHalfMoney = 0;
            DoubleMoney = 0;
            DoubleDices = 0;
            TileIndex = 1;
            Color = GetDefaultColor(id);
        }
        public void AddMoney(int amount)
        {
            if (DoubleMoney > 0){
                Money += amount * 2;
                DoubleMoney--;
            }else{
                Money += amount;
            }
        }
        public void SubtractMoney(int amount)
        {
            int finalAmount = amount;
            if (ReduceHalfMoney > 0){
                finalAmount /= 2;
                ReduceHalfMoney--;
            }
            if (Money >= finalAmount)
                Money -= finalAmount;
            else
                Money = 0;
        }
        public void AddOutPrisonCard() => OutPrison++;
        public void AddReduceHalfCard() => ReduceHalfMoney++;
        public void AddDoubleMoneyCard() => DoubleMoney++;
        public void RolledDoubleDice() => DoubleDices++;
        public void ResetDoubleDice() => DoubleDices = 0;
        public bool MustGoToJailByDouble() => DoubleDices >= 3;
        public Image GetAvatar()
        {
            string imagePath = Path.Combine(Application.StartupPath, "Assets", "Images", $"player{ID}.png");
            return Image.FromFile(imagePath);
        }
        private Color GetDefaultColor(int id)
        {
            return id switch
            {
                1 => Color.Blue,
                2 => Color.Yellow,
                3 => Color.Red,
                4 => Color.Green,
                _ => Color.Gray
            };
        }
        public void DeclareBankruptcy()
        {
            IsBankrupt = true;
            // Có thể thêm logic khác khi phá sản
        }
    }
}