using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceB.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<string> TestServiceA()
        {
            var url = _configuration["ConsulAddress"].ToString();

            using (var consulClient = new ConsulClient(a => a.Address = new Uri(url)))
            {
                var services = consulClient.Catalog.Service("ServiceA").Result.Response;
                if (services != null && services.Any())
                {
                    // 模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡框架
                    Random r = new Random();
                    int index = r.Next(services.Count());
                    var service = services.ElementAt(index);

                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.GetAsync($"http://{service.ServiceAddress}:{service.ServicePort}/values/test");
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                }
            }
            return "未发现服务";
        }
    }
}
