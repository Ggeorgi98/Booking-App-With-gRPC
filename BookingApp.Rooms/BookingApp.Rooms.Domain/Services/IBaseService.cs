using BookingApp.Rooms.Domain.Utils;

namespace BookingApp.Rooms.Domain.Services
{
    public interface IBaseService
    {

        IValidationDictionary ValidationDictionary { get; set; }
    }
}
