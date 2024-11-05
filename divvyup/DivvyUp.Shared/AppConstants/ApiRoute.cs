using System.Linq.Dynamic.Core.Tokenizer;

namespace DivvyUp_Shared.AppConstants
{
    public class ApiRoute
    {
        private const string BaseUrl = "/rm";

        public const string arg_ID = "{id}";
        public const string arg_Settled = "{settled}";
        public const string arg_PersonId = "{personId}";
        public const string arg_Year = "{year}";
        public const string arg_From = "{from}";
        public const string arg_To = "{to}";
        public const string arg_Lent = "{lent}";
        public const string ARG_TOKEN = "{token}";

        public static class USER_ROUTES
        {
            public const string USER_ROUTE = $"{BaseUrl}/user";
            public const string LOGIN = $"{USER_ROUTE}/login";
            public const string REGISTER = $"{USER_ROUTE}/register";
            public const string EDIT = $"{USER_ROUTE}/edit";
            public const string REMOVE = $"{USER_ROUTE}/remove";
            public const string CHANGE_PASSWORD = $"{USER_ROUTE}/change-password";
            public const string ME = $"{USER_ROUTE}/me";
            public const string VALIDATE_TOKEN = $"{USER_ROUTE}/validate-token/{ARG_TOKEN}";
        }

        // Person
        public readonly string AddPerson = $"{BaseUrl}/person/add";
        public readonly string EditPerson = $"{BaseUrl}/person/edit/{arg_ID}";
        public readonly string RemovePerson = $"{BaseUrl}/person/remove/{arg_ID}";
        public readonly string GetPerson = $"{BaseUrl}/person/{arg_ID}";
        public readonly string GetPersons = $"{BaseUrl}/person/people";
        public readonly string GetUserPerson = $"{BaseUrl}/person/user-person";
        public readonly string GetPersonsFromReceipt = $"{BaseUrl}/person/from-receipt/{arg_ID}";
        public readonly string GetPersonsFromProduct = $"{BaseUrl}/person/from-product/{arg_ID}";

        // Loan
        public readonly string AddLoan = $"{BaseUrl}/loan/add";
        public readonly string EditLoan = $"{BaseUrl}/loan/edit/{arg_ID}";
        public readonly string RemoveLoan = $"{BaseUrl}/loan/remove/{arg_ID}";
        public readonly string SetPersonLoan = $"{BaseUrl}/loan/{arg_ID}/set-person?personId={arg_PersonId}";
        public readonly string SetSettledLoan = $"{BaseUrl}/loan/{arg_ID}/set-settled?settled={arg_Settled}";
        public readonly string SetLentLoan = $"{BaseUrl}/loan/{arg_ID}/set-lent?lent={arg_Lent}";
        public readonly string GetLoan = $"{BaseUrl}/loan/{arg_ID}";
        public readonly string GetLoanPerson = $"{BaseUrl}/loan/person-loans/{arg_PersonId}";
        public readonly string GetLoans = $"{BaseUrl}/loan/loans";
        public readonly string GetLoansByDataRange = $"{BaseUrl}/loan/date-range?from={arg_From}&to={arg_To}";

        // Receipt
        public readonly string AddReceipt = $"{BaseUrl}/receipt/add";
        public readonly string EditReceipt = $"{BaseUrl}/receipt/edit/{arg_ID}";
        public readonly string RemoveReceipt = $"{BaseUrl}/receipt/remove/{arg_ID}";
        public readonly string SetSettledReceipt = $"{BaseUrl}/receipt/{arg_ID}/settled?settled={arg_Settled}";
        public readonly string GetReceipt = $"{BaseUrl}/receipt/{arg_ID}";
        public readonly string GetReceipts = $"{BaseUrl}/receipt/receipts";
        public readonly string GetReceiptsByDataRange = $"{BaseUrl}/receipt/date-range?from={arg_From}&to={arg_To}";

        // Product
        public readonly string AddProduct = $"{BaseUrl}/product/{arg_ID}/add";
        public readonly string EditProduct = $"{BaseUrl}/product/edit/{arg_ID}";
        public readonly string RemoveProduct = $"{BaseUrl}/product/remove/{arg_ID}";
        public readonly string SetSettledProduct = $"{BaseUrl}/product/{arg_ID}/settled?settled={arg_Settled}";
        public readonly string GetProduct = $"{BaseUrl}/product/{arg_ID}";
        public readonly string GetProducts = $"{BaseUrl}/product/products-from-receipt/{arg_ID}";

        // Person Product
        public readonly string AddPersonProduct = $"{BaseUrl}/person-product/{arg_ID}/add";
        public readonly string EditPersonProduct = $"{BaseUrl}/person-product/edit/{arg_ID}";
        public readonly string RemovePersonProduct = $"{BaseUrl}/person-product/remove/{arg_ID}";
        public readonly string SetPersonPersonProduct = $"{BaseUrl}/person-product/{arg_ID}/set-person?personId={arg_PersonId}";
        public readonly string SetSettledPersonProduct = $"{BaseUrl}/person-product/{arg_ID}/settled?settled={arg_Settled}";
        public readonly string SetCompensationPersonProduct = $"{BaseUrl}/person-product/{arg_ID}/set-compensation";
        public readonly string GetPersonProduct = $"{BaseUrl}/person-product/{arg_ID}";
        public readonly string GetPersonProducts = $"{BaseUrl}/person-product/person-products";
        public readonly string GetPersonProductsForProduct = $"{BaseUrl}/person-product/product/{arg_ID}";

        // Chart
        public readonly string GetTotalAmountsChart = $"{BaseUrl}/chart/total-amounts";
        public readonly string GetUnpaidAmountsChart = $"{BaseUrl}/chart/unpaid-amounts";
        public readonly string GetPercentageExpensesChart = $"{BaseUrl}/chart/percentage-expenses";
        public readonly string GetMonthlyTotalExpensesChart = $"{BaseUrl}/chart/monthly-total-expenses?year={arg_Year}";
        public readonly string GetMonthlyUserExpenses = $"{BaseUrl}/chart/monthly-user-expenses?year={arg_Year}";
        public readonly string GetWeeklyTotalExpenses = $"{BaseUrl}/chart/weekly-total-expenses";
        public readonly string GetWeeklyUserExpenses = $"{BaseUrl}/chart/weekly-user-expenses";
        public readonly string GetMonthlyTopProducts = $"{BaseUrl}/chart/monthly-top-products";
    }
}
