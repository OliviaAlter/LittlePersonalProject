using System.Linq.Expressions;

namespace IdentityCore.RepositoryInterface.Generic;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
    Task<T> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity, Expression<Func<T, bool>>? duplicateCriteria = null);
    Task UpdatePartialAsync(T entity, params Expression<Func<T, object>>[] updatedProperties);
    Task DeleteAsync(T entity);
}