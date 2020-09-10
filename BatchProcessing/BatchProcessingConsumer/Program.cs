using System;
using System.Threading;
using System.Threading.Tasks;
using BatchProcessingContract;
using MassTransit;

namespace BatchProcessingConsumer
{
    class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("event-listener", e =>
                {
                    e.PrefetchCount = 100;
                    e.Batch<Person>(b =>
                   {
                       b.MessageLimit = 100;
                       b.ConcurrencyLimit = 10;
                       b.TimeLimit = TimeSpan.FromSeconds(1);
                       b.Consumer(() => new PersonConsumer());
                   });
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
    }
}
