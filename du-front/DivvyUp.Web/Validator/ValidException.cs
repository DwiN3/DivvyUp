namespace DivvyUp.Web.Validator
{
    public class ValidException : Exception
    {
        public ValidException() : base() { }

        public ValidException(string message) : base(message) { }

        public ValidException(string message, Exception innerException) : base(message, innerException) { }
    }
}
