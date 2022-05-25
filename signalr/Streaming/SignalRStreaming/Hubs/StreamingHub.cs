using Microsoft.AspNetCore.SignalR;

using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace SignalRStreaming.Hubs;

public record SensorData(int Number, int Value, DateTime TimeStamp);

public class StreamingHub : Hub
{
    // v1 - using ChannelReader
    public ChannelReader<SensorData> GetSomeDataWithChannelReader(
        int count,
        int delay,
        CancellationToken cancellationToken)
    {
        var channel = Channel.CreateUnbounded<SensorData>();
        _ = WriteItemsAsync(channel.Writer, count, delay, cancellationToken);

        return channel.Reader;
    }

    private async Task WriteItemsAsync(
       ChannelWriter<SensorData> writer,
       int count,
       int delay,
       CancellationToken cancellationToken)
    {
        try
        {
            for (var i = 0; i < count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await writer.WriteAsync(new SensorData(i, Random.Shared.Next(40), DateTime.Now));

                await Task.Delay(delay, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            writer.TryComplete(ex);
        }

        writer.TryComplete();
    }

    // v2 async streams
    public async IAsyncEnumerable<SensorData> GetSensorData([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        for (int i = 0; i < 1000; i++)
        {
            yield return new SensorData(i, Random.Shared.Next(20, 40), DateTime.Now);
            await Task.Delay(100, cancellationToken);
        }
    }
}
