using System;

namespace Client_Management_System.Models
{
    public class LoginUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public int FailedAttempts { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public DateTime? LockoutEnd { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}