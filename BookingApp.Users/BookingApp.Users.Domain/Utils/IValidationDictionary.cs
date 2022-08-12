using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookingApp.Users.Domain.Utils
{
    public interface IValidationDictionary
    {
        bool IsValid();

        void AddModelError(string key, string message);

        ModelStateDictionary GetModelState();
    }
}
