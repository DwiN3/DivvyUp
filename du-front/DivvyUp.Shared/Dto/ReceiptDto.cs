namespace DivvyUp_Shared.Dto
{
    public class ReceiptDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public double totalAmount { get; set; }
        public bool settled { get; set; }

        public ReceiptDto()
        {
            id = 0;
            name = string.Empty;
            date = DateTime.Now;
            totalAmount = 0;
            settled = false;
        }

        public ReceiptDto(int id, string name, DateTime date, double totalAmount, bool settled)
        {
            this.id = id;
            this.name = name;
            this.date = date;
            this.totalAmount = totalAmount;
            this.settled = settled;
        }
    }
}
