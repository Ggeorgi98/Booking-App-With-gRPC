using BookingApp.Rooms.Domain.Services;
using BookingApp.Rooms.Domain.Utils;

namespace BookingApp.Rooms.DomainService
{
    public abstract class BaseService : IBaseService
    {
        private IValidationDictionary _validationDictionary;

        public IValidationDictionary ValidationDictionary
        {
            get { return _validationDictionary; }
            set { _validationDictionary = value; }
        }
    }
}
