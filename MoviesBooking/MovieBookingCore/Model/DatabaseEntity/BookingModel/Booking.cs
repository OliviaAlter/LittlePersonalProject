using System.ComponentModel.DataAnnotations;
using MovieBookingCore.Model.DatabaseEntity.MovieModel;
using MovieBookingCore.Model.DatabaseEntity.SeatModel;

namespace MovieBookingCore.Model.DatabaseEntity.BookingModel;

public class Booking
{
    [Key] public Guid BookingId { get; set; }

    public Guid AccountId { get; set; }
    public Guid ShowingId { get; set; }
    public DateTime BookingTime { get; set; }
    public string PaymentMethod { get; set; }

    // Navigation properties
    public MovieShowing Showing { get; set; }
    public ICollection<BookedSeat> BookedSeats { get; set; }
}