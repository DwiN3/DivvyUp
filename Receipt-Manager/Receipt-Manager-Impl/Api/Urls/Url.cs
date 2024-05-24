using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receipt_Manager_Impl.Api.Urls
{
    public class Url
    {
        /// Auth
        public readonly string Register = "http://localhost:8080/rm/register";
        public readonly string Login = "http://localhost:8080/rm/auth";
        public readonly string Remove = "http://localhost:8080/rm/remove-account";
    }
}
