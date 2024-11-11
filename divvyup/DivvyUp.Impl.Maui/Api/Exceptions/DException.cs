using System.Net;
using System.Text.RegularExpressions;

namespace DivvyUp_Impl_Maui.Api.Exceptions
{
    public class DException : Exception
    {
        public HttpStatusCode Status { get; }

        public new string Message { get; }

        public string DisplayMessage { get; }


        public DException(HttpStatusCode status, string message) : base(message)
        {
            Status = status;
            Message = message;
            DisplayMessage = CleanMessage(message);
        }

        public DException(string message) : base(message)
        {
            Message = message;
            DisplayMessage = CleanMessage(message);
        }

        private static string CleanMessage(string message)
        {
            string cleanedMessage = message.Replace("DivvyUp_Impl_Maui.Api.Exceptions.DException: ", "").Trim();
            int atIndex = cleanedMessage.IndexOf(" at");
            if (atIndex >= 0)
            {
                return cleanedMessage.Substring(0, atIndex).Trim();
            }

            return cleanedMessage.Trim();
        }
    }
}
