using DivvyUp.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DivvyUp_Impl.Model;

namespace DivvyUp_Impl.Interface
{
    public interface IUserAppService
    {
        User GetUser();
        void SetUser(string username, string token, bool isLogin);
        void ClearUser();
        bool IsLoggedIn();
    }
}
