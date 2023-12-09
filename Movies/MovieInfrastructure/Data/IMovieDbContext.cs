using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using MovieCore.Model.DatabaseEntity.CommentModel;
using MovieCore.Model.DatabaseEntity.MovieGenreModel;
using MovieCore.Model.DatabaseEntity.MovieModel;

namespace MovieInfrastructure.Data;

public interface IMovieDbContext
{
    // DbSet 
    DbSet<MovieComment> MovieComments { get; set; }
    DbSet<Genres> Genres { get; set; }
    DbSet<MovieGenre> MovieGenres { get; set; }
    DbSet<Movie> Movies { get; set; }
    DbSet<MovieRating> MovieRatings { get; set; }

    // DbContext
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> IsDatabaseAvailableAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    Task RollbackTransactionAsync(IDbContextTransaction transaction);
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}