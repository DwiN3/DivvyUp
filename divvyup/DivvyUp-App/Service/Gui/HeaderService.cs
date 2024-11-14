using DivvyUp_Shared.AppConstants;

namespace DivvyUp_App.Service.Gui
{
    public class HeaderService
    {
        private Dictionary<string, string> MenuItems = new Dictionary<string, string>
        {
            { AppPath.DASHBOARD, "Główny panel" },
            { AppPath.RECEIPT, "Rachunki" },
            { AppPath.PERSON, "Osoby" },
            { AppPath.LOAN, "Pożyczki" },
            { AppPath.ACCOUT_MANAGER, "Zarządzaj kontem" },
            { AppPath.LOGIN, "Logowanie" },
            { AppPath.REGISTER, "Rejestracja" },
        };

        public string GetHeader(string url)
        {
            var relativePath = new Uri(url).AbsolutePath;

            if (relativePath.Contains(AppPath.RECEIPT) && relativePath.Contains("/products"))
            {
                return "Rachunek " + relativePath.Split('/')[2];
            }
            else if (MenuItems.ContainsKey(relativePath))
            {
                return MenuItems[relativePath];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
