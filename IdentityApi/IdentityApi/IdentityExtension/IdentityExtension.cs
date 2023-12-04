namespace Identity.IdentityExtension;

public static class IdentityExtension
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var userId = context.User.Claims.SingleOrDefault(x => x.Type == "userid");

        if (Guid.TryParse(userId?.Value, out var parsedUserId)) return parsedUserId;

        return null;
    }
}