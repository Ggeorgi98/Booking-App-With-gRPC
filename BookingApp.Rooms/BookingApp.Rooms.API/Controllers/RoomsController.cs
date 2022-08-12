using BookingApp.Rooms.Domain;
using BookingApp.Rooms.Domain.Dtos;
using BookingApp.Rooms.Domain.Services;
using BookingApp.Rooms.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookingApp.Rooms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsService _roomsService;

        public RoomsController(IRoomsService roomsService)
        {
            _roomsService = roomsService;
            _roomsService.ValidationDictionary = new ValidationDictionary(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Domain.Dtos.RoomDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<RoomDto>> GetRoomById(Guid id)
        {
            var response = await _roomsService.GetByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(PagedResults<RoomDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PagedResults<RoomDto>>> GetPagedAllRooms()
        {
            var pager = new Paginator
            {
                CurrentPage = 1,
                PageSize = int.MaxValue
            };

            var result = await _roomsService.GetListAsync(pager, x => true, null, true);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> CreateRoom([FromBody] RoomSpecDto model)
        {
            var user = await _roomsService.AddAsync<RoomSpecDto>(model);

            if (user == null)
            {
                return BadRequest(_roomsService.ValidationDictionary.GetModelState());
            }

            return Ok(user.Id);
        }
    }
}
