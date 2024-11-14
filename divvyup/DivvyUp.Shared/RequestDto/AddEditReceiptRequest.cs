namespace DivvyUp_Shared.RequestDto
{
    public class AddEditReceiptRequest
    {
        public string Name { get; set; }
        public DateOnly Date { get; set; }

        public AddEditReceiptRequest()
        {
            Name = string.Empty;
            Date = DateOnly.FromDateTime(DateTime.Now);
        }

        public AddEditReceiptRequest(string name, DateOnly date)
        {
            Name = name;
            Date = date;
        }
    }
}