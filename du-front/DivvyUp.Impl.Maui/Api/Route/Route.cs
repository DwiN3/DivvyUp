﻿namespace DivvyUp_Impl_Maui.Api.Route
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
        public readonly string ReceiptRemove = $"http://localhost:8080/rm/receipt/remove/{ID}";
        public readonly string SetSettledReceipt = $"http://localhost:8080/rm/receipt/set-is-settled/{ID}";
        public readonly string ShowReceipts = $"http://localhost:8080/rm/receipt/show-all";

        // Person
        public readonly string AddPerson = $"http://localhost:8080/rm/person/add";
        public readonly string EditPerson = $"http://localhost:8080/rm/person/edit/{ID}";
        public readonly string RemovePerson = $"http://localhost:8080/rm/person/remove/{ID}";
        public readonly string SetReceiptsCountsPerson = $"http://localhost:8080/rm/person/set-receipts-counts/{ID}";
        public readonly string SetTotalPurchaseAmountPerson = $"http://localhost:8080/rm/person/set-total-purchase-amount/{ID}";
        public readonly string ShowPerson = $"http://localhost:8080/rm/person/show/{ID}";
        public readonly string ShowPersons = $"http://localhost:8080/rm/person/show-all";
    }
}
