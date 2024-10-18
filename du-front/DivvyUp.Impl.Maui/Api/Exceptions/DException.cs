using System.Net;

namespace DivvyUp_Impl_Maui.Api.Exceptions
{
    public class DException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }

        public DException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
