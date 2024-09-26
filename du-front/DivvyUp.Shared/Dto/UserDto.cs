namespace DivvyUp.Shared.Dto
{
    public class UserDto
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public UserDto()
        {
            userId = 0;
            username = string.Empty;
            email = string.Empty;
            password = string.Empty;
        }

        public UserDto(int userId, string username, string email, string password)
        {
            this.userId = userId;
            this.username = username;
            this.email = email;
            this.password = password;
        }
    }
}
