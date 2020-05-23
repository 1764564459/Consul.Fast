using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul.Consumer.Fast.Extension.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Consul.Consumer.Fast.Extension.Consul
{
    public static class FastConsul
    {
        /// <summary>
        /// 配置
        /// </summary>
        private static readonly ConsulConfig config = new ConsulConfig();

        /// <summary>
        /// 添加ConsulClient服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsulClient(this IServiceCollection services,Action<ConsulConfig> option)
        {
            option(config);
            //可添加多个Configure
            services.Configure<ConsulConfig>(option);
            services.AddSingleton<ConsulCore>();
            services.AddSingleton<ConsulCheck>();
            //services.Configure<int>(conf=>conf.);
            //services.AddOptions()
            return services;
        }

        /// <summary>
        /// 使用ConsulClient【开启定时检查服务】
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsulClient(this IApplicationBuilder app)
        {
            var consul = app.ApplicationServices.GetRequiredService<ConsulCheck>();
            consul.StartAsync().Wait();
            return app;
        }
    }
}
