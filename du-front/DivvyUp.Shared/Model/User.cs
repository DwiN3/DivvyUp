using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp.Shared.Model
{
    public class User
    {
        public string username { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public bool isLogin { get; set; } = false;
    }
}
