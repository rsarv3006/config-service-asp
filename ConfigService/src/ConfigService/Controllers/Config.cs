using System.Text.Json;
using ApiAlerts.Common.Services;
using ConfigService.Models;
using ConfigService.Repositories;
using Microsoft.AspNetCore.Mvc;
using DotNetEnv;
using ApiAlerts.Common.Models;

namespace ConfigService.Controllers;

[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
  private readonly ConfigRepository _configRepository;
  private readonly AlertService _alertService;

  public ConfigController(ConfigRepository configRepository, AlertService alertService)
  {
    _configRepository = configRepository;
    var apiKey = Env.GetString("API_ALERTS_KEY");
    if (apiKey != null) alertService.Activate(apiKey);
    _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
  }

  [HttpGet("{appName}")]
  public async Task<ActionResult<Config>> GetConfig(string appName)
  {
    var config = await _configRepository.GetConfig(appName);
    if (config == null)
    {
      return NotFound();
    }
    return config;
  }

  [HttpPost("{appName}")]
  public async Task<ActionResult<Config>> CreateConfig(string appName, [FromBody] JsonDocument config)
  {
    var configEntity = await _configRepository.CreateConfig(appName, config);

    List<string> tags = new List<string>
            { "config service", "create", appName };

    var alert = new ApiAlert(messageText: "Config Created", tags, $"/api/Config/{appName}");
    _alertService.PublishAlert(alert);

    return configEntity;
  }

  [HttpPost("{appName}/activate/{version}")]
  public async Task<ActionResult<Config>> ActivateConfig(string appName, int version)
  {
    var config = await _configRepository.MakeConfigActive(appName, version);
    if (config == null)
    {
      return NotFound();
    }

    List<string> tags = new List<string>
            { "config service", "activate", appName };

    var alert = new ApiAlert(messageText: "Config Activated", tags, $"/api/Config/{appName}");
    _alertService.PublishAlert(alert);

    return config;
  }
}
