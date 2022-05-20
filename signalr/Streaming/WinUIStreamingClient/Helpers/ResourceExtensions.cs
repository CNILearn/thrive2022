using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUIStreamingClient.Helpers;

internal static class ResourceExtensions
{
    private static ResourceLoader _resourceLoader = new();

    public static string GetLocalized(this string resourceKey)
    {
        return _resourceLoader.GetString(resourceKey);
    }
}
