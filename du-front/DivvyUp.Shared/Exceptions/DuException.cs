using System.Net;

namespace DivvyUp_Shared.Exceptions
{
    public class DuException : Exception
    {
        public HttpStatusCode ErrorCode { get; }

        public DuException(HttpStatusCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
