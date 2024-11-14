namespace DivvyUp_Shared.Dtos.Request
{
    public class AddEditLoanDto
    {
        public int PersonId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public bool Lent { get; set; }

        public AddEditLoanDto()
        {
            PersonId = 0;
            Date = DateOnly.FromDateTime(DateTime.Now);
            Amount = 0;
            Lent = false;
        }

        public AddEditLoanDto(int personId, DateOnly date, decimal amount, bool lent)
        {
            PersonId = personId;
            Date = date;
            Amount = amount;
            Lent = lent;
        }
    }
}