namespace DivvyUp_Shared.Dtos.Entity
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal AdditionalPrice { get; set; }
        public decimal CompensationPrice { get; set; }
        public bool Divisible { get; set; }
        public bool Settled { get; set; }
        public int MaxQuantity { get; set; }
        public int AvailableQuantity { get; set; }

        public decimal TotalPrice => Price + AdditionalPrice;
        public bool isNew => Id == 0;

        public List<PersonDto> Persons { get; set; }

        public ProductDto()
        {
            Id = 0;
            ReceiptId = 0;
            Name = string.Empty;
            CompensationPrice = 0;
            Divisible = false;
            Price = 0;
            AdditionalPrice = 0;
            Settled = false;
            MaxQuantity = 1;
            AvailableQuantity = 1;
        }

        public ProductDto(int id, int receiptId, string name, decimal price, decimal additionalPrice, decimal compensationPrice, bool divisible, bool settled, int maxQuantity, int availableQuantity)
        {
            Id = id;
            ReceiptId = receiptId;
            Name = name;
            Price = price;
            AdditionalPrice = additionalPrice;
            CompensationPrice = compensationPrice;
            Divisible = divisible;
            Settled = settled;
            MaxQuantity = maxQuantity;
            AvailableQuantity = availableQuantity;
        }
    }
}
