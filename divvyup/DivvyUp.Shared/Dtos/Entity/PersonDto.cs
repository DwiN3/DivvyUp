namespace DivvyUp_Shared.Dtos.Entity
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int ReceiptsCount { get; set; }
        public int ProductsCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public decimal LoanBalance { get; set; }
        public bool UserAccount { get; set; }

        public string FullName => $"{Name} {Surname}";

        public PersonDto()
        {
            Id = 0;
            Name = string.Empty;
            Surname = string.Empty;
            ReceiptsCount = 0;
            ProductsCount = 0;
            TotalAmount = 0;
            UnpaidAmount = 0;
            LoanBalance = 0;
            UserAccount = false;
        }

        public PersonDto(int id, string name, string surname, int receiptsCount, int productsCount, decimal totalAmount, decimal unpaidAmount, decimal loanBalance, bool userAccount)
        {
            Id = id;
            Name = name;
            Surname = surname;
            ReceiptsCount = receiptsCount;
            ProductsCount = productsCount;
            TotalAmount = totalAmount;
            UnpaidAmount = unpaidAmount;
            LoanBalance = loanBalance;
            UserAccount = userAccount;
        }
    }
}
