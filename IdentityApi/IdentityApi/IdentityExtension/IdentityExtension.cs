namespace Identity.IdentityExtension;

public static class IdentityExtension
{
    public static Guid? GetAccountId(this HttpContext context)
    {
        var accountId = context.User.Claims.SingleOrDefault(x => x.Type == "AccountId");

        if (Guid.TryParse(accountId?.Value, out var parsedUserId)) return parsedUserId;

        return null;
    }
}