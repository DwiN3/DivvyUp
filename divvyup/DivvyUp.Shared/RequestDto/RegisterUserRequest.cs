﻿namespace DivvyUp_Shared.RequestDto
{
    public class RegisterUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterUserRequest()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        public RegisterUserRequest(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}