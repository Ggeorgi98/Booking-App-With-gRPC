using BookingApp.Users.Domain.Utils;

namespace BookingApp.Users.Domain.Dtos
{
    public class UserDto : BaseDtoWithId
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
    }
}
