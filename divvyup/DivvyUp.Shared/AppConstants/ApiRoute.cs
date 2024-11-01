namespace DivvyUp_Shared.AppConstants
{
    public class ApiRoute
    {
        private const string BaseUrl = "http://localhost:5185/rm";

        // Arguments
        public const string arg_ID = "{id}";
        public const string arg_Settled = "{settled}";
        public const string arg_PersonId = "{personId}";
        public const string arg_Year = "{year}";
        public const string arg_From = "{from}";
        public const string arg_To = "{to}";
        public const string arg_Lent = "{lent}";

        // User
        public readonly string Login = $"{BaseUrl}/user/auth";
        public readonly string Register = $"{BaseUrl}/user/register";
        public readonly string EditUser = $"{BaseUrl}/user/edit";
        public readonly string ChangePasswordUser = $"{BaseUrl}/user/change-password";
        public readonly string RemoveUser = $"{BaseUrl}/user/remove";
        public readonly string GetUser = $"{BaseUrl}/user/get-user";
        public readonly string IsValid = $"{BaseUrl}/user/validate-token";

        // Person
        public readonly string AddPerson = $"{BaseUrl}/person/add";
        public readonly string EditPerson = $"{BaseUrl}/person/edit/{arg_ID}";
        public readonly string RemovePerson = $"{BaseUrl}/person/remove/{arg_ID}";
        public readonly string GetPerson = $"{BaseUrl}/person/get/{arg_ID}";
        public readonly string GetPersons = $"{BaseUrl}/person/get/persons";
        public readonly string GetUserPerson = $"{BaseUrl}/person/get/user-person";
        public readonly string GetPersonsFromReceipt = $"{BaseUrl}/person/{arg_ID}/from-receipt";
        public readonly string GetPersonsFromProduct = $"{BaseUrl}/person/{arg_ID}/from-product";

        // Loan
        public readonly string AddLoan = $"{BaseUrl}/loan/add";
        public readonly string EditLoan = $"{BaseUrl}/loan/{arg_ID}/edit";
        public readonly string RemoveLoan = $"{BaseUrl}/loan/{arg_ID}/remove";
        public readonly string SetPersonLoan = $"{BaseUrl}/loan/{arg_ID}/set-person?personId={arg_PersonId}";
        public readonly string SetSettledLoan = $"{BaseUrl}/loan/{arg_ID}/set-settled?settled={arg_Settled}";
        public readonly string SetLentLoan = $"{BaseUrl}/loan/{arg_ID}/set-lent?lent={arg_Lent}";
        public readonly string GetLoan = $"{BaseUrl}/loan/{arg_ID}";
        public readonly string GetLoanPerson = $"{BaseUrl}/loan/person/{arg_ID}";
        public readonly string GetLoans = $"{BaseUrl}/loan";
        public readonly string GetLoansByDataRange = $"{BaseUrl}/loan/date-range?from={arg_From}&to={arg_To}";

        // Receipt
        public readonly string AddReceipt = $"{BaseUrl}/receipt/add";
        public readonly string EditReceipt = $"{BaseUrl}/receipt/edit/{arg_ID}";
        public readonly string RemoveReceipt = $"{BaseUrl}/receipt/remove/{arg_ID}";
        public readonly string SetSettledReceipt = $"{BaseUrl}/receipt/set-settled/{arg_ID}?settled={arg_Settled}";
        public readonly string GetReceipt = $"{BaseUrl}/receipt/get/{arg_ID}";
        public readonly string GetReceipts = $"{BaseUrl}/receipt/get/receipts";
        public readonly string GetReceiptsByDataRange = $"{BaseUrl}/receipt/get/receipts-date-range?from={arg_From}&to={arg_To}";

        // Product
        public readonly string AddProduct = $"{BaseUrl}/receipt/{arg_ID}/product/add";
        public readonly string EditProduct = $"{BaseUrl}/product/{arg_ID}/edit";
        public readonly string RemoveProduct = $"{BaseUrl}/product/{arg_ID}/remove";
        public readonly string SetSettledProduct = $"{BaseUrl}/product/{arg_ID}/set-settled?settled={arg_Settled}";
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
        public readonly string GetMonthlyTotalExpensesChart = $"{BaseUrl}/chart/monthly-total-expenses/{arg_Year}";
        public readonly string GetMonthlyUserExpenses = $"{BaseUrl}/chart/monthly-user-expenses/{arg_Year}";
        public readonly string GetWeeklyTotalExpenses = $"{BaseUrl}/chart/weekly-total-expenses";
        public readonly string GetWeeklyUserExpenses = $"{BaseUrl}/chart/weekly-user-expenses";
        public readonly string GetMonthlyTopProducts = $"{BaseUrl}/chart/monthly-top-products";
    }
}
