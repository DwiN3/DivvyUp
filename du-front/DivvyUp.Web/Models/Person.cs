namespace DivvyUp.Web.Models
{
    public class Person
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int ReceiptsCount { get; set; }
        public int ProductsCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public decimal LoanBalance { get; set; }
        public bool UserAccount { get; set; }
    }
}
