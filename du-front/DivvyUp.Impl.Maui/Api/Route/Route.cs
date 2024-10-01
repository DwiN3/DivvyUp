namespace DivvyUp_Impl_Maui.Api.Route
{
    public class Route
    {
        public const string ID = "{id}";

        /// Auth
        public readonly string Login = $"http://localhost:8080/rm/auth";
        public readonly string Register = $"http://localhost:8080/rm/register";
        public readonly string Remove = $"http://localhost:8080/rm/remove-account";
        public readonly string IsValid = $"http://localhost:8080/rm/validate-token";

        // Person
        public readonly string AddPerson = $"http://localhost:8080/rm/person/add";
        public readonly string EditPerson = $"http://localhost:8080/rm/person/edit/{ID}";
        public readonly string RemovePerson = $"http://localhost:8080/rm/person/remove/{ID}";
        public readonly string SetReceiptsCountsPerson = $"http://localhost:8080/rm/person/set-receipts-counts/{ID}";
        public readonly string SetTotalPurchaseAmountPerson = $"http://localhost:8080/rm/person/set-total-purchase-amount/{ID}";
        public readonly string ShowPerson = $"http://localhost:8080/rm/person/show/{ID}";
        public readonly string ShowPersons = $"http://localhost:8080/rm/person/show-all";

        // Receipt
        public readonly string AddReceipt = $"http://localhost:8080/rm/receipt/add";
        public readonly string EditReceipt = $"http://localhost:8080/rm/receipt/edit/{ID}";
        public readonly string RemoveReceipt = $"http://localhost:8080/rm/receipt/remove/{ID}";
        public readonly string SetSettledReceipt = $"http://localhost:8080/rm/receipt/set-is-settled/{ID}";
        public readonly string ShowReceipts = $"http://localhost:8080/rm/receipt/show-all";

        // Product
        public readonly string AddProduct = $"http://localhost:8080/rm/receipt/{ID}/product/add";
        public readonly string EditProduct = $"http://localhost:8080/rm/receipt/product/edit/{ID}";
        public readonly string RemoveProduct = $"http://localhost:8080/rm/product/remove/{ID}";
        public readonly string SetSettledProduct = $"http://localhost:8080/rm/receipt/set-is-settled/{ID}";
        public readonly string ShowProduct = $"http://localhost:8080/rm/product/show/{ID}";
        public readonly string ShowProducts = $"http://localhost:8080/rm/receipt/{ID}/product/show-all";

        // Person Product
        public readonly string AddPersonProduct = $"http://localhost:8080/rm/product/{ID}/person-product/add";
        public readonly string RemovePersonProduct = $"http://localhost:8080/rm/person-product/remove/{ID}";
        public readonly string SetSettledPersonProduct = $"http://localhost:8080/rm/receipt/set-is-settled/{ID}";
        public readonly string SetCompensationPersonProduct = $"http://localhost:8080/rm/receipt/set-is-settled/{ID}";
        public readonly string ShowPersonProduct = $"http://localhost:8080/rm/person-product/show/{ID}";
        public readonly string ShowPersonProducts = $"http://localhost:8080/rm/product/{ID}/person-product/show-all";
    }
}
