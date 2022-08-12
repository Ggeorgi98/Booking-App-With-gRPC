using BookingApp.Users.Domain.Utils;

namespace BookingApp.Users.Domain.Services
{
    public interface IBaseService
    {

        IValidationDictionary ValidationDictionary { get; set; }
    }
}
