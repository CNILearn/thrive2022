using Microsoft.AspNetCore.SignalR.Client;

using System.Runtime.CompilerServices;

namespace WinUIStreamingClient.Core.Contracts.Services;

public class SensorClient
{
    private HubConnection _hubConnection;
    public SensorClient()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .WithUrl("https://localhost:5001/stream")
            .Build();
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await _hubConnection.StartAsync(cancellationToken);
    }

    public async IAsyncEnumerable<SensorData> StreamAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var data in _hubConnection.StreamAsync<SensorData>("GetSensorData").WithCancellation(cancellationToken))
        {
            yield return data;
        };
    }
}

public record SensorData(int Number, int Value, DateTime TimeStamp);