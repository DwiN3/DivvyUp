namespace DivvyUp_Shared.RequestDto
{
    public class EditUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public EditUserRequest()
        {
            Username = string.Empty;
            Email = string.Empty;
        }

        public EditUserRequest(string username, string email)
        {
            Username = username;
            Email = email;
        }
    }
}