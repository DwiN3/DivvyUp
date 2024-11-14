namespace DivvyUp_Shared.Dto
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public bool Lent { get; set; }
        public bool Settled { get; set; }
        public PersonDto Person { get; set; }

        public LoanDto()
        {
            Id = 0;
            PersonId = 0;
            Date = DateOnly.FromDateTime(DateTime.Now);
            Amount = 0;
            Lent = false;
            Settled = false;
        }

        public LoanDto(int id, int personId, DateOnly date, decimal amount, bool lent, bool settled)
        {
            Id = id;
            PersonId = personId;
            Date = date;
            Amount = amount;
            Lent = lent;
            Settled = settled;
        }
    }
}
