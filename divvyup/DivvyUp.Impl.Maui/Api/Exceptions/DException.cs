using System.Net;

namespace DivvyUp_Impl_Maui.Api.Exceptions
{
    public class DException : Exception
    {
        public HttpStatusCode Status { get; }
        public string Message { get; }

        public DException(HttpStatusCode status, string message) : base(message)
        {
            Status = status;
            Message = message;
        }
    }
}
