using System.ComponentModel.DataAnnotations;
using MovieCore.Model.DatabaseEntity.MovieModel;

namespace MovieCore.Model.DatabaseEntity.CommentModel;

public class MovieComment
{
    [Key] public int MovieCommentId { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public Guid UserId { get; set; }
    public required string CommentTextArea { get; set; }
    public DateTime CommentDate { get; set; }
}