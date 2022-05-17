using Windows.UI.Popups;

using WinRT.Interop;

namespace WinUIChatClient.Services;

internal class WinUIMessageDialog : IMessageDialog
{
    public async Task ShowMessageAsync(string message)
    {
        MessageDialog dlg = new(message);
        InitializeWithWindow.Initialize(dlg, WindowNative.GetWindowHandle(this));
        await dlg.ShowAsync();
    }
}
