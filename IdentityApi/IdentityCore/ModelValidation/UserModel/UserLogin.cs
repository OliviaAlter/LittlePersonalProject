namespace IdentityCore.ModelValidation.UserModel;

public class UserLogin
{
    public string Username { get; set; }
    public string Email { get; set; }
    public required string Password { get; set; }
}