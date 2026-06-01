using PersonalHub.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalHub.Domain.Entities.Security
{
    public class User : Entity
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; }
        public User(string username, string email, string password)
        {
            Username = username;
            Password = password;
            CreatedAt = DateTime.UtcNow;
        }
        public void UpdatePassword(string newPassword)
        {
            Password = newPassword;
        }
    }
}
