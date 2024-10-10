namespace DivvyUp_Shared.Model
{
    public class User
    {
        public string id { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public bool isLogin { get; set; } = false;
    }
}
