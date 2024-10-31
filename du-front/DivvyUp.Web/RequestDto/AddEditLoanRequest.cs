using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DivvyUp.Web.RequestDto
{
    public class AddEditLoanRequest
    {
        private int PersonId { get; set; }
        private DateTime Date { get; set; }
        private decimal Amount { get; set; }
        private bool Lent { get; set; }
    }
}
