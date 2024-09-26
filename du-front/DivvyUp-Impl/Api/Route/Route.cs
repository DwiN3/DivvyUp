namespace DivvyUp_Impl.Api.Route
{
    public class Route
    {
        public const string ID = "{id}";

        /// Auth
        public readonly string Login = $"http://localhost:8080/rm/auth";
        public readonly string Register = $"http://localhost:8080/rm/register";
        public readonly string Remove = $"http://localhost:8080/rm/remove-account";
        public readonly string IsValid = $"http://localhost:8080/rm/validate-token";

        // Receipt
        public readonly string AddReceipt = $"http://localhost:8080/rm/receipt/add";
        public readonly string EditReceipt = $"http://localhost:8080/rm/receipt/edit/{ID}";
        public readonly string SetSettled = $"http://localhost:8080/rm/receipt/set-is-settled/{ID}";
        public readonly string ReceiptRemove = $"http://localhost:8080/rm/receipt/remove/{ID}";
        public readonly string ShowAll = $"http://localhost:8080/rm/receipt/show-all";
    }
}
