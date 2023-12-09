using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MovieCore.Model.DatabaseEntity.CommentModel;
using MovieCore.Model.DatabaseEntity.MovieGenreModel;
using MovieCore.Model.DatabaseEntity.MovieModel;

namespace MovieInfrastructure.Data;

public class MovieDbContext : DbContext, IMovieDbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    {
    }

    public async Task<bool> IsDatabaseAvailableAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Check if the database can be connected to
            var canConnect = await Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
                return false;

            // Check if the database exists
            var databaseExists = await Database.GetService<IRelationalDatabaseCreator>()
                .ExistsAsync(cancellationToken);

            return databaseExists;
        }
        catch
        {
            // Handle or log the exception as needed
            return false;
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }

    public DbSet<MovieComment> MovieComments { get; set; }
    public DbSet<Genres> Genres { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieRating> MovieRatings { get; set; }
}