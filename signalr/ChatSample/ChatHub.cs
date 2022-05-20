using System.Text.Encodings.Web;

namespace ChatSample;

public class ChatHub : Hub
{
    public async Task SendMessage(string name, string message)
    {
        string encName = HtmlEncoder.Default.Encode(name);
        string encMessage = HtmlEncoder.Default.Encode(message);
        await Clients.All.SendAsync("MessageToAll", name, encMessage);
    }
}
