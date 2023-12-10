using System.ComponentModel.DataAnnotations;
using MovieBookingCore.Model.DatabaseEntity.MovieModel;

namespace MovieBookingCore.Model.DatabaseEntity.TheaterModel;

public class TheaterHall
{
    [Key] public Guid TheaterHallId { get; set; }

    public Guid TheaterId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int NumberOfSeats { get; set; }

    // Navigation properties
    public Theater Theater { get; set; }
    public ICollection<MovieShowing> Showings { get; set; }
}