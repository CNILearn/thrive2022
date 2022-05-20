using System.Text.Encodings.Web;

namespace ChatSample;

public class GroupChatHub : Hub
{
    private readonly GroupNamesService _groupNamesService;

    public GroupChatHub(GroupNamesService groupNamesService)
    {
        _groupNamesService = groupNamesService;
    }

    public async Task JoinGroup(string group)
    {
        string encGroup = HtmlEncoder.Default.Encode(group);
        await Groups.AddToGroupAsync(Context.ConnectionId, encGroup);
    }

    public async Task GetGroups()
    {
        var groups = _groupNamesService.GetGroups();
        await Clients.Caller.SendAsync("GroupNames", groups);
    }

    public async Task LeaveGroup(string group)
    {
        string encGroup = HtmlEncoder.Default.Encode(group);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
    }

    public async Task SendMessageToGroup(string name, string group, string message)
    {
        string encName = HtmlEncoder.Default.Encode(name);
        string encMessage = HtmlEncoder.Default.Encode(message);
        string encGroup = HtmlEncoder.Default.Encode(group);
        await Clients.Group(group).SendAsync("MessageToGroup", encName, encGroup, encMessage);
    }
}
