using ConfigService.Models;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace ConfigService;

public class ConfigServiceContext : DbContext
{
  public DbSet<User> Users { get; set; }
  public DbSet<Config> Configs { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {

    var host = Env.GetString("DB_HOST");
    var db = Env.GetString("DB_NAME");
    var user = Env.GetString("DB_USER");
    var password = Env.GetString("DB_PASS");

    optionsBuilder.UseNpgsql(
        $"Host={host};Database={db};Username={user};Password={password}");
  }

  public ConfigServiceContext(DbContextOptions<ConfigServiceContext> options)
      : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {


  }
}
