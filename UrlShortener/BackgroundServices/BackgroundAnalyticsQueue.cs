using System.Threading.Channels;

namespace UrlShortener.BackgroundServices
{
    public class BackgroundAnalyticsQueue : IBackgroundAnalyticsQueue   {
        private readonly Channel<Func<IServiceProvider, CancellationToken, Task>> _queue;

        public BackgroundAnalyticsQueue(int capacity = 500)
        {
            _queue = Channel.CreateBounded<Func<IServiceProvider, CancellationToken, Task>>(
                new BoundedChannelOptions(capacity)
                {
                    FullMode = BoundedChannelFullMode.Wait,
                    SingleReader = true,
                    SingleWriter = false
                });
        }

        public ValueTask EnqueueAsync(Func<IServiceProvider, CancellationToken, Task> workItem)
        {
            if (workItem is null) throw new ArgumentNullException(nameof(workItem));
            return _queue.Writer.WriteAsync(workItem);
        }

        public ValueTask<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(CancellationToken ct)
            => _queue.Reader.ReadAsync(ct);
    }

    
}
