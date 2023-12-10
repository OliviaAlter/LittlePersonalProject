using System.ComponentModel.DataAnnotations;
using MovieBookingCore.Model.DatabaseEntity.BookingModel;

namespace MovieBookingCore.Model.DatabaseEntity.SeatModel;

public class BookedSeat
{
    [Key] public Guid BookedSeatId { get; set; }

    public Guid SeatId { get; set; }

    // Navigation properties
    public Booking Booking { get; set; }
    public Seat Seat { get; set; }
}