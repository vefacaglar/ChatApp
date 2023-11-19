using Autofac;
using ChatApp.Application.EventBus;
using ChatApp.Infrastructure;
using RabbitMQ.Client;

namespace ChatApp.Application.IoC
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
