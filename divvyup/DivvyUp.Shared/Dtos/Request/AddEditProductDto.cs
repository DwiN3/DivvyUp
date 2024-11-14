namespace DivvyUp_Shared.Dtos.Request
{
    public class AddEditProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Divisible { get; set; }
        public int MaxQuantity { get; set; }

        public AddEditProductDto()
        {
            Name = string.Empty;
            Price = 0;
            Divisible = false;
            MaxQuantity = 1;
        }

        public AddEditProductDto(string name, decimal price, bool divisible, int maxQuantity)
        {
            Name = name;
            Price = price;
            Divisible = divisible;
            MaxQuantity = maxQuantity;
        }
    }
}