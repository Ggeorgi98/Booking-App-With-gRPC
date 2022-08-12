namespace BookingApp.Users.Domain
{
    public interface IPasswordHashingHelper
    {
        string GenerateSalt();

        string HashPassword(string password, string salt);
    }
}
