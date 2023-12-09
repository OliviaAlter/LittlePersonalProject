using System.ComponentModel.DataAnnotations;

namespace MovieCore.Model.DatabaseEntity.MovieGenreModel;

public class Genres
{
    [Key] public Guid GenresId { get; set; }

    public required string GenresName { get; set; }

    public List<MovieGenre> MovieGenres { get; set; }
}