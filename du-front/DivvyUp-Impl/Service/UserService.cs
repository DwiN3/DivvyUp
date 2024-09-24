using DivvyUp_Impl.Model;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DivvyUp_Impl.Service
{
    public class UserService
    {
        private readonly string _userFileName;
        private User _currentUser;
        private const string UserFileName = @"C:\Users\dwini\Desktop\DivvyUp\du-front\DivvyUp-App\wwwroot\user_data.json";


        public UserService(string basePath)
        {
            _userFileName = Path.Combine(UserFileName);
            LoadUserData();
        }

        public User GetUser() => _currentUser;

        public void SetUser(string username, string token, bool isLogin)
        {
            _currentUser.username = username;
            _currentUser.token = token;
            _currentUser.isLogin = isLogin;

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
                var userData = JsonSerializer.Serialize(_currentUser);
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
                    string jsonContent = "{\n" +
                                         "  \"username\": \"\",\n" +
                                         "  \"token\": \"\",\n" +
                                         "  \"isLogin\": false\n" +
                                         "}";

                    File.WriteAllText(_userFileName, jsonContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania danych użytkownika: {ex.Message}");
            }
        }
    }
}
