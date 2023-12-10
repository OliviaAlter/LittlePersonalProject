using System.ComponentModel.DataAnnotations;
using MovieBookingCore.Model.DatabaseEntity.TheaterModel;

namespace MovieBookingCore.Model.DatabaseEntity.SeatModel;

public class Seat
{
    [Key] public Guid SeatId { get; set; }
    public Guid TheaterHallId { get; set; }
    public required string SeatNumber { get; set; }

    public bool IsReserved { get; set; }

    // Navigation property
    public TheaterHall Hall { get; set; }
}