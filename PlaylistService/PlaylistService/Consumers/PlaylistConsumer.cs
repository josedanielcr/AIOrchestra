
using CommonLibrary;
using KafkaLibrary.Interfaces;
using PlaylistService.Shared;
using System.Threading;

namespace PlaylistService.Consumers
{
    public class PlaylistConsumer : IHostedService, IDisposable
    {
        private readonly IConsumer consumer;
        private readonly ILogger<PlaylistConsumer> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private Task executingTask;
        private CancellationTokenSource cts;
        public void Dispose() => cts.Cancel();

        public PlaylistConsumer(IConsumer consumer, ILogger<PlaylistConsumer> logger, IServiceScopeFactory serviceScopeFactory)
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

        private async Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(Topics.PlaylistService);
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
            if (executingTask == null) return;
            cts.Cancel();
            await Task.WhenAny(executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }
    }
}
