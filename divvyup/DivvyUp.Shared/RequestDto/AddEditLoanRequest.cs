namespace DivvyUp_Shared.RequestDto
{
    public class AddEditLoanRequest
    {
        private int PersonId { get; set; }
        private DateTime Date { get; set; }
        private decimal Amount { get; set; }
        private bool Lent { get; set; }
    }
}
