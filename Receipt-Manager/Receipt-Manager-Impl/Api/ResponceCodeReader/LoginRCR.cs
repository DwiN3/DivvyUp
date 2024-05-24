﻿using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Receipt_Manager_Impl.Api.ResponceCodeReader
{
    public class LoginRCR
    {
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
    }
}
