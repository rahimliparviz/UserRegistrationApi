using EventBus.Messages.Constants;
using EventBus.Messages.Events;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        //static void Main(string[] args)
        public static async Task Main()
        {
            Console.WriteLine("Listening to events...");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("amqp://guest:guest@localhost:5672");
                cfg.ReceiveEndpoint(EventBusContstants.UserRegistrationQueue, e =>
                {
                    e.Consumer<EventConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
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
                Console.WriteLine("Value: {0}", context.Message.Message);
            }
        }
    }
}
