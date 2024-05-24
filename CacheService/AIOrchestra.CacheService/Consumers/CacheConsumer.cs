using CommonLibrary;
using KafkaLibrary.Implementations;
using KafkaLibrary.Interfaces;

namespace AIOrchestra.CacheService.Consumers
{
    public class CacheConsumer : IHostedService
    {
        private readonly IConsumer consumer;
        private readonly ILogger<CacheConsumer> logger;
        private Task executingTask;
        private CancellationTokenSource cts;

        public CacheConsumer(IConsumer consumer, ILogger<CacheConsumer> logger)
        {
            this.consumer = consumer;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            ExecuteAsync(cts.Token);
            return executingTask.IsCompleted ? executingTask : Task.CompletedTask;
        }

        private void ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = consumer.Consume(Topics.Cache);
                logger.LogInformation($"Consumed message: {result}");
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
