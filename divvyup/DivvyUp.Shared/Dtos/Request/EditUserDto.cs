namespace DivvyUp_Shared.Dtos.Request
{
    public class EditUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public EditUserDto()
        {
            Username = string.Empty;
            Email = string.Empty;
        }

        public EditUserDto(string username, string email)
        {
            Username = username;
            Email = email;
        }
    }
}