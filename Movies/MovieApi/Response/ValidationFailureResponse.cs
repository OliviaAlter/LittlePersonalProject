namespace MovieApi.Response;

public class ValidationFailureResponse
{
    public required IEnumerable<ValidationResponse> Errors { get; init; }
}