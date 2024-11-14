using DivvyUp_Shared.AppConstants;

namespace DivvyUp_App.Services.Gui
{
    public class HeaderService
    {
        private Dictionary<string, string> MenuItems = new Dictionary<string, string>
        {
            { AppPath.DASHBOARD, "Główny panel" },
            { AppPath.RECEIPT, "Rachunki" },
            { AppPath.PERSON, "Osoby" },
            { AppPath.LOAN, "Pożyczki" },
            { AppPath.ACCOUT_MANAGER, "Zarządzanie kontem" },
            { AppPath.LOGIN, "Logowanie" },
            { AppPath.REGISTER, "Rejestracja" },
        };

        public string GetHeader(string url)
        {
            var relativePath = new Uri(url).AbsolutePath;

            if (relativePath.Contains(AppPath.RECEIPT) && relativePath.Contains(AppPath.PRODUCT))
            {
                return "Produkty";
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
