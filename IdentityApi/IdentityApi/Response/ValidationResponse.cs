namespace Identity.Response;

public class ValidationResponse
{
    public required string PropertyName { get; set; }
    public required string ErrorMessage { get; set; }
}