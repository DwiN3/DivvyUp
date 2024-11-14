namespace DivvyUp_Shared.RequestDto
{
    public class AddEditLoanRequest
    {
        public int PersonId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public bool Lent { get; set; }
    }
}
