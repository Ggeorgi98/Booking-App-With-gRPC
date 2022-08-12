using BookingApp.Users.DomainServices.Utils;

namespace BookingApp.Users.API.Utils
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; }
        
        public CryptographySettings CryptographySettings { get; set; }
    }
}
