using System.Net;

namespace DivvyUp_Impl_Maui.Api.CodeReader
{
    public class CodeReaderResponse
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
                    return "Błędne hasło";
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
