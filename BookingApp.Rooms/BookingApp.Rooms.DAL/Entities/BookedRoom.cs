namespace BookingApp.Rooms.DAL.Entities
{
    public class BookedRoom : BaseEntityWithId
    {
        public Guid BoookingId { get; set; }

        public virtual Booking Booking { get; set; }

        public Guid RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}
