namespace DivvyUp_Impl_Maui.Api.Route
{
    public class Route
    {
        public const string ID = "{id}";
        private const string BaseUrl = "http://localhost:8080/rm";

        /// Auth
        public readonly string Login = $"{BaseUrl}/auth";
        public readonly string Register = $"{BaseUrl}/register";
        public readonly string Remove = $"{BaseUrl}/remove-account";
        public readonly string IsValid = $"{BaseUrl}/validate-token";

        // Person
        public readonly string AddPerson = $"{BaseUrl}/person/add";
        public readonly string EditPerson = $"{BaseUrl}/person/edit/{ID}";
        public readonly string RemovePerson = $"{BaseUrl}/person/remove/{ID}";
        public readonly string SetReceiptsCountsPerson = $"{BaseUrl}/person/set-receipts-counts/{ID}";
        public readonly string SetTotalPurchaseAmountPerson = $"{BaseUrl}/person/set-total-purchase-amount/{ID}";
        public readonly string GetPerson = $"{BaseUrl}/person/{ID}";
        public readonly string GetPersons = $"{BaseUrl}/person";

        // Receipt
        public readonly string AddReceipt = $"{BaseUrl}/receipt/add";
        public readonly string EditReceipt = $"{BaseUrl}/receipt/edit/{ID}";
        public readonly string RemoveReceipt = $"{BaseUrl}/receipt/remove/{ID}";
        public readonly string SetSettledReceipt = $"{BaseUrl}/receipt/set-is-settled/{ID}";
        public readonly string GetReceipt = $"{BaseUrl}/receipt/{ID}";
        public readonly string GetReceipts = $"{BaseUrl}/receipt";

        // Product
        public readonly string AddProduct = $"{BaseUrl}/receipt/{ID}/product/add";
        public readonly string EditProduct = $"{BaseUrl}/product/edit/{ID}";
        public readonly string RemoveProduct = $"{BaseUrl}/product/remove/{ID}";
        public readonly string SetSettledProduct = $"{BaseUrl}/product/set-is-settled/{ID}";
        public readonly string GetProduct = $"{BaseUrl}/product/{ID}";
        public readonly string GetProducts = $"{BaseUrl}/receipt/{ID}/product";

        // Person Product
        public readonly string AddPersonProduct = $"{BaseUrl}/product/{ID}/person-product/add";
        public readonly string RemovePersonProduct = $"{BaseUrl}/person-product/remove/{ID}";
        public readonly string SetSettledPersonProduct = $"{BaseUrl}/receipt/set-is-settled/{ID}";
        public readonly string SetCompensationPersonProduct = $"{BaseUrl}/receipt/set-is-settled/{ID}";
        public readonly string GetPersonProduct = $"{BaseUrl}/person-product/{ID}";
        public readonly string GetPersonProducts = $"{BaseUrl}/product/{ID}/person-product";
    }
}
