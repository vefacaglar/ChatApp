using ChatApp.Application.Chat;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(CreateChatRoomCommand).Assembly));
            return services;
        }
    }
}
