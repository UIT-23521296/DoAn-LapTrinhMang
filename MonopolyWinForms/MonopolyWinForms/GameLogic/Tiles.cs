using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyWinForms.GameLogic
{
    public class Tile
    {
        public int TileId { get; set; }              // ID ô đất
        public string Name { get; set; }             // Tên ô (ví dụ: "Park Lane")
        public int? OwnerId { get; set; }            // ID người sở hữu (null nếu chưa ai mua)
        public int Level { get; set; }               // 0: chưa mua, 1: có đất, 2: 1 nhà, 3: 2 nhà..., 4: hotel
        public int BasePrice { get; set; }           // Giá mua cơ bản
        public int HousePrice { get; set; }          // Giá mỗi nhà
        public int HotelPrice { get; set; }          // Giá nâng cấp lên hotel
        public double Rent { get; set; }                // Giá thuê hiện tại (tính theo level)
        public int Monopoly { get; set; }

        public Tile(int tileId, string name, int basePrice, int housePrice, int hotelPrice, int monopoly)
        {
            TileId = tileId;
            Name = name;
            BasePrice = basePrice;
            HousePrice = housePrice;
            HotelPrice = hotelPrice;
            OwnerId = null;
            Level = 0;
            Monopoly = monopoly;
            Rent = CalculateRent();
            Monopoly = monopoly;
        }

        // Tính giá thuê dựa trên cấp độ của ô đất
        public double CalculateRent()
        {
            if (Level == 0) return 0;  // Nếu chưa có nhà, không có tiền thuê
            if (Level == 1) return BasePrice * 0.1;  // Thuê với mức giá 1
            if (Level <= 3) return (BasePrice + (HousePrice * Level)) / 2;  // Tính tiền thuê khi có nhà
            if (Level == 4) return (BasePrice + (HousePrice * 3) + HotelPrice) / 2;  // Tiền thuê khi có khách sạn
            return 0;
        }

        // Mua ô đất nếu ô đất chưa có chủ
        public bool Purchase(int playerId, int playerMoney)
        {
            if (OwnerId == null && playerMoney >= BasePrice)  // Nếu ô đất chưa có chủ và người chơi đủ tiền
            {
                OwnerId = playerId;  // Gán chủ sở hữu cho người chơi
                return true;  // Mua thành công
            }
            return false;  // Không thể mua (hoặc ô đất đã có chủ hoặc không đủ tiền)
        }

        // Bán ô đất hoặc khi người chơi phá sản
        public void ResetTile()
        {
            OwnerId = null;  // Đặt lại chủ sở hữu về null
            Level = 0;  // Đặt lại cấp độ về 0 (chưa mua)
            Rent = CalculateRent();  // Tính lại tiền thuê (sẽ là 0 vì cấp độ là 0)
        }

        // Thanh toán tiền thuê cho ô đất
        public bool PayRent(int playerMoney)
        {
            if (OwnerId != null && Rent > 0) // Nếu có chủ và có tiền thuê
            {
                if (playerMoney >= Rent)
                {
                    return true; // Trả tiền thuê thành công
                }
                else
                {
                    return false; // Không đủ tiền để trả thuê
                }
            }
            return false;
        }

        // In thông tin của ô đất
        public void PrintTileInfo()
        {
            Console.WriteLine($"Tile: {Name}, Owner: {OwnerId}, Level: {Level}, Rent: {Rent}");
        }
    }
}
