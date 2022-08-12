using BookingApp.Users.Domain;
using BookingApp.Users.Domain.Dtos;
using BookingApp.Users.Domain.Repositories;
using BookingApp.Users.Domain.Services;
using System.Linq.Expressions;

namespace BookingApp.Users.DomainServices
{
    public class BaseCrudService<TDto> : BaseService, IBaseCrudService<TDto>
        where TDto : BaseDtoWithId
    {
        protected IBaseCrudRepository<TDto> Repository { get; }

        public BaseCrudService(IBaseCrudRepository<TDto> repository)
        {
            Repository = repository;
        }

        public virtual async Task<PagedResults<TDto>> GetListAsync(
            Paginator paginator,
            Expression<Func<TDto, bool>> filter,
            Expression<Func<TDto, object>> orderBy,
            bool ascending = true)
        {
            return await Repository
                .GetListAsync(paginator, filter, orderBy, ascending)
                .ConfigureAwait(false);
        }

        public virtual async Task<TDto> GetByIdAsync(Guid id)
        {
            return await Repository
                .GetByIdAsync(id)
                .ConfigureAwait(false);
        }

        public virtual async Task<TDto> GetAsync(Expression<Func<TDto, bool>> dtoFilter, bool asNoTracking = true)
        {
            return await Repository
                .GetAsync(dtoFilter, asNoTracking)
                .ConfigureAwait(false);
        }

        public virtual async Task<TDto> AddAsync(TDto item)
        {
            return await Repository
                .AddAsync(item)
                .ConfigureAwait(false);
        }

        public virtual async Task<TDto> AddAsync<TSpecDto>(TSpecDto item)
        {
            return await Repository
                .AddAsync<TSpecDto>(item)
                .ConfigureAwait(false);
        }

        public virtual async Task<TDto> SaveAsync(TDto item)
        {
            return await Repository
                .SaveAsync(item)
                .ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TDto item)
        {
            await Repository
                .UpdateAsync(item)
                .ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(TDto item)
        {
            await Repository
                .DeleteAsync(item)
                .ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await Repository
                .DeleteAsync(id)
                .ConfigureAwait(false);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TDto, bool>> predicate)
        {
            return await Repository
                .AnyAsync(predicate)
                .ConfigureAwait(false);
        }
    }
}
