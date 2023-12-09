using System.ComponentModel.DataAnnotations;
using MovieCore.Model.DatabaseEntity.MovieModel;

namespace MovieCore.Model.DatabaseEntity.MovieGenreModel;

public class MovieGenre
{
    [Key] public Guid MovieGenresId { get; set; }

    public Guid GenresId { get; set; }
    public Genres Genre { get; set; }

    public Guid MovieId { get; set; }
    public Movie Movie { get; set; }
}