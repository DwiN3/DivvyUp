namespace DivvyUp_Shared.Dtos.Request
{
    public class ChangePasswordUserDto
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }

        public ChangePasswordUserDto()
        {
            Password = string.Empty;
            NewPassword = string.Empty;
        }

        public ChangePasswordUserDto(string password, string newPassword)
        {
            Password = password;
            NewPassword = newPassword;
        }
    }
}