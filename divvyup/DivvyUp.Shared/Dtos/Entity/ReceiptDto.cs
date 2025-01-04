namespace DivvyUp_Shared.Dtos.Entity
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalPrice { get; set; }
        public int DiscountPercentage { get; set; }
        public bool Settled { get; set; }

        public ReceiptDto()
        {
            Id = 0;
            Name = string.Empty;
            Date = DateOnly.FromDateTime(DateTime.Now);
            TotalPrice = 0;
            DiscountPercentage = 0;
            Settled = false;
        }

        public ReceiptDto(int id, string name, DateOnly date, decimal totalPrice, bool settled, int discountPercentage)
        {
            Id = id;
            Name = name;
            Date = date;
            TotalPrice = totalPrice;
            DiscountPercentage = discountPercentage;
            Settled = settled;
        }
    }
}
