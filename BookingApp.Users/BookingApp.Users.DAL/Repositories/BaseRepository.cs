using AutoMapper;
using BookingApp.Users.DAL.Context;
using BookingApp.Users.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Users.DAL.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected readonly IDbContextFactory<UsersDBContext> _dbContext;
        protected IMapper Mapper { get; }

        protected BaseRepository(IDbContextFactory<UsersDBContext> dBContext, IMapper mapper)
        {
            _dbContext = dBContext ?? throw new ArgumentNullException();
            Mapper = mapper ?? throw new ArgumentNullException();
        }
    }
}
