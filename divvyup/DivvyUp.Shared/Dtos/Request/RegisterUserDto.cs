namespace DivvyUp_Shared.Dtos.Request
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterUserDto()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        public RegisterUserDto(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}