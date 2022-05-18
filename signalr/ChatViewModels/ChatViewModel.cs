using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

using System.Collections.ObjectModel;

namespace ChatViewModels;

[ObservableObject]
public partial class ChatViewModel
{
    private HubConnection? _hubConnection;
    private string _chatUrl;
    private readonly IMessageDialog _dialogService;

    public ChatViewModel(IOptions<SignalROptions> options, IMessageDialog dialogService)
    {
        _dialogService = dialogService;
        _chatUrl = options.Value.ChatUrl ?? "https://localhost:5001/chat";
    }

    public ObservableCollection<string> Messages { get; } = new ();

    [ICommand]
    private async Task ConnectAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }

        _hubConnection = new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .WithUrl(_chatUrl)
            .Build();

        _hubConnection.On("MessageToAll", (string name, string message) =>
        {
            Messages.Add($"{name}: {message}");
        });

        await _hubConnection.StartAsync();
        await _dialogService.ShowMessageAsync("Connected to SignalR");
    }

    [ICommand]
    protected async Task SendMessageAsync()
    {
        if (_hubConnection is null)
        {
            await _dialogService.ShowMessageAsync("Connection not initialized");
            return;
        }
        await _hubConnection.SendAsync("SendMessage", _name, _message);
    }

    [ObservableProperty]
    protected string? _name;

    [ObservableProperty]
    protected string? _message;

}
