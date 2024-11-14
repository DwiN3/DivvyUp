namespace DivvyUp_Shared.Dtos.Entity
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserDto()
        {
            Id = 0;
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        public UserDto(int id, string username, string email, string password)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
        }
    }
}
