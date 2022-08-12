using System.ComponentModel.DataAnnotations;

namespace BookingApp.Rooms.DAL.Entities
{
    public class Room : BaseEntityWithId
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Capacity { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public double Price { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<BookedRoom> BookedRooms { get; set; }
    }
}
