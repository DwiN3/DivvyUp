namespace DivvyUp_Shared.Dto
{
    public class PersonDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public int receiptsCount { get; set; }
        public double totalAmount { get; set; }
        public double unpaidAmount { get; set; }

        public string fullName => $"{name} {surname}";

        public PersonDto()
        {
            id = 0;
            name = string.Empty;
            surname = string.Empty;
            receiptsCount = 0;
            totalAmount = 0;
            unpaidAmount = 0;
        }

        public PersonDto(int id, string name, string surname, int receiptsCount, double totalAmount, double unpaidAmount)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.receiptsCount = receiptsCount;
            this.totalAmount = totalAmount;
            this.unpaidAmount = unpaidAmount;
        }
    }
}
