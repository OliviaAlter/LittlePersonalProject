using System.ComponentModel.DataAnnotations;

namespace MovieBookingCore.Model.DatabaseEntity.TheaterModel;

public class Theater
{
    [Key] public Guid TheaterId { get; set; }

    public required string TheaterName { get; set; }

    // Navigation properties
    public ICollection<TheaterHall> Halls { get; set; }
}