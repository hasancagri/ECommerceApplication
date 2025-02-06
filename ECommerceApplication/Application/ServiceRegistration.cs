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
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.RegisterServicesFromAssembly(assembly);
        });

        services.AddScoped<ICustomerOrderService, CustomerOrderService>();
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
