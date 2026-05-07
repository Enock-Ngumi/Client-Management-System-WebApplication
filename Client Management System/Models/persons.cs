namespace Client_Management_System.Models
{
    public class Persons
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";

        public DateTime Dob { get; set; }
    }
}
