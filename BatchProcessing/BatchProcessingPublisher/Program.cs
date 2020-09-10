using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using BatchProcessingContract;
using System.Collections.Generic;
using System.Linq;

namespace BatchProcessing
{
    class Program
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter the total number of messages you want to publish (or quit to exit)");
                        Console.Write("> ");
                        return Console.ReadLine();
                    });

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Exiting ...");
                        Thread.Sleep(2000);
                        return;
                    }

                    if (!int.TryParse(value, out int total))
                    {
                        Console.WriteLine("Unknown value, exiting ...");
                        Thread.Sleep(2000);
                    return;
                    }
                    for (int x = 0; x <= total - 1; x++)
                    {
                        await busControl.Publish<Person>(new
                        {
                            Id = x.ToString(),
                            Name = RandomString(20),
                            DateOfBirth = DateTime.Now,
                            Interests = new List<string>() { "Soccer", "Running", "Hiking" }
                        });
                    }
                    Console.WriteLine("Bye ...");
                    Thread.Sleep(2000);
                return;
                
        
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}

