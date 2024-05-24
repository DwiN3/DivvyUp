using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Receipt_Manager_Impl;
using Receipt_Manager_Impl.Api.Interface;
using Receipt_Manager_Impl.Api.ResponceCodeReader;
using Receipt_Manager_Impl.Api.Response;

namespace Receipt_Manager_App.Pages
{
    partial class Login
    {
        private String Username { get; set; }
        private String Password { get; set; }
        [Inject]
        private IAuthService authService { get; set; }
        private String LoginInfo { get; set; } = String.Empty;
        private String ColorInfo { get; set; } = "black";
        private LoginRCR RCR { get; set; } = new LoginRCR();

        private async Task SingUp()
        {
            var response = await authService.Login(Username, Password);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);
                System.Diagnostics.Debug.Print(loginResponse.token);
                ColorInfo = "green";
            }
            else
            {
                ColorInfo = "red";
            }
            LoginInfo = RCR.ReadLogin(response.StatusCode);
        }
    }
}
