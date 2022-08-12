using System.ComponentModel.DataAnnotations;

namespace BookingApp.Rooms.DAL.Entities
{
    public class BaseEntityWithId
    {
        [Key]
        public Guid Id { get; set; }
    }
}
