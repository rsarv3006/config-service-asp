using ApiAlerts.Common.Configurations;
using ApiAlerts.Common.Services;
using ApiUtilities.Common.Handlers;
using ApiUtilities.Common.Interfaces;
using ConfigService.Repositories;
using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace ConfigService;

public class Startup
{
  public IConfiguration Configuration { get; }
  
  public Startup()
  {
    Configuration = new ConfigurationBuilder().AddEnvironmentVariables()
      .AddDotNetEnv(".env", LoadOptions.TraversePath()).Build();
  }

  // This method gets called by the runtime. Use this method to add services to the container
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddControllers();
    services.AddDbContext<ConfigServiceContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("PostgresDb")));

    services.AddSingleton<IApiConfig, ApiConfig>();
    services.AddScoped<IRequestHandler, RequestHandler>();

    services.AddScoped<ConfigRepository>();
    services.AddScoped<UserRepository>();
    services.AddScoped<AlertService>();
    services.AddHealthChecks();

  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
      endpoints.MapHealthChecks("/health");
      endpoints.MapGet("/",
              async context =>
              {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
              });
    });
  }
}
