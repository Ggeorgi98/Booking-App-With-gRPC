namespace BookingApp.Rooms.Domain.Dtos
{
    public class BookRoomsDto : BaseDtoWithId
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public Guid UserId { get; set; }

        public UserDto User { get; set; } = new UserDto();

        public IEnumerable<RoomDto> Rooms { get; set; } = new List<RoomDto>();
    }
}
