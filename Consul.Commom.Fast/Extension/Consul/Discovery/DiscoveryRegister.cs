﻿using System;
using System.Collections.Generic;
using System.Text;
using Consul.Commom.Fast.Extension.Models.Discovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Consul.Commom.Fast.Extension.Consul.Discovery
{
    public static class DiscoveryRegister
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
        public static IServiceCollection AddConsulClient(this IServiceCollection services, Action<ConsulConfig> option)
        {
            option(config);
            //可添加多个Configure
            services.Configure<ConsulConfig>(option);
            services.AddSingleton<ConsulDiscovery>();
            services.AddSingleton<ConsulCheckPolicy>();
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
            var consul = app.ApplicationServices.GetRequiredService<ConsulCheckPolicy>();
            consul.StartAsync().Wait();
            return app;
        }
    }
}
