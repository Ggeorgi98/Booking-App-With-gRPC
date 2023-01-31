using BookingApp.Rooms.Client.ResponseModels;

namespace BookingApp.Users.Domain.Dtos
{
    public class UserFullDataDto : UserDto
    {
        public BookingResponse BookingData { get; set; }
    }
}
