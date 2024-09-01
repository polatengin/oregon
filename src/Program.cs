using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
