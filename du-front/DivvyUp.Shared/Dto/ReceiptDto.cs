namespace DivvyUp.Shared.Dto
{
    public class ReceiptDto
    {
        public int receiptId { get; set; }
        public int userId { get; set; }
        public string receiptName { get; set; }
        public DateTime date { get; set; }
        public double totalAmount { get; set; }
        public bool settled { get; set; }
    }
}
