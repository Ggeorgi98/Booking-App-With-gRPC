using BookingApp.Users.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Users.Domain.Dtos
{
    public class UserSpecDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
