using Microsoft.AspNetCore.Mvc;

namespace ConfigService.Controllers;

[Route("api/[controller]")]
public class StatusController : ControllerBase
{
  [HttpGet]
  public ActionResult<string> GetStatus()
  {
    return "OK";
  }
}
