﻿using ChatApp.Infrastructure.Repositories;
using ChatApp.Infrastructure.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, ChatDbUnitOfWork>();
            services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddTransient<IMsSqlDbConnectionFactory, MsSqlConnectionFactory>();
            return services;
        }
    }
}
