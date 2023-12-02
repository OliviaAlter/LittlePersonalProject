namespace IdentityInfrastructure.Setting;

public class JwtSettings
{
    public string NotTokenKeyForSureSourceTrustMeDude { get; set; }
    public string Issuer { get; set; }
    public List<string> Audiences { get; set; }
}