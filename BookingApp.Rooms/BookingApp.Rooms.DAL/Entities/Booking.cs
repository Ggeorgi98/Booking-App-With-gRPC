using System.ComponentModel.DataAnnotations;

namespace BookingApp.Rooms.DAL.Entities
{
    public class Booking : BaseEntityWithId
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public virtual ICollection<BookedRoom> BookedRooms { get; set; }
    }
}
