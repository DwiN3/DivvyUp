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
        public readonly string SetSettledReceipt = $"{BaseUrl}/receipt/set-settled/{ID}";
        public readonly string SetTotalPriceReceipt = $"{BaseUrl}/receipt/set-total-price/{ID}";
        public readonly string GetReceipt = $"{BaseUrl}/receipt/{ID}";
        public readonly string GetReceipts = $"{BaseUrl}/receipt";

        // Item
        public readonly string AddItem = $"{BaseUrl}/receipt/{ID}/item/add";
        public readonly string EditItem = $"{BaseUrl}/item/edit/{ID}";
        public readonly string RemoveItem = $"{BaseUrl}/item/remove/{ID}";
        public readonly string SetSettledItem = $"{BaseUrl}/item/set-settled/{ID}";
        public readonly string SetCompensationPriceItem = $"{BaseUrl}/item/set-compensation-price/{ID}";
        public readonly string GetItem = $"{BaseUrl}/item/{ID}";
        public readonly string GetItems = $"{BaseUrl}/receipt/{ID}/item";

        // Person Item Share
        public readonly string AddPersonItemShare = $"{BaseUrl}/item/{ID}/person-item-share/add";
        public readonly string RemovePersonItemShare = $"{BaseUrl}/person-item-share/remove/{ID}";
        public readonly string SetSettledPersonItemShare = $"{BaseUrl}/person-item-share/set-settled/{ID}";
        public readonly string SetCompensationPersonItemShare = $"{BaseUrl}/person-item-share/set-compensation/{ID}";
        public readonly string GetPersonItemShare = $"{BaseUrl}/person-item-share/{ID}";
        public readonly string GetPersonItemsShare = $"{BaseUrl}/item/{ID}/person-item-share";
    }
}
