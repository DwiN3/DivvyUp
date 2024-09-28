using DivvyUp_Shared.Model;

namespace DivvyUp_Shared.Interface
{
    public interface IUserAppService
    {
        User GetUser();
        void SetUser(string username, string token, bool isLogin);
        void ClearUser();
        bool IsLoggedIn();
    }
}
