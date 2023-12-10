using System.ComponentModel.DataAnnotations;
using MovieBookingCore.Model.DatabaseEntity.BookingModel;
using MovieBookingCore.Model.DatabaseEntity.TheaterModel;

namespace MovieBookingCore.Model.DatabaseEntity.MovieModel;

public class MovieShowing
{
    [Key] public int MovieShowingId { get; set; }

    public Guid MovieId { get; set; }

    public Guid TheaterSlotId { get; set; }

    // Navigation properties
    public Movie Movie { get; set; }
    public TheaterHall Hall { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}