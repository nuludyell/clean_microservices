using Basket.Application.GrpcService;
using Basket.Application.Handlers.CommandHandlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace Basket.API;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddApiVersioning();

        // Redis Settings
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Basket.API",
                Version = "v1"
            });
        });

        // DI
        services.AddAutoMapper(typeof(Startup));
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(CreateShoppingCartHandler).Assembly));
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<DiscountGrpcService>();
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));

        services.AddHealthChecks()
            .AddRedis(
            Configuration["CacheSettings:ConnectionString"],
            "Redist Health",
            HealthStatus.Degraded);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                "Basket.API v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks(
                "/heath",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
        });
    }
}
