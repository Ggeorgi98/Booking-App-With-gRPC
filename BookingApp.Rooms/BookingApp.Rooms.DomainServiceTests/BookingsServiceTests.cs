using BookingApp.Rooms.Domain;
using BookingApp.Rooms.Domain.Dtos;
using BookingApp.Rooms.Domain.Repositories;
using BookingApp.Rooms.DomainService;
using BookingApp.Users.Client;
using Grpc.Core;
using Moq;
using System.Linq.Expressions;
using System.Net;

namespace BookingApp.Rooms.DomainServiceTests
{
    public class BookingsServiceTests
    {
        private readonly Mock<IBookingsRepository> _bookingsRepository;
        private readonly Mock<UsersService.UsersServiceClient> _usersServiceClient;
        private readonly BookingsService _bookingsService;

        public BookingsServiceTests()
        {
            _bookingsRepository = new Mock<IBookingsRepository>();
            _usersServiceClient = new Mock<UsersService.UsersServiceClient>();
            _bookingsService = new BookingsService(_bookingsRepository.Object, _usersServiceClient.Object);
        }

        [Fact]
        public async Task RetryGetUser_When_Received_Timeout()
        {
            // ARRANGE
            var id = Guid.NewGuid().ToString();
            var bookRoomDto = new BookRoomsDto
            {
                FromDate = default,
                ToDate = default,
                Id = Guid.Parse(id),
                UserId = Guid.Parse(id)
            };

            _usersServiceClient.SetupSequence(x => x.GetUserByIdAsync(It.IsAny<UserRequest>(), null, null, default))
                .Throws(new HttpRequestException("", null, HttpStatusCode.RequestTimeout))
                .Throws(new HttpRequestException("", null, HttpStatusCode.RequestTimeout))
                .Returns(new AsyncUnaryCall<UserResponse>(Task.FromResult(new UserResponse { Id = id }), null, null, null, null));

            _bookingsRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(bookRoomDto);

            // ACT
            var bookingResult = await _bookingsService.GetByIdAsync(Guid.NewGuid()).ConfigureAwait(false);

            // ASSERT
            Assert.NotNull(bookingResult);
            Assert.Equal(id, bookingResult.Id.ToString());

            _usersServiceClient.Verify(x => x.GetUserByIdAsync(It.IsAny<UserRequest>(), null, null, default), Times.Exactly(3));
        }

        [Fact]
        public async Task HandleFallbackExampleOnMultipleTimeouts()
        {
            var id = Guid.NewGuid();
            var bookRoomDto = new BookRoomsDto
            {
                FromDate = default,
                ToDate = default,
                Id = id,
                UserId = id
            };

            var bookings = new PagedResults<BookRoomsDto>(new List<BookRoomsDto> { bookRoomDto }, 1, 1, 1, 1);

            _usersServiceClient.SetupSequence(x => x.GetAllUsers(It.IsAny<AllUsersRequest>(), null, null, default))
                 .Throws(new HttpRequestException("", null, HttpStatusCode.RequestTimeout))
                 .Throws(new HttpRequestException("", null, HttpStatusCode.RequestTimeout))
                 .Throws(new HttpRequestException("", null, HttpStatusCode.RequestTimeout));

            _bookingsRepository.Setup(x => x.GetListAsync(It.IsAny<Paginator>(), It.IsAny<Expression<Func<BookRoomsDto, bool>>>(),
                It.IsAny<Expression<Func<BookRoomsDto, object>>>(), It.IsAny<bool>()))
               .ReturnsAsync(bookings);

            // ACT
            await _bookingsService.GetListAsync(new Paginator(), x => true, null, true).ConfigureAwait(false);

            // ASSERT
            _usersServiceClient.Verify(x => x.GetAllUsers(It.IsAny<AllUsersRequest>(), null, null, default), Times.Exactly(4));
            Assert.Single(_bookingsService.UserIdsQueue);
        }

        [Fact]
        public async Task CircuitBreaker_Should_Trigger_When_Ten_Exceptions_Are_Thrown()
        {
            var id = Guid.NewGuid();
            var bookRoomDto = new BookRoomsDto
            {
                FromDate = default,
                ToDate = default,
                Id = id,
                UserId = id
            };

            var bookings = new PagedResults<BookRoomsDto>(new List<BookRoomsDto> { bookRoomDto }, 1, 1, 1, 1);
            _bookingsRepository.Setup(x => x.GetListAsync(It.IsAny<Paginator>(), It.IsAny<Expression<Func<BookRoomsDto, bool>>>(),
               It.IsAny<Expression<Func<BookRoomsDto, object>>>(), It.IsAny<bool>()))
              .ReturnsAsync(bookings);

            _usersServiceClient.Setup(x => x.GetAllUsers(It.IsAny<AllUsersRequest>(), null, null, default))
                 .Throws(new HttpRequestException("", null, HttpStatusCode.RequestTimeout));

            for (int i = 0; i <= 10; i++)
                await _bookingsService.GetListAsync(new Paginator(), x => true, null, true).ConfigureAwait(false);

            _usersServiceClient.Verify(x => x.GetAllUsers(It.IsAny<AllUsersRequest>(), null, null, default), Times.AtLeast(10));
            Assert.Equal(11, _bookingsService.UserIdsQueue.Count);
        }
    }
}