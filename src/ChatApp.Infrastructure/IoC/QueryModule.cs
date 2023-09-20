using Autofac;
using ChatApp.Infrastructure.Query;
using System.Reflection;

namespace ChatApp.Infrastructure.IoC
{
    public class QueryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IQueryHandler<,>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .InstancePerLifetimeScope();

            builder
                .RegisterType<QueryDispatcher>()
                .As<IQueryDispatcher>()
                .SingleInstance();
        }
    }
}
