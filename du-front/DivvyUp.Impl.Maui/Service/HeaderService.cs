using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivvyUp_Impl_Maui.Service
{
    public class HeaderService
    {
        private Dictionary<string, string> MenuItems = new Dictionary<string, string>
        {
            { "/", "Strona Główna" },
            { "/receipt", "Rachunki" },
            { "/person", "Osoby" },
            { "/accountManager", "Zarządzaj kontem" },
            { "/login", "Logowanie" },
            { "/register", "Rejestracja" },
        };

        public string GetHeader(string url)
        {
            var relativePath = new Uri(url).AbsolutePath;

            if (relativePath.Contains("/receipt/") && relativePath.Contains("/items"))
                return "Rachunek " + relativePath.Split('/')[2];
            
            else if (MenuItems.ContainsKey(relativePath))
                return MenuItems[relativePath];
            
            else
                return string.Empty;
        }
    }
}
