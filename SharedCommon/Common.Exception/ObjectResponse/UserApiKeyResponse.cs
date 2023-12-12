namespace Common.Exception.ObjectResponse;

public class UserApiKeyResponse
{
    public string Email { get; set; }
    public string Username { get; set; }
    public Guid AccountId { get; set; }
}