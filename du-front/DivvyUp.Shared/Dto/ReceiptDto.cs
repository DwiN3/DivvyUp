namespace DivvyUp_Shared.Dto
{
    public class ReceiptDto
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string receiptName { get; set; }
        public DateTime date { get; set; }
        public double totalAmount { get; set; }
        public bool settled { get; set; }

        public ReceiptDto()
        {
            id = 0;
            userId = 0;
            receiptName = string.Empty;
            date = DateTime.Now;
            totalAmount = 0;
            settled = false;
        }

        public ReceiptDto(int id, int userId, string receiptName, DateTime date, double totalAmount, bool settled)
        {
            this.id = id;
            this.userId = userId;
            this.receiptName = receiptName;
            this.date = date;
            this.totalAmount = totalAmount;
            this.settled = settled;
        }
    }
}
