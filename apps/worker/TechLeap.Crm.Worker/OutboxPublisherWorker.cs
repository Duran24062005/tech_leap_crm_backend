namespace TechLeap.Crm.Worker;

public sealed class OutboxPublisherWorker(
    ILogger<OutboxPublisherWorker> logger,
    IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalSeconds = Math.Max(1, configuration.GetValue("Worker:PollIntervalSeconds", 15));
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(intervalSeconds));

        logger.LogInformation("Outbox worker started with a {IntervalSeconds}s polling interval.", intervalSeconds);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            logger.LogDebug("Outbox polling cycle completed. Azure Service Bus publishing will be enabled in the integrations sprint.");
        }
    }
}
