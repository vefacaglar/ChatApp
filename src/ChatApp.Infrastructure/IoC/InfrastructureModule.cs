using Autofac;
using ChatApp.Infrastructure.EventBus;
using RabbitMQ.Client;

namespace ChatApp.Infrastructure.IoC
{
    public class InfrastructureModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RabbitMQPersistentConnection>()
                .As<IPersistentConnection<IModel>>()
                .SingleInstance();

            builder
                .RegisterType<RabbitMQEventBus>()
                .As<IEventBus>()
                .SingleInstance();
        }
    }
}
