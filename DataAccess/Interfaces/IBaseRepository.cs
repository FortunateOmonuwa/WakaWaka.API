using System.Linq.Expressions;
using WakaWaka.API.Domain.Models.restaurant;

namespace WakaWaka.API.DataAccessLayer.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> GetByIDAsync (int id);
        Task<T> CreateAsync(T entity);
        Task<IQueryable<T>> GetAllAsync ();
        Task<T> UpdateAsync(T entity, int entityId);
        Task<bool> DeleteAsync (int id);

        Task<IQueryable<T>> GetAllFilteredAsync(Expression<Func<T, bool>> filter);
        Task<IQueryable<T>> GetAllPagedAsync(int pageNumber, int pageSize);
        Task<IQueryable<T>> GetAllSortedAsync(Expression<Func<T, object>> searchQuery);
       // Task<IQueryable<T>> GetSortedByQueryAsync(Expression<Func<T, object>>? searchQuery, string orderBy);
        Task<IEnumerable<T>> CreateMultipleAsync(IEnumerable<T> entities);
    }
}
