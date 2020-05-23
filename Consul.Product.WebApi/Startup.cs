using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul.Product.WebApi.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Consul.Product.WebApi
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
            services.AddControllers();
            
            //Ìí¼Ó½¡¿µ¼ì²é
            services.AddHealthChecks();
            services.AddConsul();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IHostApplicationLifetime life, IOptions<ConsulSetting> setting)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseAuthorization();

            //Ê¹ÓÃ½¡¿µ¼ì²é
            app.UseHealthChecks(setting.Value.health);

            //×¢²áConsul
            app.UseConsul();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
