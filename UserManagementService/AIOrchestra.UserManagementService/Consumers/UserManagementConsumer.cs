
using AIOrchestra.UserManagementService.Shared;
using CommonLibrary;
using KafkaLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AIOrchestra.UserManagementService.Consumers
{
    public class UserManagementConsumer : IHostedService
    {
        private readonly IConsumer consumer;
        private readonly ILogger<UserManagementConsumer> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private Task executingTask;
        private CancellationTokenSource cts;

        public UserManagementConsumer(IConsumer consumer, ILogger<UserManagementConsumer> logger, IServiceScopeFactory serviceScopeFactory)
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
                var result = consumer.Consume(Topics.UserManagement);
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
