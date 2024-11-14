using System.Text.Json;
using DivvyUp_Shared.Models;

namespace DivvyUp_App.Services.Gui
{
    public class UserAppService
    {
        private readonly string _userFileName;
        private UserApp _currentUser;

        public UserAppService()
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\.."));
            string userFilePath = Path.Combine(projectDirectory, "wwwroot", "user", "user_data.json");
            _userFileName = userFilePath;
            LoadUserData();
        }

        public UserApp GetUser() => _currentUser;

        public void SetUser(string username, string email, string token, bool isLogin)
        {
            _currentUser = new UserApp { username = username, email = email, token = token, isLogin = isLogin };
            SaveUserData();
        }

        public void ClearUser()
        {
            _currentUser = new UserApp();
            ClearUserData();
        }

        public void SetLoggedIn(bool isLogin) { _currentUser.isLogin = isLogin; }

        public bool IsLoggedIn() => _currentUser.isLogin;

        private void SaveUserData()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string userData = JsonSerializer.Serialize(new UserApp
                {
                    username = _currentUser.username,
                    email = _currentUser.email,
                    token = _currentUser.token,
                    isLogin = _currentUser.isLogin
                }, options);

                File.WriteAllText(_userFileName, userData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zapisu danych użytkownika: {ex.Message}");
            }
        }

        private void LoadUserData()
        {
            try
            {
                if (File.Exists(_userFileName))
                {
                    var userData = File.ReadAllText(_userFileName);
                    _currentUser = JsonSerializer.Deserialize<UserApp>(userData) ?? new UserApp();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas odczytu danych użytkownika: {ex.Message}");
                _currentUser = new UserApp();
            }
        }

        private void ClearUserData()
        {
            try
            {
                if (File.Exists(_userFileName))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };

                    string userData = JsonSerializer.Serialize(new UserApp
                    {
                        username = string.Empty,
                        email = string.Empty,
                        token = string.Empty,
                        isLogin = false
                    }, options);

                    File.WriteAllText(_userFileName, userData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania danych użytkownika: {ex.Message}");
            }
        }
    }
}
