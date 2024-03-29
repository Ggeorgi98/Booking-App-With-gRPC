﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookingApp.Rooms.Domain.Utils
{
    public interface IValidationDictionary
    {
        bool IsValid();

        void AddModelError(string key, string message);

        ModelStateDictionary GetModelState();
    }
}
