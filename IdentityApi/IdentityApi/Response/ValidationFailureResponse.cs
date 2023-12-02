namespace Identity.Response;

public class ValidationFailureResponse
{
    public required IEnumerable<ValidationResponse> Errors { get; init; }
}

public class ValidationResponse
{
    public required string PropertyName { get; set; }
    public required string ErrorMessage { get; set; }
}