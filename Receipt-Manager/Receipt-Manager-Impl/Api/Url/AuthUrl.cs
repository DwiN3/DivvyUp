using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receipt_Manager_Impl.Api.Url
{
    public class AuthUrl
    {
        public String Register { get; } = "http://localhost:8080/rm/register";
        public String Login { get; } = "http://localhost:8080/rm/auth";
        public String Remove { get; } = "http://localhost:8080/rm/remove-account";
    }
}
