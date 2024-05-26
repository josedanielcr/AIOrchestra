using AIOrchestra.CacheService.Shared;
using CommonLibrary;
using KafkaLibrary.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedLibrary;
using System.Reflection;

namespace AIOrchestra.CacheService.Consumers
{
    public class CacheConsumer : IHostedService
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
            ExecuteAsync(cts.Token);
            return executingTask.IsCompleted ? executingTask : Task.CompletedTask;
        }

        private async void ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = consumer.Consume(Topics.Cache);
                using var scope = serviceScopeFactory.CreateScope();
                var serviceProvider = scope.ServiceProvider;
                await InvokeMethod.InvokeMethodAsync(serviceProvider, result.HandlerMethod, result);
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
