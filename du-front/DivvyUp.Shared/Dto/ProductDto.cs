using DivvyUp_Shared.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DivvyUp_Shared.Dto
{
    public class ProductDto
    {
        public int id { get; set; }
        public int receiptId { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public double compensationPrice { get; set; }
        public bool divisible { get; set; }
        public bool settled { get; set; }
        public int maxQuantity { get; set; }
        public List<PersonDto> persons { get; set; } 

        public ProductDto()
        {
            id = 0;
            receiptId = 0;
            name = string.Empty;
            compensationPrice = 0;
            divisible = false;
            price = 0;
            settled = false;
            maxQuantity = 1;
        }

        public ProductDto(int id, int receiptId, string name, double price, double compensationPrice, bool divisible, bool settled, int maxQuantity, double packagePrice)
        {
            this.id = id;
            this.receiptId = receiptId;
            this.name = name;
            this.price = price;
            this.compensationPrice = compensationPrice;
            this.divisible = divisible;
            this.settled = settled;
            this.maxQuantity = maxQuantity;
        }
    }
}
