using Application.Behaviors;

using Domain.DomainServices;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddScoped<ICustomerOrderService, CustomerOrderService>();
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
