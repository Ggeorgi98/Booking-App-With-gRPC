using System.ComponentModel.DataAnnotations;

namespace BookingApp.Users.DAL.Entities
{
    public class BaseEntityWithId
    {
        [Key]
        public Guid Id { get; set; }
    }
}
