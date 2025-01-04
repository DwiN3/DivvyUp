namespace DivvyUp_Shared.Dtos.Request
{
    public class EditUserDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public EditUserDto()
        {
            Username = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            Email = string.Empty;
        }

        public EditUserDto(string username, string name, string surname, string email)
        {
            Username = username;
            Name = name;
            Surname = surname;
            Email = email;
        }
    }
}