using System;
using System.Threading;
using System.Threading.Tasks;
using BatchProcessingContract;
using MassTransit;

namespace BatchProcessingConsumer
{
   public  class PersonConsumer : IConsumer<Batch<Person>>
    {
        public async Task Consume(ConsumeContext<Batch<Person>> context)
        {
            for (int i = 0; i < context.Message.Length; i++)
            {
                ConsumeContext<Person> message = context.Message[i];
                Console.WriteLine("Id: {0} Name: {1}", message.Message.Id, message.Message.Name);
            }

           
        }
    }
}
