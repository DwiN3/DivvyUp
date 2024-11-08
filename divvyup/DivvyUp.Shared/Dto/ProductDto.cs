namespace DivvyUp_Shared.Dto
{
    public class ProductDto
    {
        public int id { get; set; }
        public int receiptId { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public double additionalPrice { get; set; }
        public double compensationPrice { get; set; }
        public bool divisible { get; set; }
        public bool settled { get; set; }
        public int maxQuantity { get; set; }
        public int availableQuantity { get; set; }

        public double totalPrice => price + additionalPrice;

        public List<PersonDto> persons { get; set; } 

        public ProductDto()
        {
            id = 0;
            receiptId = 0;
            name = string.Empty;
            compensationPrice = 0;
            divisible = false;
            price = 0;
            additionalPrice = 0;
            settled = false;
            maxQuantity = 1;
            availableQuantity = 1;
        }

        public ProductDto(int id, int receiptId, string name, double price, double additionalPrice, double compensationPrice, bool divisible, bool settled, int maxQuantity, int availableQuantity)
        {
            this.id = id;
            this.receiptId = receiptId;
            this.name = name;
            this.price = price;
            this.additionalPrice = additionalPrice;
            this.compensationPrice = compensationPrice;
            this.divisible = divisible;
            this.settled = settled;
            this.maxQuantity = maxQuantity;
            this.availableQuantity = availableQuantity;
        }
    }
}
