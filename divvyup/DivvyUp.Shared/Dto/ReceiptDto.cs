namespace DivvyUp_Shared.Dto
{
    public class ReceiptDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateOnly date { get; set; }
        public double totalPrice { get; set; }
        public bool settled { get; set; }

        public ReceiptDto()
        {
            id = 0;
            name = string.Empty;
            date = DateOnly.FromDateTime(DateTime.Now);
            totalPrice = 0;
            settled = false;
        }

        public ReceiptDto(int id, string name, DateOnly date, double totalPrice, bool settled)
        {
            this.id = id;
            this.name = name;
            this.date = date;
            this.totalPrice = totalPrice;
            this.settled = settled;
        }
    }
}
