using DivvyUp_Impl.Interface;
using DivvyUp_Impl.Model;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace DivvyUp_Impl.Service
{
    public class UserAppService 
    {
        private readonly string _userFileName;
        private User _currentUser;

        public UserAppService()
        {
            string projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\.."));
            string userFilePath = Path.Combine(projectDirectory, "wwwroot", "user", "user_data.json");
            _userFileName = userFilePath;
            LoadUserData();
        }

        public User GetUser() => _currentUser;

        public void SetUser(string username, string token, bool isLogin)
        {
            _currentUser = new User { username = username, token = token, isLogin = isLogin };
            SaveUserData();
        }

        public void ClearUser()
        {
            _currentUser = new User();
            ClearUserData();
        }

        public bool IsLoggedIn() => _currentUser.isLogin;

        private void SaveUserData()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string userData = JsonSerializer.Serialize(new User
                {
                    username = _currentUser.username,
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
                    _currentUser = JsonSerializer.Deserialize<User>(userData) ?? new User();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas odczytu danych użytkownika: {ex.Message}");
                _currentUser = new User();
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

                    string userData = JsonSerializer.Serialize(new User
                    {
                        username = string.Empty,
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
