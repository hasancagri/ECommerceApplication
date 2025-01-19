using CustomerOrderAPI.Contexts;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Polly;

namespace CustomerOrderAPI.Extensions;

public static class MigrationExtension
{
    public static void MigrateDatabase(this IApplicationBuilder builder)
    {
        var scope = builder.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


        var retry = Policy.Handle<SqlException>()
            .WaitAndRetry(6, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryAttempt, 2)), onRetry: (exception, retryCount, context) =>
            {
                string message = exception.Message;
            });

        retry.Execute(context.Database.Migrate);
    }
}
