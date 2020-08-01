using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;

namespace ServiceA.Consul
{
    public static class ConsulBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, ConsulOption consulOption)
        {
            var consulClient = new ConsulClient(x =>
            {
                x.Address = new Uri(consulOption.Address);
            });

            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = consulOption.ServiceName,// 服务名
                Address = consulOption.ServiceIP, // 服务绑定IP
                Port = consulOption.ServicePort, // 服务绑定端口
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                    HTTP = consulOption.ServiceHealthCheck,//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            // 服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();

            // 应用程序终止时，服务取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }
    }
}
