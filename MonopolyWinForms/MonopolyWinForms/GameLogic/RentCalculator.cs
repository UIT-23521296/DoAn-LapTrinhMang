using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    public class RentCalculator
    {
        private Monopoly monopoly;
        private Random random;

        public RentCalculator(Monopoly monopoly)
        {
            this.monopoly = monopoly;
            this.random = new Random();
        }

        public int CalculateRent(Tile tile, int playerId)
        {
            if (tile == null) return 0;
            if (tile.PlayerId == null || tile.PlayerId == playerId)
                return 0;

            switch (tile.Monopoly)
            {
                case "9": // Bến xe
                    int busCount = monopoly.CountBusesOwned(tile.PlayerId.Value);
                    return busCount switch
                    {
                        1 => 50,
                        2 => 100,
                        3 => 150,
                        4 => 200,
                        _ => 0
                    };

                case "10": // Công ty
                    int companyCount = monopoly.CountCompaniesOwned(tile.PlayerId.Value);
                    int dice1 = random.Next(1, 7);
                    int dice2 = random.Next(1, 7);
                    int diceValue = dice1 + dice2;

                    MessageBox.Show($"Bạn tung được: {dice1} và {dice2} (Tổng: {diceValue})", "Kết quả xúc xắc");

                    return companyCount switch
                    {
                        1 => diceValue * 20,
                        2 => diceValue * 50,
                        _ => 0
                    };

                default: // Ô đất thường
                    return tile.Level switch
                    {
                        1 => tile.LandPrice / 2,
                        2 => (tile.LandPrice + tile.HousePrice) / 2,
                        3 => (tile.LandPrice + tile.HousePrice * 2) / 2,
                        4 => (tile.LandPrice + tile.HousePrice * 3) / 2,
                        5 => (tile.LandPrice + tile.HousePrice * 3 + tile.HotelPrice) / 2,
                        _ => 0
                    };
            }
        }
    }
}
