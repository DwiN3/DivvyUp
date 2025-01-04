namespace DivvyUp_Shared.Dtos.Entity
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserDto()
        {
            Id = 0;
            Username = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        public UserDto(int id, string username, string name, string surname, string email, string password)
        {
            Id = id;
            Username = username;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
        }
    }
}
