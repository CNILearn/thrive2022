
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using WinUIStreamingClient.Activation;
using WinUIStreamingClient.Contracts.Services;

namespace WinUIStreamingClient.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly INavigationService _navigationService;
    private UIElement? _shell = default;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, INavigationService navigationService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _navigationService = navigationService;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Initialize services that you need before app activation
        // take into account that the splash screen is shown while this code runs.
        await InitializeAsync();

        if (App.MainWindow.Content == null)
        {
            App.MainWindow.Content = _shell ?? new Frame();
        }

        // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
        // will navigate to the first page
        await HandleActivationAsync(activationArgs);

        // Ensure the current window is active
        App.MainWindow.Activate();

        // Tasks after activation
        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers
                                            .FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await Task.CompletedTask;
    }
}
