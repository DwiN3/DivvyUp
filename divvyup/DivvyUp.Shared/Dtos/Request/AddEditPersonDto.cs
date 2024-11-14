namespace DivvyUp_Shared.Dtos.Request
{
    public class AddEditPersonDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public AddEditPersonDto()
        {
            Name = string.Empty;
            Surname = string.Empty;
        }

        public AddEditPersonDto(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
    }
}