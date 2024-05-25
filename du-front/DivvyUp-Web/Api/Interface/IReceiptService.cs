using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp_Web.Api.Interface
{
    public interface IReceiptService
    {
        public Task<HttpResponseMessage> ShowAll(string token);
    }
}
