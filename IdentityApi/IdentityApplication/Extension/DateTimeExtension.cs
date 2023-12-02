namespace IdentityApplication.Extension;

public static class DateTimeExtension
{
    public static string FormatDateTime(this DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy");
    }
}