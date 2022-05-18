using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

using System.Collections.ObjectModel;

namespace ChatViewModels;

[ObservableObject]
public partial class GroupChatViewModel
{
    private readonly string _groupUrl;
    private HubConnection? _hubConnection;
    private readonly IMessageDialog _dialogService;

    public GroupChatViewModel(IOptions<SignalROptions> options, IMessageDialog dialogService)
    {
        _dialogService = dialogService;
        _groupUrl = options.Value.GroupChatUrl ?? "https://localhost:5001/groupchat";
    }

    public ObservableCollection<string> Groups { get; } = new();
    public ObservableCollection<string> Messages { get; } = new();

    [ICommand]
    private async Task ConnectAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }

        _hubConnection = new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .WithUrl(_groupUrl)
            .Build();

        _hubConnection.On("MessageToGroup", (string name, string group, string message) =>
        {
            Messages.Add($"{group}-{name}: {message}");
        });

        await _hubConnection.StartAsync();
        await _dialogService.ShowMessageAsync("Connected to SignalR");
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
                await _hubConnection.InvokeAsync("JoinGroup", NewGroup);
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

    [ICommand]
    protected async Task SendMessageAsync()
    {
        if (_hubConnection is null)
        {
            await _dialogService.ShowMessageAsync("Connection not initialized");
            return;
        }
        await _hubConnection.SendAsync("SendMessageToGroup", _name, _selectedGroup, _message);
    }

    [ObservableProperty]
    private string? _newGroup;

    [ObservableProperty]
    private string? _selectedGroup;


    [ObservableProperty]
    protected string? _name;

    [ObservableProperty]
    protected string? _message;
}
