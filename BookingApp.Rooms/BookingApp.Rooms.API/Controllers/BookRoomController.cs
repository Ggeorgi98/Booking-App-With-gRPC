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
    public class BookRoomController : ControllerBase
    {
        private readonly IBookingsService _bookService;

        public BookRoomController(IBookingsService bookService)
        {
            _bookService = bookService;
            _bookService.ValidationDictionary = new ValidationDictionary(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Domain.Dtos.BookRoomsDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<BookRoomsDto>> GetBookingById(Guid id)
        {
            var response = await _bookService.GetByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(PagedResults<BookRoomsDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PagedResults<BookRoomsDto>>> GetPagedAllBookings()
        {
            var pager = new Paginator
            {
                CurrentPage = 1,
                PageSize = int.MaxValue
            };

            var result = await _bookService.GetListAsync(pager, x => true, null, true);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("book")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> BookRooms([FromBody] BookRoomsSpecDto model)
        {
            var booking = await _bookService.BookRoomsAsync(model);

            if (booking == null)
            {
                return BadRequest(_bookService.ValidationDictionary.GetModelState());
            }

            return Ok(booking.Id);
        }
    }
}
