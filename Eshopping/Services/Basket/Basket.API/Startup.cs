﻿using Basket.Application.GrpcService;
using Basket.Application.Handlers.CommandHandlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
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
        services.AddApiVersioning();
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                //TODO read the same from settings for prod deployment
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        });

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

        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ct, cfg) =>
            {
                cfg.Host(Configuration["EventBusSettings:HostAddress"]);
            });
        });
        services.AddMassTransitHostedService();

        // Identity Server changes
        var userPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        services.AddControllers(config => config.Filters.Add(new AuthorizeFilter(userPolicy)));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://id-local.eshopping.com:44344";
                options.Audience = "Basket";
            });
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        var ngindxPath = "/basket";

        if (env.IsEnvironment("Local"))
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                "Basket.API v1"));
        }

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"{ngindxPath}/swagger/{description.GroupName}/swagger.json",
                        $"Basket API {description.GroupName.ToLowerInvariant()}");

                    options.RoutePrefix = string.Empty;
                }

                options.DocumentTitle = "Basket API Documentation";
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
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
