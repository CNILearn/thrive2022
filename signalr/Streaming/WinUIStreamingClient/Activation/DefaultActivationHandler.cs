
using Microsoft.UI.Xaml;

using WinUIStreamingClient.Contracts.Services;
using WinUIStreamingClient.ViewModels;

namespace WinUIStreamingClient.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        string fullName = typeof(MainViewModel).FullName ?? throw new InvalidOperationException();
        _navigationService.NavigateTo(fullName, args.Arguments);
        await Task.CompletedTask;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the app activation
        return _navigationService.Frame.Content == null;
    }
}
