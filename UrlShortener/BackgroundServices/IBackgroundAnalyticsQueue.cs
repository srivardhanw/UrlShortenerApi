namespace UrlShortener.BackgroundServices
{
    public interface IBackgroundAnalyticsQueue
    {
        ValueTask EnqueueAsync(Func<IServiceProvider, CancellationToken, Task> workItem);
        ValueTask<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(CancellationToken ct);
    }

}
