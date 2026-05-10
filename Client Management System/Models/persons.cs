using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Client_Management_System.Models
{
    public class Persons
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _phoneNumber = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters allowed")]
        public string FirstName
        {
            get => _firstName;
            set => _firstName = Capitalize(value);
        }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters allowed")]
        public string LastName
        {
            get => _lastName;
            set => _lastName = Capitalize(value);
        }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^(0\d{9}|254\d{9})$",
            ErrorMessage = "Enter 0712345678 or 254712345678")]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = FormatPhone(value);
        }

        public string PhoneDisplay
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PhoneNumber))
                    return string.Empty;

                if (PhoneNumber.StartsWith("254") && PhoneNumber.Length == 12)
                    return "0" + PhoneNumber.Substring(3);

                return PhoneNumber;
            }
        }

        [Column("dateofbirth")]
        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        private string Capitalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(value.ToLower());
        }

        private string FormatPhone(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.Trim();

            if (value.StartsWith("0") && value.Length == 10)
                return "254" + value.Substring(1);

            if (value.StartsWith("+254"))
                return value.Substring(1);

            return value;
        }
        public bool IsDeleted { get; set; } = false;
    }
}