namespace DivvyUp_Shared.Dto
{
    public class PersonDto
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public int receiptsCount { get; set; }
        public double totalPurchaseAmount { get; set; }
    }
}
