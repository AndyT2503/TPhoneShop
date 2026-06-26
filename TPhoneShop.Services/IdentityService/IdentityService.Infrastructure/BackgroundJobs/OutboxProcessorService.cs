using BuildingBlocks.Domain.Constants;
using IdentityService.Infrastructure.Messaging.RabbitMQ;
using IdentityService.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.BackgroundJobs
{
    public class OutboxProcessorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxProcessorService> _logger;
        public OutboxProcessorService(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessorService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(OutboxProcessorService)} is running");
                await Process(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task Process(CancellationToken cancellationToken)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();

            var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var dispatcher = scope.ServiceProvider.GetRequiredService<EventDispatcher>();

            var messages = await db.OutboxMessages
                .Where(x => x.Status == OutboxStatus.Pending)
                .OrderBy(x => x.CreatedAt)
                .Take(50)
                .ToListAsync(cancellationToken);

            foreach (var msg in messages)
            {
                try
                {
                    if (msg.ExpiresAt < DateTimeOffset.UtcNow)
                    {
                        msg.Status = OutboxStatus.Expired;
                        continue;
                    }
                    _logger.LogInformation("Message {id} is dispatching", new List<string>() { msg.Id.ToString() });
                    await dispatcher.DispatchAsync(msg);

                    msg.Status = OutboxStatus.Processed;
                    msg.ProcessedAt = DateTimeOffset.UtcNow;
                }
                catch (Exception ex)
                {
                    msg.RetryCount++;
                    msg.Error = ex.ToString();

                    if (msg.RetryCount >= 10)
                        msg.Status = OutboxStatus.Failed;
                }
            }

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
