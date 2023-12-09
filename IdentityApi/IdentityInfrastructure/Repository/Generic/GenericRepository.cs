using System.Linq.Expressions;
using IdentityCore.RepositoryInterface.Generic;
using IdentityInfrastructure.Data;
using IdentityInfrastructure.DatabaseException;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IdentityInfrastructure.Repository.Generic;

public class GenericRepository<T>(IAuthIdentityDbContext context) : IGenericRepository<T>
    where T : class
{
    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }
        catch (SqlException ex)
        {
            throw new DatabaseOperationException("Database operation failed.", ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while finding entities.", ex);
        }
    }

    public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await context.Set<T>().FindAsync(predicate)
                   ?? throw new EntityNotFoundException("Entity not found.");
        }
        catch (SqlException ex)
        {
            throw new DatabaseOperationException("Database operation failed.", ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while finding entities.", ex);
        }
    }

    public async Task AddAsync(T entity, Expression<Func<T, bool>>? duplicateCriteria = null)
    {
        try
        {
            if (duplicateCriteria is not null && await context.Set<T>().AnyAsync(duplicateCriteria))
                throw new InvalidOperationException("Duplicate entry detected.");

            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Handle specific database update exceptions, e.g., duplicate entry
            if (IsDuplicateEntryException(ex))
                throw new DuplicateEntityException("A duplicate entry detected.", ex);
            throw; // Rethrow other DbUpdateExceptions
        }
        catch (SqlException ex)
        {
            // Handle SQL-specific exceptions
            // Log and/or rethrow as a custom exception
            throw new DatabaseOperationException("Database operation failed.", ex);
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            throw new RepositoryException("An error occurred in the repository.", ex);
        }
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        try
        {
            var guidId = Guid.TryParse(id.ToString(), out var guid)
                ? guid
                : Guid.Empty;

            return await context.Set<T>()
                .FindAsync(guidId);
        }

        catch (SqlException ex)
        {
            throw new DatabaseOperationException("Database operation failed.", ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching the entity.", ex);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException("An error occurred while deleting the entity.", ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred in the repository.", ex);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await context.Set<T>().ToListAsync();
        }
        catch (SqlException ex)
        {
            throw new DatabaseOperationException("Database operation failed.", ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred while fetching entities.", ex);
        }
    }

    public async Task UpdatePartialAsync(T entity, params Expression<Func<T, object>>[] updatedProperties)
    {
        try
        {
            var dbEntityEntry = context.Entry(entity);
            foreach (var property in updatedProperties) dbEntityEntry.Property(property).IsModified = true;
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException("An error occurred while updating the entity.", ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An error occurred in the repository.", ex);
        }
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().AnyAsync(predicate);
    }

    private static bool IsDuplicateEntryException(Exception ex)
    {
        const int duplicateSqlErrorCode = 2627; // SQL Server error code for duplicate key
        var sqlException = ex.GetBaseException() as SqlException;

        return sqlException is { Number: duplicateSqlErrorCode };
    }
}