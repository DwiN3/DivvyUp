namespace DivvyUp_Web.Api.Models
{
    public class Receipt
    {
        public int receiptId;
        public User user;
        public string receiptName;
        private DateTime date;
        private double totalAmount;
        private bool isSettled;
    }
}
