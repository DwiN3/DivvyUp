using System.Xml.Linq;

namespace DivvyUp_Shared.Dtos.Request
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterUserDto()
        {
            Username = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        public RegisterUserDto(string username, string name, string surname, string email, string password)
        {
            Username = username;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
        }
    }
}