using System.ComponentModel.DataAnnotations;
using Core.Model.DatabaseEntity.TheaterModel;

namespace Core.Model.DatabaseEntity.SeatModel;

public class Seat
{
    [Key] public Guid SeatId { get; set; }

    public Guid TheaterId { get; set; }
    public required string SeatNumber { get; set; }

    public bool IsAvailable { get; set; }

    // Navigation property
    public Theater Theater { get; set; }
}