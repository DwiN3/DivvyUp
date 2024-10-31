using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DivvyUp.Web.RequestDto
{
    public class AddEditReceipt
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
