
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DivvyUp.Shared.Model;

namespace DivvyUp.Shared.Interface
{
    public interface IUserAppService
    {
        User GetUser();
        void SetUser(string username, string token, bool isLogin);
        void ClearUser();
        bool IsLoggedIn();
    }
}
