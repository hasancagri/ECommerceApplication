using Application.Data;
using Application.Services;

using Domain.ProductModels;

using Infrastructure.EntityFramework.Contexts;
using Infrastructure.EntityFramework.Interceptors;
using Infrastructure.EntityFramework.Repositories.Customers;
using Infrastructure.EntityFramework.Repositories.OutboxMessages;
using Infrastructure.EntityFramework.Repositories.ProductModels;
using Infrastructure.EntityFramework.Repositories.Products;
using Infrastructure.EntityFramework.UnitOfWorks;
using Infrastructure.Services.Caching;
using Infrastructure.Services.Jobs;

using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

using StackExchange.Redis;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddSingleton<InsertOutboxMessagesInterceptor>();
        services.AddSingleton<PublishDomainEventInterceptor>();
        services.AddDbContext<ApplicationContext>((sp, options) =>
        {
            var outBoxInterceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptor>();
            var publishDomainInterceptor = sp.GetRequiredService<PublishDomainEventInterceptor>();

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .AddInterceptors(outBoxInterceptor,
                             publishDomainInterceptor);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

        services.AddScoped<IProductModelRepository, ProductModelRepository>()
            .Decorate<IProductModelRepository, ProductModelCacheRepository>();

        services.AddQuartz(configurator =>
        {
            JobKey jobKey = new("JobKey");
            configurator.AddJob<OutboxCreatedBackgroundJob>(options => options.WithIdentity(jobKey));

            TriggerKey triggerKey = new("JobTrigger");
            configurator.AddTrigger(options => options.ForJob(jobKey)
            .WithIdentity(triggerKey)
            .StartAt(DateTime.UtcNow)
            .WithSimpleSchedule(builder => builder
                .WithIntervalInSeconds(15)
                .RepeatForever()));
        });

        services.AddQuartzHostedService(options => options.AwaitApplicationStarted = true);

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, _configurator) =>
            {
                _configurator.Host(configuration["RabbitMq:Url"], h =>
                {
                    h.Username(configuration["RabbitMq:Username"]!);
                    h.Password(configuration["RabbitMq:Password"]!);
                });
            });
        });

        services.AddSingleton<ICacheService, RedisCacheService>();
        var redisConnection = ConnectionMultiplexer
            .Connect(configuration.GetConnectionString("Redis")!, x => x.AllowAdmin = true);
        services.AddSingleton<IConnectionMultiplexer>(redisConnection);

        return services;
    }
}
