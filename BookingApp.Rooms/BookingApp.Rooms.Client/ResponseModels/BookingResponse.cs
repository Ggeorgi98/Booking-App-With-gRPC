namespace BookingApp.Rooms.Client.ResponseModels
{
    public class BookingResponse
    {
        public Guid Id { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public Guid UserId { get; set; }

        public IEnumerable<RoomResponse> Rooms { get; set; } = new List<RoomResponse>();
    }
}
