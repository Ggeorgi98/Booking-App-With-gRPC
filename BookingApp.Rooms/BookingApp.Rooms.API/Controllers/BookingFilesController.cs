using BookingApp.Rooms.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookingApp.Rooms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingFilesController : ControllerBase
    {
        private readonly IBookingFilesService _bookingFilesService;

        public BookingFilesController(IBookingFilesService bookingFilesService)
        {
            _bookingFilesService = bookingFilesService;
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(Domain.Dtos.BookRoomsDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<string>> UploadFileAsync([FromForm(Name = "file")] IFormFile file)
        {
            using var mem = new MemoryStream();
            await file.CopyToAsync(mem);

            await _bookingFilesService.UploadBookingFileAsync(mem, $@"D:\Georgi\packages\{file.FileName}");

            return Ok();
        }

        [HttpGet("download")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<IFormFile>> DownloadFileAsync()
        {
            var fileForDownload = await _bookingFilesService
                .DownloadBookingFileAsync(@"D:\Georgi\packages\IMG_0191.JPG");

            var content = new MemoryStream(fileForDownload);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "IMG_0191.JPG";

            return File(content, contentType, fileName);
        }
    }
}
