using Autofac;
using ChatApp.Infrastructure.Command;
using System.Reflection;

namespace ChatApp.Infrastructure.IoC
{
    public class CommandModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(ICommandHandler<,>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<,>))
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(typeof(ICommandHandler<>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .SingleInstance();
        }
    }
}
