using CustomerOrderAPI.Consumers;
using CustomerOrderAPI.Contexts;
using CustomerOrderAPI.Extensions;
using CustomerOrderAPI.Services.Jobs;

using MassTransit;

using Microsoft.EntityFrameworkCore;

using Quartz;

using Shared.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(_ => _.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<AddToBasketConsumer>();
    configurator.AddConsumer<ProductNameChangeConsumer>();

    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["RabbitMq:Url"], h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]!);
            h.Password(builder.Configuration["RabbitMq:Password"]!);
        });

        _configurator.ReceiveEndpoint(RabbmitMqConstantValues.AddToBasketQueue, e =>
        {
            e.ConfigureConsumer<AddToBasketConsumer>(context);
        });

        _configurator.ReceiveEndpoint(RabbmitMqConstantValues.ProductNameChangeQueue, e =>
        {
            e.ConfigureConsumer<ProductNameChangeConsumer>(context);
        });
    });
});

builder.Services.AddQuartz(configurator =>
{
    JobKey jobKey = new("CustomerOrderJobKey");
    configurator.AddJob<InboxCreatedBackgroundJob>(options => options.WithIdentity(jobKey));

    TriggerKey triggerKey = new("CustomerOrderJobTrigger");
    configurator.AddTrigger(options => options.ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
        .WithIntervalInSeconds(15)
        .RepeatForever()));
});

builder.Services.AddQuartzHostedService(options => options.AwaitApplicationStarted = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.MigrateDatabase();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
