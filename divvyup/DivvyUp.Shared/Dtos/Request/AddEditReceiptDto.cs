namespace DivvyUp_Shared.Dtos.Request
{
    public class AddEditReceiptDto
    {
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public int DiscountPercentage { get; set; }

        public AddEditReceiptDto()
        {
            Name = string.Empty;
            Date = DateOnly.FromDateTime(DateTime.Now);
            DiscountPercentage = 0;
        }

        public AddEditReceiptDto(string name, DateOnly date, int discountPercentage)
        {
            Name = name;
            Date = date;
            DiscountPercentage = discountPercentage;    
        }
    }
}