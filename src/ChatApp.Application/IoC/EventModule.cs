using Autofac;
using ChatApp.Infrastructure;
using System.Reflection;

namespace ChatApp.Application.IoC
{
    public class EventModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(IEventHandler<>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<EventDispatcher>()
                .As<IEventDispatcher>()
                .SingleInstance();
        }
    }
}
