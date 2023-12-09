using System.ComponentModel.DataAnnotations;
using MovieCore.Model.DatabaseEntity.CommentModel;
using MovieCore.Model.DatabaseEntity.MovieGenreModel;

namespace MovieCore.Model.DatabaseEntity.MovieModel;

public class Movie
{
    [Key] public required Guid MovieId { get; init; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public string Slug { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime StreamTime { get; set; }
    public int? UserRating { get; set; }
    public float? Rating { get; set; }

    public List<MovieGenre> MovieGenres { get; set; }
    public List<MovieRating> Ratings { get; set; }
    public List<MovieComment> Comments { get; set; }
}