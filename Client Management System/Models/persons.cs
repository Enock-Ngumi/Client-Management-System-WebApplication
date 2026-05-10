using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client_Management_System.Models
{
    public class Persons : BaseEntity
    {
        public int? UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters allowed")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters allowed")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^07\d{8}$",
            ErrorMessage = "Enter number in format 0712345678")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string PhoneDisplay =>
            PhoneNumber.StartsWith("254") && PhoneNumber.Length == 12
                ? "0" + PhoneNumber.Substring(3)
                : PhoneNumber;

        [Column("dateofbirth")]
        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        public void NormalizePhone()
        {
            if (!string.IsNullOrWhiteSpace(PhoneNumber) &&
                PhoneNumber.StartsWith("07") &&
                PhoneNumber.Length == 10)
            {
                PhoneNumber = "254" + PhoneNumber.Substring(1);
            }
        }
    }
}