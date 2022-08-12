using BookingApp.Rooms.Domain;
using BookingApp.Rooms.Domain.Dtos;
using BookingApp.Rooms.Domain.Repositories;
using BookingApp.Rooms.Domain.Services;
using BookingApp.Users.Client;
using Grpc.Core;
using System.Linq.Expressions;

namespace BookingApp.Rooms.DomainService
{
    public class BookingsService : BaseCrudService<BookRoomsDto>, IBookingsService
    {
        private readonly IBookingsRepository _bookRoomsRepository;
        private readonly UsersService.UsersServiceClient _usersServiceClient;

        public BookingsService(IBookingsRepository bookRoomsRepository, UsersService.UsersServiceClient usersServiceClient)
            : base(bookRoomsRepository)
        {
            _bookRoomsRepository = bookRoomsRepository;
            _usersServiceClient = usersServiceClient;
        }

        public async Task<BookRoomsDto> BookRoomsAsync(BookRoomsSpecDto dto)
        {
            return await _bookRoomsRepository.BookRoomsAsync(dto);
        }

        public override async Task<BookRoomsDto> GetByIdAsync(Guid id)
        {
            var bookingDto = await base.GetByIdAsync(id);

            var userDto = await _usersServiceClient.GetUserByIdAsync(new UserRequest
            {
                Id = bookingDto.UserId.ToString()
            });

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

            return bookings;
        }
    }
}
