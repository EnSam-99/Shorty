namespace Shorty.Domain.Extensions;

public static class ShortyExtensions
{
    public static string GetShortyCode(this string shortyUrl)
    {
        if (string.IsNullOrWhiteSpace(shortyUrl))
            return string.Empty;

        var lastSegment = shortyUrl.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

        return lastSegment ?? string.Empty;
    }
}