using BookingApp.Users.Domain.Dtos;
using System.Linq.Expressions;

namespace BookingApp.Users.Domain.Repositories
{
    public interface IBaseCrudRepository<TDto> : IBaseRepository
        where TDto : BaseDtoWithId
    {
        Task<TDto> AddAsync(TDto item);

        Task<TDto> AddAsync<TSpecDto>(TSpecDto item);

        Task<bool> AnyAsync(Expression<Func<TDto, bool>> dtoPredicate);

        Task DeleteAsync(TDto item);

        Task DeleteAsync(Guid id);

        Task<TDto> GetAsync(Expression<Func<TDto, bool>> dtoFilter, bool asNoTracking = true);

        Task<TDto> GetByIdAsync(Guid id);

        Task<PagedResults<TDto>> GetListAsync(Paginator paginator,
            Expression<Func<TDto, bool>> dtoFilter,
            Expression<Func<TDto, object>> dtoOrderBy,
            bool isAscending = true);

        Task<TDto> SaveAsync(TDto item);

        Task UpdateAsync(TDto item);
    }
}
