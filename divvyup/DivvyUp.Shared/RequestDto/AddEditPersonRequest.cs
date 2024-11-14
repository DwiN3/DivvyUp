namespace DivvyUp_Shared.RequestDto
{
    public class AddEditPersonRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public AddEditPersonRequest()
        {
            Name = string.Empty;
            Surname = string.Empty;
        }

        public AddEditPersonRequest(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
    }
}