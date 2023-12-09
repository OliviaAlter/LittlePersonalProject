using System.ComponentModel.DataAnnotations;
using Core.Model.DatabaseEntity.BookingModel;
using Core.Model.DatabaseEntity.TheaterModel;

namespace Core.Model.DatabaseEntity.MovieModel;

public class MovieShowing
{
    [Key]
    public int MovieShowingId { get; set; }
    public Guid MovieId { get; set; }
    public Guid TheaterSlotId { get; set; }
    // Navigation properties
    public Movie Movie { get; set; }
    public TheaterSlot Slot { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}