using BookingApp.Users.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Users.DAL.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntityWithId
    {
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public string PasswordSalt { get; set; }
    }
}
