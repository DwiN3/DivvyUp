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
        public readonly string EditPerson = $"{BaseUrl}/person/{ID}/edit";
        public readonly string RemovePerson = $"{BaseUrl}/person/{ID}/remove";
        public readonly string SetReceiptsCountsPerson = $"{BaseUrl}/person/{ID}/set-receipts-counts";
        public readonly string SetTotalPurchaseAmountPerson = $"{BaseUrl}/person/{ID}/set-total-purchase-amount";
        public readonly string GetPerson = $"{BaseUrl}/person/{ID}";
        public readonly string GetPersons = $"{BaseUrl}/person";
        public readonly string GetPersonsFromReceipt = $"{BaseUrl}/person/{ID}/from-receipt";
        public readonly string GetPersonsFromProduct = $"{BaseUrl}/person/{ID}/from-product";

        // Receipt
        public readonly string AddReceipt = $"{BaseUrl}/receipt/add";
        public readonly string EditReceipt = $"{BaseUrl}/receipt/edit/{ID}";
        public readonly string RemoveReceipt = $"{BaseUrl}/receipt/remove/{ID}";
        public readonly string SetSettledReceipt = $"{BaseUrl}/receipt/set-settled/{ID}";
        public readonly string SetTotalPriceReceipt = $"{BaseUrl}/receipt/set-total-price/{ID}";
        public readonly string GetReceipt = $"{BaseUrl}/receipt/{ID}";
        public readonly string GetReceipts = $"{BaseUrl}/receipt";

        // Product
        public readonly string AddProduct = $"{BaseUrl}/receipt/{ID}/product/add";
        public readonly string EditProduct = $"{BaseUrl}/product/edit/{ID}";
        public readonly string RemoveProduct = $"{BaseUrl}/product/remove/{ID}";
        public readonly string SetSettledProduct = $"{BaseUrl}/product/set-settled/{ID}";
        public readonly string SetCompensationPriceProduct = $"{BaseUrl}/product/set-compensation-price/{ID}";
        public readonly string GetProduct = $"{BaseUrl}/product/{ID}";
        public readonly string GetProducts = $"{BaseUrl}/receipt/{ID}/product";

        // Person Product
        public readonly string AddPersonProduct = $"{BaseUrl}/product/{ID}/person-product/add";
        public readonly string RemovePersonProduct = $"{BaseUrl}/person-product/remove/{ID}";
        public readonly string SetChangePersonPersonProduct = $"{BaseUrl}/person-product/change-person/{ID}";
        public readonly string SetSettledPersonProduct = $"{BaseUrl}/person-product/set-settled/{ID}";
        public readonly string SetCompensationPersonProduct = $"{BaseUrl}/person-product/set-compensation/{ID}";
        public readonly string GetPersonProduct = $"{BaseUrl}/person-product/{ID}";
        public readonly string GetPersonProductsForProduct = $"{BaseUrl}/product/{ID}/person-product";
        public readonly string GetPersonProducts= $"{BaseUrl}/person-product";
    }
}
