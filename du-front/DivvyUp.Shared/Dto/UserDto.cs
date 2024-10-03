namespace DivvyUp_Shared.Dto
{
    public class UserDto
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public UserDto()
        {
            username = string.Empty;
            email = string.Empty;
            password = string.Empty;
        }

        public UserDto(string username, string email, string password)
        {
            this.username = username;
            this.email = email;
            this.password = password;
        }
    }
}
