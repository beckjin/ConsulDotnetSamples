using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/healthCheck")]
        public IActionResult Check() => Ok("ok");
    }
}
