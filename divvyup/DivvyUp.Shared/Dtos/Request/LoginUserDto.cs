namespace DivvyUp_Shared.Dtos.Request
{
    public class LoginUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginUserDto()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        public LoginUserDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}