using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/healthcheck")]
[ApiController]
public class HealthCheck : ControllerBase
{
    //HeathCheck endpoint
    [HttpGet]
    public ActionResult HealthCheckEndpoint()
    {
        return Ok();
    }
}