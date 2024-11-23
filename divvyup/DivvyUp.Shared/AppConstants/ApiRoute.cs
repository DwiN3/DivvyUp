namespace DivvyUp_Shared.AppConstants
{
    public class ApiRoute
    {
        private const string BaseUrl = "/api";

        public const string ARG_PERSON = "{personId}";
        public const string ARG_RECEIPT = "{receiptId}";
        public const string ARG_PRODUCT = "{productId}";
        public const string ARG_PERSON_PRODUCT = "{personProductId}";
        public const string ARG_LOAN = "{loanId}";
        public const string ARG_SETTLED = "{settled}";
        public const string ARG_YEAR = "{year}";
        public const string ARG_FROM = "{from}";
        public const string ARG_TO = "{to}";
        public const string ARG_LENT = "{lent}";
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

        public static class PERSON_ROUTES
        {
            public const string PERSON_ROUTE = $"{BaseUrl}/person";
            public const string ADD = $"{PERSON_ROUTE}/add";
            public const string EDIT = $"{PERSON_ROUTE}/edit/{ARG_PERSON}";
            public const string REMOVE = $"{PERSON_ROUTE}/remove/{ARG_PERSON}";
            public const string PERSON = $"{PERSON_ROUTE}/{ARG_PERSON}";
            public const string PEOPLE = $"{PERSON_ROUTE}/people";
            public const string PERSON_USER = $"{PERSON_ROUTE}/user-person";
            public const string PEOPLE_FROM_RECEIPT = $"{PERSON_ROUTE}/from-receipt/{ARG_RECEIPT}";
            public const string PEOPLE_FROM_PRODUCT = $"{PERSON_ROUTE}/from-product/{ARG_PRODUCT}";
        }

        public static class LOAN_ROUTES
        {
            public const string LOAN_ROUTE = $"{BaseUrl}/loan";
            public const string ADD = $"{LOAN_ROUTE}/add";
            public const string EDIT = $"{LOAN_ROUTE}/edit/{ARG_LOAN}";
            public const string REMOVE = $"{LOAN_ROUTE}/remove/{ARG_LOAN}";
            public const string SET_PERSON = $"{LOAN_ROUTE}/set-person/{ARG_LOAN}/{ARG_PERSON}";
            public const string SET_SETTLED = $"{LOAN_ROUTE}/set-settled/{ARG_LOAN}/{ARG_SETTLED}";
            public const string SET_LENT = $"{LOAN_ROUTE}/set-lent/{ARG_LOAN}/{ARG_LENT}";
            public const string LOAN = $"{LOAN_ROUTE}/{ARG_LOAN}";
            public const string LOANS_PERSON = $"{LOAN_ROUTE}/person-loans/{ARG_PERSON}";
            public const string LOANS = $"{LOAN_ROUTE}/loans";
            public const string LOANS_DATA_RANGE = $"{LOAN_ROUTE}/date-range/{ARG_FROM}/{ARG_TO}";
        }

        public static class RECEIPT_ROUTES
        {
            public const string RECEIPT_ROUTE = $"{BaseUrl}/receipt";
            public const string ADD = $"{RECEIPT_ROUTE}/add";
            public const string EDIT = $"{RECEIPT_ROUTE}/edit/{ARG_RECEIPT}";
            public const string REMOVE = $"{RECEIPT_ROUTE}/remove/{ARG_RECEIPT}";
            public const string SET_SETTLED = $"{RECEIPT_ROUTE}/set-settled/{ARG_RECEIPT}/{ARG_SETTLED}";
            public const string RECEIPT = $"{RECEIPT_ROUTE}/{ARG_RECEIPT}";
            public const string RECEIPTS = $"{RECEIPT_ROUTE}/receipts";
            public const string RECEIPTS_DATA_RANGE = $"{RECEIPT_ROUTE}/date-range";
        }

        public static class PRODUCT_ROUTES
        {
            public const string PRODUCT_ROUTE = $"{BaseUrl}/product";
            public const string ADD = $"{PRODUCT_ROUTE}/{ARG_RECEIPT}/add";
            public const string EDIT = $"{PRODUCT_ROUTE}/edit/{ARG_PRODUCT}";
            public const string ADD_WIDTH_PERSON = $"{PRODUCT_ROUTE}/{ARG_RECEIPT}/add/{ARG_PERSON}";
            public const string ADD_WIDTH_PERSONS = $"{PRODUCT_ROUTE}/{ARG_RECEIPT}/add/persons";
            public const string EDIT_WIDTH_PERSON = $"{PRODUCT_ROUTE}/{ARG_PRODUCT}/edit/{ARG_PERSON}";
            public const string REMOVE = $"{PRODUCT_ROUTE}/remove/{ARG_PRODUCT}";
            public const string SET_SETTLED = $"{PRODUCT_ROUTE}/set-settled/{ARG_PRODUCT}/{ARG_SETTLED}";
            public const string PRODUCT = $"{PRODUCT_ROUTE}/{ARG_PRODUCT}";
            public const string PRODUCTS = $"{PRODUCT_ROUTE}";
            public const string PRODUCTS_FROM_RECEIPT = $"{PRODUCT_ROUTE}/products-from-receipt/{ARG_RECEIPT}";
        }

        public static class PERSON_PRODUCT_ROUTES
        {
            public const string PERSON_PRODUCT_ROUTE = $"{BaseUrl}/person-product";
            public const string ADD = $"{PERSON_PRODUCT_ROUTE}/{ARG_PRODUCT}/add";
            public const string EDIT = $"{PERSON_PRODUCT_ROUTE}/edit/{ARG_PERSON_PRODUCT}";
            public const string REMOVE = $"{PERSON_PRODUCT_ROUTE}/remove/{ARG_PERSON_PRODUCT}";
            public const string REMOVE_LIST = $"{PERSON_PRODUCT_ROUTE}/remove/list/{ARG_PRODUCT}";
            public const string SET_PERSON = $"{PERSON_PRODUCT_ROUTE}/set-person/{ARG_PERSON_PRODUCT}/{ARG_PERSON}";
            public const string SET_SETTLED = $"{PERSON_PRODUCT_ROUTE}/set-settled/{ARG_PERSON_PRODUCT}/{ARG_SETTLED}";
            public const string SET_COMPENSATION = $"{PERSON_PRODUCT_ROUTE}/set-compensation/{ARG_PERSON_PRODUCT}";
            public const string PERSON_PRODUCT = $"{PERSON_PRODUCT_ROUTE}/{ARG_PERSON_PRODUCT}";
            public const string PERSON_PRODUCTS = $"{PERSON_PRODUCT_ROUTE}/person-products";
            public const string PERSON_PRODUCT_FROM_PERSON = $"{PERSON_PRODUCT_ROUTE}/person/{ARG_PERSON}";
            public const string PERSON_PRODUCT_FROM_PRODUCT= $"{PERSON_PRODUCT_ROUTE}/product/{ARG_PRODUCT}";
        }

        public static class CHART_ROUTES
        {

            public const string CHART_ROUTE = $"{BaseUrl}/chart";
            public const string TOTAL_AMOUNTS = $"{CHART_ROUTE}/total-amounts";
            public const string UNPAID_AMOUNTS = $"{CHART_ROUTE}/unpaid-amounts";
            public const string PERCENTAGE_EXPENSES = $"{CHART_ROUTE}/percentage-expenses";
            public const string MONTHLY_TOTAL_EXPANSES = $"{CHART_ROUTE}/monthly-total-expenses/{ARG_YEAR}";
            public const string MONTHLY_USER_EXPANSES = $"{CHART_ROUTE}/monthly-user-expenses/{ARG_YEAR}";
            public const string WEEKLY_TOTAL_EXPENSES = $"{CHART_ROUTE}/weekly-total-expenses";
            public const string WEEKLY_USER_EXPENSES = $"{CHART_ROUTE}/weekly-user-expenses";
            public const string MONTHLY_TOP_PRODUCTS = $"{CHART_ROUTE}/monthly-top-products";
        }
    }
}
