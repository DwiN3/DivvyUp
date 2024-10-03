namespace DivvyUp_Shared.Dto
{
    public class PersonDto
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public int receiptsCount { get; set; }
        public double totalAmount { get; set; }
        public double unpaidAmount { get; set; }

        public PersonDto()
        {
            id = 0;
            userId = 0;
            name = string.Empty;
            surname = string.Empty;
            receiptsCount = 0;
            totalAmount = 0;
            unpaidAmount = 0;
        }
    }
}
