using BookingApp.Rooms.Domain;
using BookingApp.Rooms.Domain.Dtos;
using BookingApp.Rooms.Domain.Repositories;
using BookingApp.Rooms.Domain.Services;
using BookingApp.Users.Client;
using Grpc.Core;
using Polly;
using System.Linq.Expressions;
using System.Net;

namespace BookingApp.Rooms.DomainService
{
    public class BookingsService : BaseCrudService<BookRoomsDto>, IBookingsService
    {
        public readonly Queue<Guid> UserIdsQueue = new();

        private readonly IBookingsRepository _bookRoomsRepository;
        private readonly UsersService.UsersServiceClient _usersServiceClient;
        private readonly IAsyncPolicy _circuitBreaker;

        private const int NUMBER_OF_RETRIES = 3;
        private const int EXCEPTIONS_ALLOWED_BEFORE_BREAKING_CIRCUIT = 10;

        public BookingsService(IBookingsRepository bookRoomsRepository, UsersService.UsersServiceClient usersServiceClient)
            : base(bookRoomsRepository)
        {
            _bookRoomsRepository = bookRoomsRepository;
            _usersServiceClient = usersServiceClient;
            _circuitBreaker = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(EXCEPTIONS_ALLOWED_BEFORE_BREAKING_CIRCUIT, TimeSpan.FromMilliseconds(5000));
        }

        public async Task<BookRoomsDto> BookRoomsAsync(BookRoomsSpecDto dto)
        {
            return await _bookRoomsRepository.BookRoomsAsync(dto);
        }

        public override async Task<BookRoomsDto> GetByIdAsync(Guid id)
        {
            var bookingDto = await base.GetByIdAsync(id);

            var userDto = await Policy
               .Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.RequestTimeout)
               .RetryAsync(NUMBER_OF_RETRIES, async (exception, retryCount) => await Task.Delay(500).ConfigureAwait(false))
               .ExecuteAsync(async () => await _usersServiceClient.GetUserByIdAsync(new UserRequest
               {
                   Id = bookingDto.UserId.ToString()
               }).ConfigureAwait(false))
               .ConfigureAwait(false);

            bookingDto.User = new UserDto
            {
                Email = userDto.Email,
                FullName = userDto.FirstName + " " + userDto.LastName,
                Id = bookingDto.UserId,
                PhoneNumber = userDto.PhoneNumber
            };

            return bookingDto;
        }

        public override async Task<PagedResults<BookRoomsDto>> GetListAsync(Paginator paginator,
            Expression<Func<BookRoomsDto, bool>> filter, Expression<Func<BookRoomsDto, object>> orderBy, bool ascending = true)
        {
            var bookings = await base.GetListAsync(paginator, filter, orderBy, ascending);
            var users = new List<UserResponse>();

            var userIds = bookings.Result.Select(x => x.UserId.ToString()).Distinct();
            var allUsersRequest = new AllUsersRequest();
            allUsersRequest.Ids.AddRange(userIds);

            var retryPolicy = Policy
               .Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.RequestTimeout)
               .RetryAsync(NUMBER_OF_RETRIES, async (exception, retryCount) => await Task.Delay(500));

            var fallbackPolicy = Policy
                .Handle<Exception>()
                .FallbackAsync(async (cancellationToken) => await SaveUserIdsInQueue(userIds.Select(x => Guid.Parse(x))).ConfigureAwait(false));

            await fallbackPolicy
               .WrapAsync(retryPolicy)
               .WrapAsync(_circuitBreaker)
               .ExecuteAsync(async () =>
               {
                   using var clientData = _usersServiceClient.GetAllUsers(allUsersRequest);
                   await foreach (var user in clientData.ResponseStream.ReadAllAsync())
                   {
                       var bookingUser = bookings.Result.FirstOrDefault(x => x.UserId == Guid.Parse(user.Id));
                       if (bookingUser != null)
                       {
                           bookingUser.User = new UserDto
                           {
                               Id = bookingUser.UserId,
                               Email = user.Email,
                               FullName = user.FirstName + " " + user.LastName,
                               PhoneNumber = user.PhoneNumber
                           };
                       }
                   }
               });     

            return bookings;
        }

        private Task SaveUserIdsInQueue(IEnumerable<Guid> userIds)
        {
            //an example use case in which in case of fallback the operation will be added in a queue for retrial later
            userIds.ToList().ForEach(x => UserIdsQueue.Enqueue(x));

            return Task.CompletedTask;
        }
    }
}
