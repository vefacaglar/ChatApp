using ChatApp.Domain;
using ChatApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ChatApp.Domain.Database.ChatDb;
using ChatApp.Application;
using ChatApp.Application.EventBus;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ChatDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:ChatDbCommand"]));

builder.Services.AddDomain();
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddSingleton(sp =>
{
    var logger = sp.GetRequiredService<ILogger<RabbitMqPersistentConnection>>();

    var factory = new ConnectionFactory()
    {
        HostName = configuration["ConnectionStrings:EventBus:Connection"]
    };

    if (!string.IsNullOrEmpty(configuration["ConnectionStrings:EventBus:UserName"]))
    {
        factory.UserName = configuration["ConnectionStrings:EventBus:UserName"];
    }

    if (!string.IsNullOrEmpty(configuration["ConnectionStrings:EventBus:Password"]))
    {
        factory.Password = configuration["ConnectionStrings:EventBus:Password"];
    }

    return new RabbitMqPersistentConnection(factory);
});

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

app.UseRabbitListener();

app.Run();
