namespace DivvyUp_Shared.Dto
{
    public class PersonDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public int receiptsCount { get; set; }
        public int productsCount { get; set; }
        public decimal totalAmount { get; set; }
        public decimal unpaidAmount { get; set; }
        public decimal loanBalance { get; set; }
        public bool userAccount { get; set; }

        public string fullName => $"{name} {surname}";

        public PersonDto()
        {
            id = 0;
            name = string.Empty;
            surname = string.Empty;
            receiptsCount = 0;
            productsCount = 0;
            totalAmount = 0;
            unpaidAmount = 0;
            loanBalance = 0;
            userAccount = false;
        }

        public PersonDto(int id, string name, string surname, int receiptsCount, int productsCount, decimal totalAmount, decimal unpaidAmount, decimal loanBalance, bool userAccount)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.receiptsCount = receiptsCount;
            this.productsCount = productsCount;
            this.totalAmount = totalAmount;
            this.unpaidAmount = unpaidAmount;
            this.loanBalance = loanBalance;
            this.userAccount = userAccount;
        }
    }
}
