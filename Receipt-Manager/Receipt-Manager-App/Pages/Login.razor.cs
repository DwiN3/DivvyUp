﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
        private string Username { get; set; }
        private string Password { get; set; }
        [Inject]
        private IAuthService AuthService { get; set; }

        private ResponseCodeReader RCR { get; set; } = new();
        private string LoginInfo { get; set; } = string.Empty;
        private string ColorInfo { get; set; } = "black";
        

        private async Task SingUp()
        {
            try
            {
                var response = await AuthService.Login(Username, Password);
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
            catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException)
            {
                Console.WriteLine($"Błąd połączenia: {socketException.Message}");
                LoginInfo = "Błąd połączenia z serwerem.";
                ColorInfo = "red";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                LoginInfo = "Wystąpił nieoczekiwany błąd.";
                ColorInfo = "red";
            }
        }
    }
}
