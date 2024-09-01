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

              services.AddScoped<ProjectDbContext>();
            });

using var host = CreateHostBuilder(args).Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started.");

// application logic here

host.Run();

public class ProjectDbContext : DbContext
{
  private readonly ILogger<ProjectDbContext> _logger;

  public ProjectDbContext(DbContextOptions<ProjectDbContext> options, ILogger<ProjectDbContext> logger)
      : base(options)
  {
    _logger = logger;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      optionsBuilder.UseSqlServer("YourConnectionString")
          .EnableSensitiveDataLogging()
          .LogTo(_logger.LogInformation, LogLevel.Information);
    }
  }
}
