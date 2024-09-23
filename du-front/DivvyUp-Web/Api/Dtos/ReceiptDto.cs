namespace DivvyUp_Web.Api.Dtos
{
    public class ReceiptDto
    {
        public int receiptId;
        public int userId;
        public string receiptName;
        public DateTime date;
        public double totalAmount;
        public bool settled;
    }
}
