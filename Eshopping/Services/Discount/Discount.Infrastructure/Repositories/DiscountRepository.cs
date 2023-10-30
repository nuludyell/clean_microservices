using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Coupon> GetDiscountAsync(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
            ("SELECT * FROM Coupons WHERE ProductName = @ProductName", new { ProductName = productName });

        if (coupon == null)
        {
            return new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No available discount"
            };
        }

        return coupon;
    }

    public async Task<bool> CreateDiscountAsync(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected =
            await connection.ExecuteAsync(
                "INSERT INTO Coupons (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new
                {
                    coupon.ProductName,
                    coupon.Description,
                    coupon.Amount
                });

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> UpdateDiscountAsync(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected =
            await connection.ExecuteAsync(
                "UPDATE Coupons SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new
                {
                    coupon.ProductName,
                    coupon.Description,
                    coupon.Amount,
                    coupon.Id
                });

        if (affected == 0)
            return false;

        return true;
    }

    public async Task<bool> DeleteDiscountAsync(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        var affected =
            await connection.ExecuteAsync(
                "DELETE Coupons WHERE ProductName = @productName",
                new
                {
                    ProductName = productName
                });

        if (affected == 0)
            return false;

        return true;
    }
}
