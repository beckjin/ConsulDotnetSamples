using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ServiceA.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public string Test()
        {
            return "请求 ServiceA-1 成功";
        }
    }
}
