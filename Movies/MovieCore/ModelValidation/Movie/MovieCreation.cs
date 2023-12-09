using System.Text.RegularExpressions;

namespace MovieCore.ModelValidation.Movie;

public partial class MovieCreation
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public DateTime StreamTime { get; set; }
    public string Slug => GenerateSlug();

    private string GenerateSlug()
    {
        var sluggedTitle = SlugRegex().Replace(Title, string.Empty)
            .ToLower().Replace(" ", "-");
        return $"{sluggedTitle}-{ReleaseDate.Year}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}