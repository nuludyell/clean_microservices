using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(
        this IHost host,
        Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        try
        {
            logger.LogInformation($"Started Db Migration: {typeof(TContext).Name}.");
            CallSeeder(seeder, context, services);
            logger.LogInformation($"Migration completed: {typeof(TContext).Name}.");
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, $"An error has occurred while migation db {typeof(TContext).Name}.");
            throw;
        }

        return host;
    }

    private static void CallSeeder<TContext>(
        Action<TContext, IServiceProvider> seeder,
        TContext context,
        IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}
