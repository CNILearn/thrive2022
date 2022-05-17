using ChatViewModels;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;


namespace WinUIChatClient.Views;

public sealed partial class GroupChatUC : UserControl
{
    public GroupChatUC()
    {
        ViewModel = Ioc.Default.GetRequiredService<GroupChatViewModel>();
        InitializeComponent();
    }

    public GroupChatViewModel ViewModel { get; }
}
