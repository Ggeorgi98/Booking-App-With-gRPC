using BookingApp.Users.Domain.Utils;

namespace BookingApp.Users.Domain.Dtos
{
    public class UserTokenDto
    {
        public UserRole Role { get; set; }

        public Guid UserId { get; set; }

        public string Email { get; set; }
    }
}
