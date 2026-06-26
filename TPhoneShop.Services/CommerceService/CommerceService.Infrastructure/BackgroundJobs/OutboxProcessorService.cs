using BuildingBlocks.Domain.Constants;
using CommerceService.Application.Common.Events;
using CommerceService.Domain.Entities;
using CommerceService.Domain.Events.Abstractions;
using CommerceService.Domain.Events.Product;
using CommerceService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CommerceService.Infrastructure.BackgroundJobs
{
    public class OutboxProcessorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxProcessorService> _logger;
        private readonly IMediator _mediator;
        public OutboxProcessorService(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessorService> logger, IMediator mediator)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _mediator = mediator;
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

            var db = scope.ServiceProvider.GetRequiredService<CommerceDbContext>();

            var messages = await db.OutboxMessages
                .Where(x => x.Status == OutboxStatus.Pending)
                .OrderBy(x => x.CreatedAt)
                .Take(50)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                await ProcessMessageAsync(message, cancellationToken);
            }

            await db.SaveChangesAsync(cancellationToken);
        }

        private async Task ProcessMessageAsync(OutboxMessage message, CancellationToken cancellationToken)
        {
            try
            {
                if (IsExpired(message))
                {
                    message.Status = OutboxStatus.Expired;
                    return;
                }

                _logger.LogInformation("Dispatching outbox message {MessageId}", message.Id);

                await PublishEventAsync(message, cancellationToken);

                message.Status = OutboxStatus.Processed;
                message.ProcessedAt = DateTimeOffset.UtcNow;
            }
            catch (Exception ex)
            {
                HandleFailure(message, ex);
            }
        }

        private Task PublishEventAsync(OutboxMessage message, CancellationToken cancellationToken)
        {
            return message.Type switch
            {
                ProductCreatedEvent.EventName => PublishAsync<ProductCreatedEvent>(message, cancellationToken),

                _ => throw new InvalidOperationException($"Unknown outbox message type '{message.Type}'.")
            };
        }

        private async Task PublishAsync<TEvent>(OutboxMessage message, CancellationToken cancellationToken) where TEvent : class, IDomainEvent
        {
            var domainEvent = JsonSerializer.Deserialize<TEvent>(message.Payload)
                ?? throw new InvalidOperationException($"Unable to deserialize '{message.Type}'.");

            await _mediator.Publish(new DomainEventNotification<TEvent>(domainEvent), cancellationToken);
        }

        private static bool IsExpired(OutboxMessage message)
        {
            return message.ExpiresAt is not null &&
                   message.ExpiresAt < DateTimeOffset.UtcNow;
        }

        private static void HandleFailure(OutboxMessage message, Exception exception)
        {
            message.RetryCount++;
            message.Error = exception.ToString();

            if (message.RetryCount >= 10)
            {
                message.Status = OutboxStatus.Failed;
            }
        }
    }
}
