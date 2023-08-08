using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/healthcheck")]
[ApiController]
public class HealthCheck : ControllerBase
{
    [HttpGet]
    public ActionResult HealthCheckEndpoint()
    {
        return Ok();
    }
}