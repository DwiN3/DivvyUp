namespace DivvyUp_Impl_Maui.Api.Route
{
    public class Route
    {
        public const string ID = "{id}";
        private const string BaseUrl = "http://localhost:8080/rm";
        public const string Settled = "{settled}";

        /// Auth
        public readonly string Login = $"{BaseUrl}/auth";
        public readonly string Register = $"{BaseUrl}/register";
        public readonly string EditUser = $"{BaseUrl}/user/edit";
        public readonly string ChangePasswordUser = $"{BaseUrl}/user/change-password";
        public readonly string RemoveUser = $"{BaseUrl}/user/remove";
        public readonly string GetUser = $"{BaseUrl}/user";
        public readonly string IsValid = $"{BaseUrl}/validate-token";

        // Person
        public const string ReceiptsCount = "{receiptsCount}";
        public const string TotalAmount = "{totalAmount}";
        public readonly string AddPerson = $"{BaseUrl}/person/add";
        public readonly string EditPerson = $"{BaseUrl}/person/{ID}/edit";
        public readonly string RemovePerson = $"{BaseUrl}/person/{ID}/remove";
        public readonly string SetReceiptsCountsPerson = $"{BaseUrl}/person/{ID}/set-receipts-counts?receiptsCount={ReceiptsCount}";
        public readonly string SetTotalAmountPerson = $"{BaseUrl}/person/{ID}/set-total-purchase-amount?totalAmount={TotalAmount}";
        public readonly string GetPerson = $"{BaseUrl}/person/{ID}";
        public readonly string GetPersons = $"{BaseUrl}/person";
        public readonly string GetPersonsFromReceipt = $"{BaseUrl}/person/{ID}/from-receipt";
        public readonly string GetPersonsFromProduct = $"{BaseUrl}/person/{ID}/from-product";

        // Receipt
        public const string TotalPrice = "{totalPrice}";
        public readonly string AddReceipt = $"{BaseUrl}/receipt/add";
        public readonly string EditReceipt = $"{BaseUrl}/receipt/{ID}/edit";
        public readonly string RemoveReceipt = $"{BaseUrl}/receipt/{ID}/remove";
        public readonly string SetSettledReceipt = $"{BaseUrl}/receipt/{ID}/set-settled?settled={Settled}";
        public readonly string SetTotalPriceReceipt = $"{BaseUrl}/receipt/{ID}/set-total-price?totalPrice={TotalPrice}";
        public readonly string GetReceipt = $"{BaseUrl}/receipt/{ID}";
        public readonly string GetReceipts = $"{BaseUrl}/receipt";

        // Product
        public const string CompensationPrice = "{compensationPrice}";
        public readonly string AddProduct = $"{BaseUrl}/receipt/{ID}/product/add";
        public readonly string EditProduct = $"{BaseUrl}/product/{ID}/edit";
        public readonly string RemoveProduct = $"{BaseUrl}/product/{ID}/remove";
        public readonly string SetSettledProduct = $"{BaseUrl}/product/{ID}/set-settled?settled={Settled}";
        public readonly string SetCompensationPriceProduct = $"{BaseUrl}/product/{ID}/set-compensation-price?compensationPrice={CompensationPrice}";
        public readonly string GetProduct = $"{BaseUrl}/product/{ID}";
        public readonly string GetProducts = $"{BaseUrl}/receipt/{ID}/product";

        // Person Product
        public const string PersonId = "{personId}";
        public const string Compensation = "{compensation}";
        public readonly string AddPersonProduct = $"{BaseUrl}/product/{ID}/person-product/add";
        public readonly string RemovePersonProduct = $"{BaseUrl}/person-product/{ID}/remove";
        public readonly string ChangePersonPersonProduct = $"{BaseUrl}/person-product/{ID}/change-person?personId={PersonId}";
        public readonly string SetSettledPersonProduct = $"{BaseUrl}/person-product/{ID}/set-settled?settled={Settled}";
        public readonly string SetCompensationPersonProduct = $"{BaseUrl}/person-product/{ID}/set-compensation?compensation={Compensation}";
        public readonly string GetPersonProduct = $"{BaseUrl}/person-product/{ID}";
        public readonly string GetPersonProductsForProduct = $"{BaseUrl}/product/{ID}/person-product";
        public readonly string GetPersonProducts= $"{BaseUrl}/person-product";
    }
}
