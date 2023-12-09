using System.ComponentModel.DataAnnotations;

namespace MovieCore.Model.DatabaseEntity.MovieModel;

public class MovieRating
{
    [Key] public Guid MovieRatingId { get; init; }

    public Guid MovieId { get; set; }
    public Movie Movie { get; set; }

    public Guid UserId { get; set; }
    public decimal Rating { get; init; }
}