using ChatViewModels;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;


namespace WinUIChatClient.Views;

public sealed partial class ChatUC : UserControl
{
    public ChatUC()
    {
        ViewModel = Ioc.Default.GetRequiredService<ChatViewModel>();
        InitializeComponent();
    }

    public ChatViewModel ViewModel { get; }
}
