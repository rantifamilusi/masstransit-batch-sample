using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace BatchProcessingConsumer
{
    public class PersonConsumerDefinition :
        ConsumerDefinition<PersonConsumer>
    {
        public PersonConsumerDefinition()
        {
            Endpoint(x => x.PrefetchCount = 1000);
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<PersonConsumer> consumerConfigurator)
        {
            consumerConfigurator.Options<BatchOptions>(options => options
                .SetMessageLimit(100)
                .SetTimeLimit(1000)
                .SetConcurrencyLimit(10));
        }
    }
}
