using AIOrchestra.CacheService.Shared;
using CommonLibrary;
using KafkaLibrary.Interfaces;

namespace AIOrchestra.CacheService.Consumers
{
    public class CacheConsumer : IHostedService, IDisposable
    {
        private readonly IConsumer consumer;
        private readonly ILogger<CacheConsumer> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private Task executingTask;
        private CancellationTokenSource cts;

        public CacheConsumer(IConsumer consumer, ILogger<CacheConsumer> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.consumer = consumer;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            executingTask = ExecuteAsync(cts.Token);
            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(Topics.Cache);
                    using var scope = serviceScopeFactory.CreateScope();
                    var serviceProvider = scope.ServiceProvider;
                    await InvokeMethod.InvokeMethodAsync(serviceProvider, result.HandlerMethod, result);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while consuming messages.");
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (executingTask == null)
            {
                return;
            }

            cts.Cancel();

            await Task.WhenAny(executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        public void Dispose()
        {
            cts?.Cancel();
        }
    }
}

