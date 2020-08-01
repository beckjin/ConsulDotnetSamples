using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ServiceA.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public string Get()
        {
            return $"service-a {_configuration["Id"]}";
        }
    }
}
