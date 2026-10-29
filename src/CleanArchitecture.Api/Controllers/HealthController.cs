using CleanArchitecture.Contracts.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[Route("api/health")]
[AllowAnonymous]
public class HealthController : ApiController
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok(new HealthResponse("ok"));
    }
}
