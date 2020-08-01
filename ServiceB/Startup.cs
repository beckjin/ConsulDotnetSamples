using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceB.Consul;

namespace ServiceB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration.GetSection("Consul").Get<ConsulOption>());
            services.AddHttpClient();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, ConsulOption consulOption)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 注册
            app.RegisterConsul(lifetime, consulOption);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
