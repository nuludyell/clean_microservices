using Discount.API.Services;
using Discount.Application.Handlers.CommandHandlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;

namespace Discount.API;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(CreateDiscountHandler).Assembly));
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddAutoMapper(typeof(Startup));
        services.AddGrpc();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<DiscountService>();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
            });
        });
    }
}
