namespace MovieBookingCore.Model.DatabaseEntity.MovieModel;

public class Movie
{
    public Guid MovieId { get; set; }
    public required string MovieTitle { get; set; }
    public required string Description { get; set; }
    public required string Duration { get; set; }
    public DateTime ReleaseDate { get; set; }

    // Navigation properties
    public ICollection<MovieShowing> MovieShowings { get; set; }
}