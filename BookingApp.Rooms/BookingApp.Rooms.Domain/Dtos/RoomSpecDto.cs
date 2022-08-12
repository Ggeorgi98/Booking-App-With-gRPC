namespace BookingApp.Rooms.Domain.Dtos
{
    public class RoomSpecDto
    {
        public int Capacity { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
