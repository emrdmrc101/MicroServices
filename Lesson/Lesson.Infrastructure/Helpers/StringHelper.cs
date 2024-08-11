namespace Lesson.Infrastructure.Helpers;

public static class StringHelper
{
    public static Guid ToGuid(this string? value)
    {
        var isGuid = Guid.TryParse(value, out Guid outValue);

        return isGuid ? outValue : Guid.Empty;
    }
}