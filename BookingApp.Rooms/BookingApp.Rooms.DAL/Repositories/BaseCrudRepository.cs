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
    public class BaseCrudRepository<TEntity, TDto> : BaseRepository, IBaseCrudRepository<TDto>
       where TEntity : BaseEntityWithId
       where TDto : BaseDtoWithId
    {
        protected BaseCrudRepository(
            IDbContextFactory<RoomsContext> dBContext, IMapper mapper)
            : base(dBContext, mapper)
        {

        }

        public virtual async Task<PagedResults<TDto>> GetListAsync(
            Paginator paginator,
            Expression<Func<TDto, bool>> dtoFilter,
            Expression<Func<TDto, object>> dtoOrderBy,
            bool isAscending)
        {
            await using var context = _dbContext.CreateDbContext();

            var query = context.Set<TEntity>().AsNoTracking().AsQueryable();

            if (dtoFilter != null)
            {
                var entityFilter = dtoFilter.ReplaceParameter<TDto, TEntity>();
                query = query.Where(entityFilter);
            }

            if (dtoOrderBy != null)
            {
                var entityOrderBy = dtoOrderBy.ReplaceParameter<TDto, TEntity>();
                query = isAscending
                   ? query.OrderBy(entityOrderBy)
                   : query.OrderByDescending(entityOrderBy);
            }

            return await Mapper.ProjectTo<TDto>(query)
                .PaginateAsync(paginator)
                .ConfigureAwait(false);
        }

        public virtual async Task<TDto> GetByIdAsync(Guid id)
        {
            await using var context = _dbContext.CreateDbContext();

            var entity = await context.Set<TEntity>().AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id)
                .ConfigureAwait(false);

            return Mapper.Map<TEntity, TDto>(entity);
        }

        public virtual async Task<TDto> GetAsync(Expression<Func<TDto, bool>> dtoFilter, bool asNoTracking)
        {
            if (dtoFilter == null)
                throw new ArgumentNullException(nameof(dtoFilter));

            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<TEntity>();

            var entityFilter = dtoFilter.ReplaceParameter<TDto, TEntity>();
            var queryableItems = asNoTracking ? items.AsNoTracking() : items;
            var item = await queryableItems
                .FirstOrDefaultAsync(entityFilter)
                .ConfigureAwait(false);

            return Mapper.Map<TEntity, TDto>(item);
        }

        public virtual async Task<TDto> AddAsync(TDto item)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<TEntity>();

            var entity = Mapper.Map<TDto, TEntity>(item);
            entity.Id = Guid.NewGuid();

            await items.AddAsync(entity).ConfigureAwait(false);

            await context.SaveChangesAsync().ConfigureAwait(false);

            return Mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> AddAsync<TSpecDto>(TSpecDto item)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<TEntity>();

            var entity = Mapper.Map<TSpecDto, TEntity>(item);
            entity.Id = Guid.NewGuid();

            OnBeforeInsert(entity);

            await items.AddAsync(entity).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            context.Entry(entity).State = EntityState.Detached;

            var result = Mapper.Map<TDto>(entity);

            return result;
        }

        public virtual async Task UpdateAsync(TDto item)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<TEntity>();

            var entity = Mapper.Map<TDto, TEntity>(item);

            OnBeforeUpdate(entity);

            context.Entry(entity).State = EntityState.Modified;
            items.Update(entity);

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task<TDto> SaveAsync(TDto item)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<TEntity>();

            if (!await items.AnyAsync(x => x.Id == item.Id).ConfigureAwait(false))
            {
                return await AddAsync(item).ConfigureAwait(false);
            }
            else
            {
                await UpdateAsync(item).ConfigureAwait(false);
                return item;
            }
        }

        public virtual async Task DeleteAsync(TDto item)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<TEntity>();

            items.Remove(Mapper.Map<TDto, TEntity>(item));

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<TEntity>();

            var entity = await items
                .FirstOrDefaultAsync(item => item.Id == id)
                .ConfigureAwait(false);

            if (entity != null)
            {
                items.Remove(entity);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TDto, bool>> dtoPredicate)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<TEntity>();

            var entityPredicate = dtoPredicate.ReplaceParameter<TDto, TEntity>();
            return await items.AnyAsync(entityPredicate).ConfigureAwait(false);
        }

        protected virtual void OnBeforeInsert(TEntity entity)
        {
        }

        protected virtual void OnBeforeUpdate(TEntity entity)
        {
        }
    }
}
