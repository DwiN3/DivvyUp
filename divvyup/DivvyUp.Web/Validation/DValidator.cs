using System.Net;
using Microsoft.IdentityModel.Tokens;
using DivvyUp_Shared.Exceptions;

namespace DivvyUp.Web.Validation
{
    public class DValidator
    {
        public void IsNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new DException(HttpStatusCode.BadRequest, message);
            }
        }

        public void IsEmpty(string str, string message)
        {
            if (str.IsNullOrEmpty() || str.Equals(""))
            {
                throw new DException(HttpStatusCode.BadRequest, message);
            }
        }

        public void IsMinusValue(decimal obj, string message)
        {
            if (obj < 0)
            {
                throw new DException(HttpStatusCode.BadRequest, message);
            }
        }

        public void IsCorrectDataRange(DateOnly dateFrom, DateOnly dateTo)
        {
            if (dateFrom > dateTo)
            {
                throw new DException(HttpStatusCode.BadRequest, "Zakres dat jest źle ustawiony");
            }
        }

        public void IsCorrectPercentageRange(int percentageValue)
        {
            if (percentageValue < 0 || percentageValue > 100)
            {
                throw new DException(HttpStatusCode.BadRequest, "Wartość procentowa jest błędnie ustawiona");
            }
        }
    }
}
