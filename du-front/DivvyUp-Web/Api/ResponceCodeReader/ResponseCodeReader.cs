using System.Net;

namespace DivvyUp_Web.Api.ResponceCodeReader
{
    public class ResponseCodeReader
    {
        // Auth
        public string ReadLogin(HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.OK:
                    return "Pomyślnie zalogowano";
                case HttpStatusCode.BadRequest:
                    return "Błędne zapytanie";
                case HttpStatusCode.Unauthorized:
                    return "Błędne dane";
                case HttpStatusCode.NotFound:
                    return "Nie znaleziono użytkownika";
                default:
                    return "Błędne zapytanie";
            }
        }

        public string ReadRegister(HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.OK:
                    return "Pomyślnie utworzono konto";
                case HttpStatusCode.BadRequest:
                    return "Błędne zapytanie";
                case HttpStatusCode.Conflict:
                    return "Użytkownik o podanej nazwie istnieje";
                default:
                    return "Błędne zapytanie";
            }
        }
    }
}
