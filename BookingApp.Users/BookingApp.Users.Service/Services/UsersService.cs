using AutoMapper;
using BookingApp.Users.Client;
using BookingApp.Users.Domain;
using BookingApp.Users.Domain.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using static BookingApp.Users.Client.UsersService;

namespace BookingApp.Users.Service.Services
{
    public class UsersService : UsersServiceBase
    {
        private readonly ILogger<UsersService> _logger;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersService(ILogger<UsersService> logger, IUsersService usersService, IMapper mapper)
        {
            _logger = logger;
            _usersService = usersService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        public override async Task<UserResponse> GetUserById(UserRequest request, ServerCallContext context)
        {
            var userDto = await _usersService.GetByIdAsync(Guid.Parse(request.Id));
            if (userDto == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"User with ID={request.Id} is not found."));
            }

            return new UserResponse
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.PhoneNumber
            };
        }

        public override async Task GetAllUsers(AllUsersRequest request, IServerStreamWriter<UserResponse> responseStream,
            ServerCallContext context)
        {
            var pager = new Paginator
            {
                CurrentPage = 1,
                PageSize = int.MaxValue
            };

            var users = await _usersService.GetListAsync(pager, x => request.Ids.Contains(x.Id.ToString()), null, true);

            foreach (var user in users.Result)
            {
                await responseStream.WriteAsync(_mapper.Map<UserResponse>(user));
            }
        }
    }
}
