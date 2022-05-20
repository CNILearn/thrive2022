
using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using WinUIStreamingClient.Contracts.Services;
using WinUIStreamingClient.ViewModels;
using WinUIStreamingClient.Views;

namespace WinUIStreamingClient.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();

    public PageService()
    {
        Configure<MainViewModel, MainPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
       // where VM : ObservableObject  // with the source code generator, this base class is no longer used
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName ?? throw new InvalidOperationException();
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
