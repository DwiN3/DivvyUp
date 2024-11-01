namespace DivvyUp_Shared.RequestDto
{
    public class AddEditProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Divisible { get; set; }
        public int MaxQuantity { get; set; }
    }
}
