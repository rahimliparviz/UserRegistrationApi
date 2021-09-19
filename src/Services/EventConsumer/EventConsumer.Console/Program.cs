using System;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Messages.Events;
using MassTransit;

namespace EventConsumer.Console
{
   

    public class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("amqp://guest:guest@localhost:5672");
                cfg.ReceiveEndpoint("event-listener", e =>
                {
                    e.Consumer<EventConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                System.Console.WriteLine("Press enter to exit");

                await Task.Run(() => System.Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        class EventConsumer : IConsumer<UserRegistratedEvent>
        {
            public async Task Consume(ConsumeContext<UserRegistratedEvent> context)
            {
                System.Console.WriteLine("Value: {0}", context.Message);
            }
        }
    }
}
