using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var config = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation("Discount Db Migration started.");
            ApplyMigration(config);
            logger.LogInformation("Discount Db Migration completed.");
        }
        catch (Exception ex)
        {
            Console.Write(ex);
            throw;
        }

        return host;
    }

    private static void ApplyMigration(IConfiguration config)
    {
        using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
        connection.Open();
        using var command = new NpgsqlCommand
        {
            Connection = connection
        };

        command.CommandText = "DROP TABLE IF EXISTS Coupons";
        command.ExecuteNonQuery();

        command.CommandText = @"CREATE TABLE Coupons(Id SERIAL PRIMARY KEY,
                                ProductName VARCHAR(500) NOT NULL,
                                Description TEXT,
                                Amount NUMERIC(5,2))";
        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO Coupons(ProductName, Description, Amount) VALUES('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500)";
        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO Coupons(ProductName, Description, Amount) VALUES('Yonvex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Racquet Discount', 700)";
        command.ExecuteNonQuery();

        connection.Close();
    }
}
