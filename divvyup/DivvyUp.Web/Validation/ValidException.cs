using System.Net;

namespace DivvyUp.Web.Validation
{
    public class ValidException : Exception
    {
        public HttpStatusCode Status { get; }

        public ValidException(HttpStatusCode status, string message)
            : base(message)
        {
            Status = status;
        }

        public ValidException(HttpStatusCode status, string message, Exception innerException)
            : base(message, innerException)
        {
            Status = status;
        }
    }
}
