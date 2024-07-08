using ApiAlerts.Common.Configurations;
using ApiAlerts.Common.Services;
using ApiUtilities.Common.Handlers;
using ApiUtilities.Common.Interfaces;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace ConfigService;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
      Configuration = configuration;
      Env.Load(".env", LoadOptions.TraversePath());
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddControllers();
    services.AddDbContext<ConfigServiceContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("PostgresDb")));

    services.AddSingleton<IApiConfig, ApiConfig>();
    services.AddScoped<IRequestHandler, RequestHandler>();

    services.AddScoped<Repositories.ConfigRepository>();
    services.AddScoped<Repositories.UserRepository>();
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
