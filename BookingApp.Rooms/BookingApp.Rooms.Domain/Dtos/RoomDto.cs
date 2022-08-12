namespace BookingApp.Rooms.Domain.Dtos
{
    public class RoomDto : BaseDtoWithId
    {
        public int Capacity { get; set; }

        public double Price { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
