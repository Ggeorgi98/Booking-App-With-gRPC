using AutoMapper;
using BookingApp.Rooms.DAL.Context;
using BookingApp.Rooms.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Rooms.DAL.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected readonly IDbContextFactory<RoomsContext> _dbContext;
        protected IMapper Mapper { get; }

        protected BaseRepository(IDbContextFactory<RoomsContext> dBContext, IMapper mapper)
        {
            _dbContext = dBContext ?? throw new ArgumentNullException();
            Mapper = mapper ?? throw new ArgumentNullException();
        }
    }
}
