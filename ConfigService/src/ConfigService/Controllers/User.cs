using ApiAlerts.Common.Models;
using ApiAlerts.Common.Services;
using ConfigService.Models;
using ConfigService.Repositories;
using DotNetEnv;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConfigService.Controllers;

[Route("api/[controller]")]
public class UserController : ControllerBase
{
  private readonly UserRepository _userRepository;
  private readonly AlertService _alertService;

  public UserController(UserRepository userRepository, AlertService alertService)
  {
    _userRepository = userRepository;
    var apiKey = Env.GetString("API_ALERTS_KEY");
    if (apiKey != null) alertService.Activate(apiKey);
    _alertService = alertService ?? throw new ArgumentNullException(nameof(alertService));
  }

  [HttpPost("{appName}")]
  [Authorize]
  public async Task<ActionResult<User?>> CreateUser(string appName)
  {
    var userEntity = await _userRepository.CreateUser(appName);
    List<string> tags = new List<string>
            { "user service", "create", appName };

    var alert = new ApiAlert(messageText: "User Created", tags, $"/api/User/{appName}");
    _alertService.PublishAlert(alert);

    return userEntity;
  }

}
