namespace DivvyUp_Shared.RequestDto
{
    public class LoginUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginUserRequest()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        public LoginUserRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}