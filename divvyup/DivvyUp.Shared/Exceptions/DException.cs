using System.Net;

namespace DivvyUp_Shared.Exceptions
{
    public class DException : Exception
    {
        public HttpStatusCode Status { get; }

        public new string Message { get; }

        public DException(HttpStatusCode status, string message) : base(message)
        {
            Status = status;
            Message = message;
        }

        public DException(string message) : base(message)
        {
            Message = message;
        }
    }
}
