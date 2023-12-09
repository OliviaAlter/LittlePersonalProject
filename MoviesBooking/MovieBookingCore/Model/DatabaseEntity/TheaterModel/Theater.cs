using System.ComponentModel.DataAnnotations;
using Core.Model.DatabaseEntity.SeatModel;

namespace Core.Model.DatabaseEntity.TheaterModel;

public class Theater
{
    [Key] public Guid TheaterId { get; set; }

    public required string TheaterName { get; set; }
    public ICollection<TheaterSlot> TheaterSlots { get; set; }
    public ICollection<Seat> Seats { get; set; }
}