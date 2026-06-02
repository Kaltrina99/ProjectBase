using Infrastructure.Data;
using Infrastructure.External;
using Infrastructure.Repositories;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? configuration["ConnectionStrings:DefaultConnection"]
                ?? configuration["DefaultConnection"]
                ?? Environment.GetEnvironmentVariable("DEFAULTCONNECTION");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("The SQL connection string 'DefaultConnection' is not configured.");
            }

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddHttpClient<IExternalServiceClient, ExternalServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("ExternalService:BaseAddress") ?? "https://httpbin.org/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
