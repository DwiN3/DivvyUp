namespace DivvyUp_Web.Api.Urls
{
    public class Url
    {
        /// Auth
        public readonly string Login = "http://localhost:8080/rm/auth";
        public readonly string Register = "http://localhost:8080/rm/register";
        public readonly string Remove = "http://localhost:8080/rm/remove-account";

        // Receipt
        public readonly string AddReceipt = "http://localhost:8080/rm/receipt/add";
        public readonly string SetSettled = "http://localhost:8080/rm/receipt/set-is-settled/";
        public readonly string ReceiptRemove = "http://localhost:8080/rm/receipt/remove/";
        public readonly string ShowAll = "http://localhost:8080/rm/receipt/show-all";
    }
}
