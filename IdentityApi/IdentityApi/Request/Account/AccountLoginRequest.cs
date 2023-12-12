namespace Identity.Request.Account;

public class AccountLoginRequest
{
    public required string EmailOrUsername { get; set; }
    public required string Password { get; set; }
}