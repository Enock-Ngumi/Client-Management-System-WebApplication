namespace Client_Management_System.Models
{
    public class LoginUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }
    }
}
