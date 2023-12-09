using System.ComponentModel.DataAnnotations;
using Core.Model.DatabaseEntity.BookingModel;

namespace Core.Model.DatabaseEntity.SeatModel;

public class BookedSeat
{
    [Key]
    public Guid BookedSeatId { get; set; }
    public Guid SeatId { get; set; }
    // Navigation properties
    public Booking Booking { get; set; }
    public Seat Seat { get; set; }
}