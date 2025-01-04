namespace DivvyUp_Shared.Dtos.Request
{
    public class AddEditProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Divisible { get; set; }
        public int MaxQuantity { get; set; }
        public decimal AdditionalPrice { get; set; }
        public int PurchasedQuantity { get; set; }
        public int DiscountPercentage { get; set; }
        public decimal TotalPrice { get; set; }

        public AddEditProductDto()
        {
            Name = string.Empty;
            Price = 0;
            Divisible = false;
            MaxQuantity = 1;
            PurchasedQuantity = 1;
            AdditionalPrice = 0;
            DiscountPercentage = 0;
            TotalPrice = Price;
        }

        public AddEditProductDto(string name, decimal price, bool divisible, int maxQuantity, decimal additionalPrice, int purchasedQuantity, int discountPercentage, decimal totalPrice)
        {
            Name = name;
            Price = price;
            Divisible = divisible;
            MaxQuantity = maxQuantity;
            AdditionalPrice = additionalPrice;
            PurchasedQuantity = purchasedQuantity;
            DiscountPercentage = discountPercentage;
            TotalPrice = totalPrice;
        }
    }
}