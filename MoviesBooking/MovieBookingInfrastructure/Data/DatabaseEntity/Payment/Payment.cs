using MovieBookingInfrastructure.Data.DatabaseEntity.Booking;

namespace MovieBookingInfrastructure.Data.DatabaseEntity.Payment;

public class Payment
{
    public Guid PaymentId { get; init; }
    public Guid AccountId { get; init; }
    public Guid MovieId { get; init; }
    public List<string> Seats { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } = "Unpaid";
    public decimal Amount { get; set; }
    public Guid BookingId { get; set; }
    public MovieBookings Booking { get; set; }
}