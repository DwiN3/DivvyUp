namespace DivvyUp_Shared.Dto
{
    public class LoanDto
    {
        public int id { get; set; }
        public int personId { get; set; }
        public DateOnly date { get; set; }
        public double amount { get; set; }
        public bool lent { get; set; }
        public bool settled { get; set; }
        public PersonDto person { get; set; }

        public LoanDto()
        {
            id = 0;
            personId = 0;
            date = DateOnly.FromDateTime(DateTime.Now);
            amount = 0;
            lent = false;
            settled = false;
        }

        public LoanDto(int id, int personId, DateOnly date, double amount, bool lent, bool settled)
        {
            this.id = id;
            this.personId = personId;
            this.date = date;
            this.amount = amount;
            this.lent = lent;
            this.settled = settled;
        }
    }
}
