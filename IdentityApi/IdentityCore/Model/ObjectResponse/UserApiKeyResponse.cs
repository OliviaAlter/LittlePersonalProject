namespace IdentityCore.Model.ObjectResponse;

public class UserApiKeyResponse
{
    public string Email { get; set; }
    public string Username { get; set; }
    public Guid EndUserId { get; set; }
}