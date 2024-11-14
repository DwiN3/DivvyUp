namespace DivvyUp_Shared.RequestDto
{
    public class ChangePasswordUserRequest
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }

        public ChangePasswordUserRequest()
        {
            Password = string.Empty;
            NewPassword = string.Empty;
        }

        public ChangePasswordUserRequest(string password, string newPassword)
        {
            Password = password;
            NewPassword = newPassword;
        }
    }
}