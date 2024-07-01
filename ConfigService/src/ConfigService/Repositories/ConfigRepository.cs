using System.Text.Json;
using ConfigService.Models;
using Microsoft.EntityFrameworkCore;

namespace ConfigService.Repositories;

public class ConfigRepository
{
  private readonly ConfigServiceContext _context;

  public ConfigRepository(ConfigServiceContext context)
  {
    _context = context;
  }

  public async Task<Config?> GetConfig(string appName)
  {
    return await _context.Configs
      .Where(c => c.AppName == appName && c.Status == ConfigStatus.Active)
      .OrderByDescending(c => c.Version)
      .FirstOrDefaultAsync();
  }

  public async Task<Config> CreateConfig(string appName, JsonDocument config)
  {
    var version = await GetConfigVersion(appName);

    var configEntity = new Config(
      config,
      appName,
      version + 1,
      version == 0 ? ConfigStatus.Active : ConfigStatus.Inactive
      );

    _context.Configs.Add(configEntity);
    await _context.SaveChangesAsync();
    return configEntity;
  }

  public async Task<int> GetConfigVersion(string appName)
  {
    var config = await _context.Configs
      .Where(c => c.AppName == appName)
      .OrderByDescending(c => c.Version)
      .FirstOrDefaultAsync();
    return config?.Version ?? 0;
  }

  public async Task<Config?> MakeConfigActive(string appName, int versionToActivate)
  {
    var config = await _context.Configs.FirstOrDefaultAsync(c => c.AppName == appName && c.Version == versionToActivate);

    if (config == null)
    {
      return null;
    }

    config.Status = ConfigStatus.Active;
    await _context.SaveChangesAsync();
    return config;
  }
}
