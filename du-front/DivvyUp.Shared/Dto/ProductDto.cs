using DivvyUp_Shared.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DivvyUp_Shared.Dto
{
    public class ProductDto
    {
        public int id { get; set; }
        public int receiptId { get; set; }
        public string productName { get; set; }
        public double price { get; set; }
        public double compensationAmount { get; set; }
        public bool divisible { get; set; }
        public bool settled { get; set; }
        public int maxQuantity { get; set; }
        public double packagePrice { get; set; }

        public ProductDto()
        {
            id = 0;
            receiptId = 0;
            productName = string.Empty;
            compensationAmount = 0;
            divisible = false;
            price = 0;
            settled = false;
            maxQuantity = 1;
            packagePrice = 0;
        }

        public ProductDto(int id, int receiptId, string productName, double price, double compensationAmount, bool divisible, bool settled, int maxQuantity, double packagePrice)
        {
            this.id = id;
            this.receiptId = receiptId;
            this.productName = productName;
            this.price = price;
            this.compensationAmount = compensationAmount;
            this.divisible = divisible;
            this.settled = settled;
            this.maxQuantity = maxQuantity;
            this.packagePrice = packagePrice;
        }
    }
}
