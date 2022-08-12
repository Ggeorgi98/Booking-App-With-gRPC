namespace BookingApp.Rooms.Domain.Utils
{
    public interface ITokenProvider
    {
        Task<string> GetTokenAsync();
    }
}
