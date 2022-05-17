global using ChatViewModels;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

using WinUIChatClient.Services;

namespace WinUIChatClient;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var settings = new ConfigurationManager()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        Ioc.Default.ConfigureServices(
            new ServiceCollection()
                .Configure<SignalROptions>(config =>
                {
                    config.ChatUrl = settings["ChatUrl"];
                    config.GroupChatUrl = settings["GroupChatUrl"];
                })
                .AddScoped<ChatViewModel>()
                .AddScoped<GroupChatViewModel>()
                .AddTransient<IMessageDialog, WinUIMessageDialog>()
                .BuildServiceProvider());
        _window = new MainWindow();
        _window.Activate();
    }

    private Window _window;
}
    