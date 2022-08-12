namespace BookingApp.Rooms.Domain.Dtos
{
    public class UserDto : BaseDtoWithId
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
