using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

using System.Collections.ObjectModel;

namespace ChatViewModels;

public partial class GroupChatViewModel : ChatViewModel
{
    private readonly string _groupUrl;

    public GroupChatViewModel(IOptions<SignalROptions> options, IMessageDialog dialogService)
        : base(options, dialogService)
    {
        _groupUrl = options.Value.GroupChatUrl ?? "https://localhost:5001/groupchat";
    }

    public ObservableCollection<string> Groups { get; } = new();

    protected override async Task ConnectSignalRAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }

        _hubConnection = new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .WithUrl(_groupUrl)
            .Build();

        _hubConnection.On("MessageToGroup", (string group, string name, string message) =>
        {
            Messages.Add($"{group}-{name}: {message}");
        });

        await _hubConnection.StartAsync();
    }

    [ICommand]
    public async Task EnterGroup()
    {
        if (_hubConnection is null)
        {
            await _dialogService.ShowMessageAsync("Connection not initialized");
            return;
        }

        try
        {
            if (NewGroup is not null)
            {
                await _hubConnection.InvokeAsync("AddGroup", NewGroup);
                Groups.Add(NewGroup);
                SelectedGroup = NewGroup;
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowMessageAsync(ex.Message);
        }
    }

    [ICommand]
    public async Task LeaveGroup()
    {
        if (_hubConnection is null)
        {
            await _dialogService.ShowMessageAsync("Connection not initialized");
            return;
        }

        try
        {
            if (SelectedGroup is not null)
            {
                await _hubConnection.InvokeAsync("LeaveGroup", SelectedGroup);
                Groups.Remove(SelectedGroup);
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowMessageAsync(ex.Message);
        }
    }

    [ObservableProperty]
    private string? _newGroup;

    [ObservableProperty]
    private string? _selectedGroup;

}
