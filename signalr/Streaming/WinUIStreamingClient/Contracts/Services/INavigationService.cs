using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinUIStreamingClient.Contracts.Services;

public interface INavigationService
{
    event NavigatedEventHandler Navigated;

    bool CanGoBack { get; }

    Frame Frame { get; set; }

    bool NavigateTo(string pageKey, object? parameter = default, bool clearNavigation = false);

    bool GoBack();
}
