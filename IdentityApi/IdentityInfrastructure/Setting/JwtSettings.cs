namespace IdentityInfrastructure.Setting;

public class JwtSettings
{
    public string NotTokenKeyForSureSourceTrustMeDude { get; }
    public string Issuer { get; }
    public List<string> Audiences { get; }
}