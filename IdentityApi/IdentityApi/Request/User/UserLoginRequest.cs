namespace Identity.Request.User;

public class UserLoginRequest
{
    public required string EmailOrUsername { get; set; }
    public required string Password { get; set; }
}