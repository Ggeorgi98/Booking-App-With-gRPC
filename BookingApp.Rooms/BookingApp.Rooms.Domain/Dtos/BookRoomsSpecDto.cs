namespace BookingApp.Rooms.Domain.Dtos
{
    public class BookRoomsSpecDto
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public Guid UserId { get; set; }

        public IEnumerable<Guid> RoomIds { get; set; } = new List<Guid>();
    }
}
