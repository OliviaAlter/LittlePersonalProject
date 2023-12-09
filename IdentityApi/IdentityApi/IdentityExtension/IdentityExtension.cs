namespace Identity.IdentityExtension;

public static class IdentityExtension
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var accountId = context.User.Claims.SingleOrDefault(x => x.Type == "userid");

        if (Guid.TryParse(accountId?.Value, out var parsedUserId)) return parsedUserId;

        return null;
    }
}