using System.ComponentModel.DataAnnotations;
using Core.Model.DatabaseEntity.MovieModel;
using Core.Model.DatabaseEntity.SeatModel;

namespace Core.Model.DatabaseEntity.BookingModel;

public class Booking
{
    [Key]
    public Guid BookingId { get; set; }
    public Guid AccountId { get; set; } 
    public Guid ShowingId { get; set; }
    public DateTime BookingTime { get; set; }
    public string PaymentMethod { get; set; }

    // Navigation properties
    public MovieShowing Showing { get; set; }
    public ICollection<BookedSeat> BookedSeats { get; set; }
}