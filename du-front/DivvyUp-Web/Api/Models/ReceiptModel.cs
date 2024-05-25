namespace DivvyUp_Web.Api.Models
{
    public class ReceiptModel
    {
        public int receiptId;
        public UserModel user;
        public string receiptName;
        public DateTime date;
        public double totalAmount;
        public bool isSettled;
    }
}
