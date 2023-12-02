namespace Identity.Request.Token;

public class TokenGenerationRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; } // The refresh token
}