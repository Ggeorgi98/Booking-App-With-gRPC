using BookingApp.Users.Domain.Services;
using BookingApp.Users.Domain.Utils;

namespace BookingApp.Users.DomainServices
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
