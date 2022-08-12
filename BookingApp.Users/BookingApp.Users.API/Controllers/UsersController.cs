using BookingApp.Users.Domain;
using BookingApp.Users.Domain.Dtos;
using BookingApp.Users.Domain.Services;
using BookingApp.Users.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookingApp.Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
            _usersService.ValidationDictionary = new ValidationDictionary(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var response = await _usersService.GetByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(PagedResults<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PagedResults<UserDto>>> GetPagedAllUsers()
        {
            var pager = new Paginator
            {
                CurrentPage = 1,
                PageSize = int.MaxValue
            };

            var result = await _usersService.GetListAsync(pager, x => true, null, true);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] UserSpecDto model)
        {
            if (await _usersService.AnyAsync(x => x.Email == model.Email))
            {
                ModelState.AddModelError(nameof(model.Email), "Email exists");

                return BadRequest(ModelState);
            }

            var user = await _usersService.CreateUserAsync(model);

            if (user == null)
            {
                return BadRequest(_usersService.ValidationDictionary.GetModelState());
            }

            return Ok(user.Id);
        }
    }
}
