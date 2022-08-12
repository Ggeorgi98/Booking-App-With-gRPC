using BookingApp.Users.Domain.Dtos;
using System.Linq.Expressions;

namespace BookingApp.Users.Domain.Services
{
    public interface IBaseCrudService<TDto> : IBaseService
        where TDto : BaseDtoWithId
    {
        Task<TDto> AddAsync(TDto item);

        Task<TDto> AddAsync<TSpecDto>(TSpecDto item);

        Task<bool> AnyAsync(Expression<Func<TDto, bool>> predicate);

        Task DeleteAsync(TDto item);

        Task DeleteAsync(Guid id);

        Task<TDto> GetByIdAsync(Guid id);

        Task<TDto> GetAsync(Expression<Func<TDto, bool>> dtoFilter, bool asNoTracking = true);

        Task<PagedResults<TDto>> GetListAsync(Paginator paginator,
            Expression<Func<TDto, bool>> filter,
            Expression<Func<TDto, object>> orderBy,
            bool ascending = true);

        Task<TDto> SaveAsync(TDto item);

        Task UpdateAsync(TDto item);
    }
}
