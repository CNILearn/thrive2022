using Microsoft.UI.Xaml;

using Windows.UI.Popups;

using WinRT.Interop;

namespace WinUIChatClient.Services;

internal class WinUIMessageDialog : IMessageDialog
{
    public async Task ShowMessageAsync(string message)
    {
        MessageDialog dlg = new(message);
        IntPtr hWnd = WindowNative.GetWindowHandle((Application.Current as App).MainWindow);
        InitializeWithWindow.Initialize(dlg, hWnd);
        await dlg.ShowAsync();
    }
}
