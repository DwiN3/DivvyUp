namespace DivvyUp_Web.Api.Models
{
    public class Receipt
    {
        public int receiptId;
        public User user;
        public string receiptName;
        public DateTime date;
        public double totalAmount;
        public bool isSettled;
    }
}
