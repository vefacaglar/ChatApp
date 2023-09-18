using ChatApp.Application.Chat;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(CreateChatRoomCommand).Assembly));
            return services;
        }
    }
}
