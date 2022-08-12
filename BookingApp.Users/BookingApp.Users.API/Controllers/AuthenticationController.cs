using BookingApp.Users.Domain.Services;
using BookingApp.Users.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookingApp.Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AuthenticationController(IUsersService usersService)
        {
            _usersService = usersService;
            _usersService.ValidationDictionary = new ValidationDictionary(ModelState);
        }

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> LoginAsync(string email, string password)
        {
            var userToken = await _usersService.AuthenticateUserAsync(email, password);

            if (!_usersService.ValidationDictionary.IsValid())
            {
                return Unauthorized(_usersService.ValidationDictionary.GetModelState());
            }

            return Ok(userToken);
        }
    }
}
