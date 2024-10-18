namespace DivvyUp_Shared.AppConstants
{
    public class ApiRoute
    {
        private const string BaseUrl = "http://localhost:8080/rm";

        // Arguments
        public const string arg_ID = "{id}";
        public const string arg_Settled = "{settled}";
        public const string arg_ReceiptsCount = "{receiptsCount}";
        public const string arg_TotalAmount = "{totalAmount}";
        public const string arg_TotalPrice = "{totalPrice}";
        public const string arg_CompensationPrice = "{compensationPrice}";
        public const string arg_PersonId = "{personId}";

        // User
        public readonly string Login = $"{BaseUrl}/auth";
        public readonly string Register = $"{BaseUrl}/register";
        public readonly string EditUser = $"{BaseUrl}/user/edit";
        public readonly string ChangePasswordUser = $"{BaseUrl}/user/change-password";
        public readonly string RemoveUser = $"{BaseUrl}/user/remove";
        public readonly string GetUser = $"{BaseUrl}/user";
        public readonly string IsValid = $"{BaseUrl}/validate-token";

        // Person
        public readonly string AddPerson = $"{BaseUrl}/person/add";
        public readonly string EditPerson = $"{BaseUrl}/person/{arg_ID}/edit";
        public readonly string RemovePerson = $"{BaseUrl}/person/{arg_ID}/remove";
        public readonly string SetReceiptsCountsPerson = $"{BaseUrl}/person/{arg_ID}/set-receipts-counts?receiptsCount={arg_ReceiptsCount}";
        public readonly string SetTotalAmountPerson = $"{BaseUrl}/person/{arg_ID}/set-total-purchase-amount?totalAmount={arg_TotalAmount}";
        public readonly string GetPerson = $"{BaseUrl}/person/{arg_ID}";
        public readonly string GetPersons = $"{BaseUrl}/person";
        public readonly string GetUserPerson = $"{BaseUrl}/person/user-person";
        public readonly string GetPersonsFromReceipt = $"{BaseUrl}/person/{arg_ID}/from-receipt";
        public readonly string GetPersonsFromProduct = $"{BaseUrl}/person/{arg_ID}/from-product";

        // Receipt
        public readonly string AddReceipt = $"{BaseUrl}/receipt/add";
        public readonly string EditReceipt = $"{BaseUrl}/receipt/{arg_ID}/edit";
        public readonly string RemoveReceipt = $"{BaseUrl}/receipt/{arg_ID}/remove";
        public readonly string SetSettledReceipt = $"{BaseUrl}/receipt/{arg_ID}/set-settled?settled={arg_Settled}";
        public readonly string SetTotalPriceReceipt = $"{BaseUrl}/receipt/{arg_ID}/set-total-price?totalPrice={arg_TotalPrice}";
        public readonly string GetReceipt = $"{BaseUrl}/receipt/{arg_ID}";
        public readonly string GetReceipts = $"{BaseUrl}/receipt";

        // Product
        public readonly string AddProduct = $"{BaseUrl}/receipt/{arg_ID}/product/add";
        public readonly string EditProduct = $"{BaseUrl}/product/{arg_ID}/edit";
        public readonly string RemoveProduct = $"{BaseUrl}/product/{arg_ID}/remove";
        public readonly string SetSettledProduct = $"{BaseUrl}/product/{arg_ID}/set-settled?settled={arg_Settled}";
        public readonly string SetCompensationPriceProduct = $"{BaseUrl}/product/{arg_ID}/set-compensation-price?compensationPrice={arg_CompensationPrice}";
        public readonly string GetProduct = $"{BaseUrl}/product/{arg_ID}";
        public readonly string GetProducts = $"{BaseUrl}/receipt/{arg_ID}/product";

        // Person Product
        public readonly string AddPersonProduct = $"{BaseUrl}/product/{arg_ID}/person-product/add";
        public readonly string EditPersonProduct = $"{BaseUrl}/person-product/{arg_ID}/edit";
        public readonly string RemovePersonProduct = $"{BaseUrl}/person-product/{arg_ID}/remove";
        public readonly string SetPersonPersonProduct = $"{BaseUrl}/person-product/{arg_ID}/set-person?personId={arg_PersonId}";
        public readonly string SetSettledPersonProduct = $"{BaseUrl}/person-product/{arg_ID}/set-settled?settled={arg_Settled}";
        public readonly string SetCompensationPersonProduct = $"{BaseUrl}/person-product/{arg_ID}/set-compensation";
        public readonly string GetPersonProduct = $"{BaseUrl}/person-product/{arg_ID}";
        public readonly string GetPersonProductsForProduct = $"{BaseUrl}/product/{arg_ID}/person-product";
        public readonly string GetPersonProducts= $"{BaseUrl}/person-product";

        // Chart
        public readonly string GetTotalAmountsChart = $"{BaseUrl}/chart/total-amounts";
        public readonly string GetUnpaidAmountsChart = $"{BaseUrl}/chart/unpaid-amounts";
        public readonly string GetPercentageExpensesChart = $"{BaseUrl}/chart/percentage-expenses";
    }
}
