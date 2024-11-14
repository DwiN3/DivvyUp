namespace DivvyUp_Shared.Dto
{
    public class ChartDto
    {
        public string Name { get; set; }
        public decimal Value { get; set; }

        public ChartDto()
        {
            Name = string.Empty;
            Value = 0;
        }

        public ChartDto(string name, decimal value)
        {
            Name = name;
            Value = value;
        }
    }
}
