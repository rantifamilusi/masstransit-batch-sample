using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using BatchProcessingContract;
using System.Collections.Generic;

namespace BatchProcessing
{
    class Program
    {
     

        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                do
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    await busControl.Publish<Person>(new
                    {
                       Id= value,
                        Name = "Ranti Familusi",
                        DateOfBirth = DateTime.Now,
                        Interests = new List<string>() { "Soccer", "Running","Hiking"}
                    });
                }
                while (true);
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}

