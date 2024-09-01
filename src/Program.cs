using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
              services.AddApplicationInsightsTelemetryWorkerService(options =>
              {
                options.ConnectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTIONSTRING");
              });

              services.AddLogging(builder =>
              {
                builder.ClearProviders();
                builder.AddApplicationInsights();
              });

              services.AddDbContext<ProjectDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")));
            });

using var host = CreateHostBuilder(args).Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started.");

// application logic here

host.Run();

public class ProjectDbContext : DbContext
{
}
