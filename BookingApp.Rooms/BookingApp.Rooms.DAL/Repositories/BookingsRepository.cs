using AutoMapper;
using BookingApp.Rooms.DAL.Context;
using BookingApp.Rooms.DAL.Entities;
using BookingApp.Rooms.Domain;
using BookingApp.Rooms.Domain.Dtos;
using BookingApp.Rooms.Domain.Repositories;
using BookingApp.Users.DAL.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookingApp.Rooms.DAL.Repositories
{
    public class BookingsRepository : BaseCrudRepository<Booking, BookRoomsDto>, IBookingsRepository
    {
        public BookingsRepository(IDbContextFactory<RoomsContext> dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {

        }

        public async Task<BookRoomsDto> BookRoomsAsync(BookRoomsSpecDto dto)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<Booking>();

            var entity = new Booking
            {
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                UserId = dto.UserId
            };

            entity.Id = Guid.NewGuid();
            entity.BookedRooms = new List<BookedRoom>();
            foreach (var roomId in dto.RoomIds)
            {
                entity.BookedRooms.Add(new BookedRoom
                {
                    Id = Guid.NewGuid(),
                    RoomId = roomId
                });
            }

            await items.AddAsync(entity).ConfigureAwait(false);

            await context.SaveChangesAsync().ConfigureAwait(false);

            return Mapper.Map<BookRoomsDto>(entity);
        }

        public override async Task<BookRoomsDto> GetByIdAsync(Guid id)
        {
            await using var context = _dbContext.CreateDbContext();

            var entity = await context.Set<Booking>()
                .Include(x => x.BookedRooms)
                .ThenInclude(x => x.Room)
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id)
                .ConfigureAwait(false);

            return Mapper.Map<Booking, BookRoomsDto>(entity!);
        }

        public override async Task<PagedResults<BookRoomsDto>> GetListAsync(Paginator paginator, 
            Expression<Func<BookRoomsDto, bool>> dtoFilter, Expression<Func<BookRoomsDto, object>> dtoOrderBy, bool isAscending)
        {
            await using var context = _dbContext.CreateDbContext();

            var query = context.Set<Booking>().AsQueryable();

            if (dtoFilter != null)
            {
                var entityFilter = dtoFilter.ReplaceParameter<BookRoomsDto, Booking>();
                query = query.Where(entityFilter);
            }

            if (dtoOrderBy != null)
            {
                var entityOrderBy = dtoOrderBy.ReplaceParameter<BookRoomsDto, Booking>();
                query = isAscending
                   ? query.OrderBy(entityOrderBy)
                   : query.OrderByDescending(entityOrderBy);
            }

            return await Mapper.ProjectTo<BookRoomsDto>(query)
                .PaginateAsync(paginator)
                .ConfigureAwait(false);
        }
    }
}
