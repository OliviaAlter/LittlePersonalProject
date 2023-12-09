using Core.Model.DatabaseEntity.MovieModel;

namespace Core.Model.DatabaseEntity.TheaterModel;

public class TheaterSlot
{
    public Guid TheaterSlotId { get; set; }
    public Guid TheaterId { get; set; }
    public Theater Theater { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    // Navigation properties
    public ICollection<MovieShowing> Showings { get; set; }
}