namespace DivvyUp_App.GuiService
{
    public class HeaderService
    {
        private Dictionary<string, string> MenuItems = new Dictionary<string, string>
        {
            { "/", "Główny panel" },
            { "/receipt", "Rachunki" },
            { "/person", "Osoby" },
            { "/loan", "Pożyczki" },
            { "/accountManager", "Zarządzaj kontem" },
            { "/login", "Logowanie" },
            { "/register", "Rejestracja" },
        };

        public string GetHeader(string url)
        {
            var relativePath = new Uri(url).AbsolutePath;

            if (relativePath.Contains("/receipt/") && relativePath.Contains("/products"))
                return "Rachunek " + relativePath.Split('/')[2];
            
            else if (MenuItems.ContainsKey(relativePath))
                return MenuItems[relativePath];
            
            else
                return string.Empty;
        }
    }
}
