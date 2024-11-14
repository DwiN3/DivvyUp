namespace DivvyUp_Shared.Dtos.Request
{
    public class AddEditReceiptDto
    {
        public string Name { get; set; }
        public DateOnly Date { get; set; }

        public AddEditReceiptDto()
        {
            Name = string.Empty;
            Date = DateOnly.FromDateTime(DateTime.Now);
        }

        public AddEditReceiptDto(string name, DateOnly date)
        {
            Name = name;
            Date = date;
        }
    }
}