using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp_Shared.Exceptions
{
    public class ValidException : Exception
    {
        public HttpStatusCode ErrorCode { get; }

        public ValidException(HttpStatusCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
