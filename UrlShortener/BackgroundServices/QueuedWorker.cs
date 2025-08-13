namespace UrlShortener.BackgroundServices
{
    // QueuedWorker.cs creates a scope per job
    public sealed class QueuedWorker : BackgroundService
    {
        private readonly IBackgroundAnalyticsQueue _queue;
        private readonly ILogger<QueuedWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public QueuedWorker(
            IBackgroundAnalyticsQueue queue,
            ILogger<QueuedWorker> logger,
            IServiceScopeFactory scopeFactory)
        {
            _queue = queue;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("QueuedWorker started");
            while (!stoppingToken.IsCancellationRequested)
            {
                Func<IServiceProvider, CancellationToken, Task> workItem;
                try
                {
                    workItem = await _queue.DequeueAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    await workItem(scope.ServiceProvider, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background job failed");
                    // TODO: add retry/ DLQ as needed
                }
            }
            _logger.LogInformation("QueuedWorker stopping");
        }
    }

}
