using ChatApp.Domain;
using ChatApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ChatApp.Domain.Database.ChatDb;
using ChatApp.Application;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using ChatApp.Application.IoC;
using ChatApp.Infrastructure.Transactions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ChatDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:ChatDbCommand"]));
builder.Services.AddSingleton(configuration.Get<ChatAppConfiguration>());

builder.Services.AddDomain();
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(
    builder => builder
        .RegisterModule(new CommandModule())
        .RegisterModule(new EventModule())
        .RegisterModule(new InfrastructureModule())
        .RegisterModule(new QueryModule())
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ConfigureEventBus(app);

app.Run();

void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.Subscribe<EventCreatedChatRoom>();
}