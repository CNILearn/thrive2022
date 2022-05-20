using Microsoft.UI.Xaml.Controls;

using WinUIStreamingClient.ViewModels;

namespace WinUIStreamingClient.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
        ViewModel.InitCanvas(canvas1);
    }
}
