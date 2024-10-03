namespace DivvyUp_Shared.Dto
{
    public class UserDto
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public UserDto()
        {
            id = 0;
            username = string.Empty;
            email = string.Empty;
            password = string.Empty;
        }

        public UserDto(int id, string username, string email, string password)
        {
            this.id = id;
            this.username = username;
            this.email = email;
            this.password = password;
        }
    }
}
