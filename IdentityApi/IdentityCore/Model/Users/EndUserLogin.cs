namespace IdentityCore.Model.Users;

public class EndUserLogin
{
    public string Username { get; set; }
    public string Email { get; set; }
    public required string Password { get; set; }
}