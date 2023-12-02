using Identity.Request.Token;
using IdentityCore.Model.Token;

namespace Identity.Mapping;

public static class TokenMapping
{
    public static TokenGeneration MapToTokenGeneration(this TokenGenerationRequest request)
    {
        return new TokenGeneration
        {
            Token = request.Token,
            RefreshToken = request.RefreshToken
        };
    }
}