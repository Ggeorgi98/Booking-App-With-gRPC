using AutoMapper;
using BookingApp.Rooms.Domain.Services;
using BookingApp.Rooms.Grpc.Service;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using static BookingApp.Rooms.Grpc.Service.BookingsService;

namespace BookingApp.Rooms.Service
{
    public class BookingsService : BookingsServiceBase
    {
        private readonly ILogger<BookingsService> _logger;
        private readonly IBookingsService _bookingsService;
        private readonly IMapper _mapper;

        public BookingsService(ILogger<BookingsService> logger, IBookingsService bookingsService, IMapper mapper)
        {
            _logger = logger;
            _bookingsService = bookingsService;
            _mapper = mapper;
        }

        public override async Task<BookingResponse> GetLastUserBookingAsync(UserRequest request, ServerCallContext context)
        {
            var bookings = await _bookingsService.GetUserBookingsAsync(Guid.Parse(request.Id));

            return _mapper.Map<BookingResponse>(bookings.Result.OrderByDescending(x => x.FromDate).First());
        }
    }
}