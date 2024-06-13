namespace DivvyUp_Web.Api.Response
{
    public class ShowReceiptResponse
    {
        public int receiptId { get; set; }
        public string receiptName { get; set; }
        public DateTime date { get; set; }
        public double totalAmount { get; set; }
        public bool settled { get; set; }
    }
}
