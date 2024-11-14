namespace DivvyUp_Shared.RequestDto
{
    public class AddEditProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Divisible { get; set; }
        public int MaxQuantity { get; set; }

        public AddEditProductRequest()
        {
            Name = string.Empty;
            Price = 0;
            Divisible = false;
            MaxQuantity = 1;
        }

        public AddEditProductRequest(string name, decimal price, bool divisible, int maxQuantity)
        {
            Name = name;
            Price = price;
            Divisible = divisible;
            MaxQuantity = maxQuantity;
        }
    }
}