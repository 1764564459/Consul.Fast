using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using Consul.Commom.Fast.Extension.Models.Register;

namespace Consul.Commom.Fast.Extension.Consul.Register
{
    /// <summary>
    /// Consul服务注册
    /// </summary>
    public static class ServiceRegister
    {
        /// <summary>
        /// 注册Consul
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.consul.json", false, true).Build();
            services.Configure<ConsulSetting>(config);

            return services;
        }

        /// <summary>
        /// 使用Consul
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            //控制反转获取配置文件
            //var config = app.ApplicationServices.GetRequiredService<IConfiguration>().GetSection("Consul");
            //获取配置节点
            //var section = configuration.GetSection("Consul");

            //获取配置
            var config = app.ApplicationServices.GetRequiredService<IOptions<ConsulSetting>>().Value;

            //获取服务地址
            Uri server = new Uri(config.server);

            //获取健康检查地址
            string health = config.health;//section.GetValue<string>("health");

            //获取服务名
            string name = config.name; //section.GetValue<string>("name");

            //获取程序运行生命
            var life = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();



            //创建配置客户端
            var client = new ConsulClient(option =>
            {
                option.Address = server;
            });

            //服务检查
            var check = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(3),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//心跳检查时间
                HTTP = $"{config.host}{health}",//健康检查地址
                Timeout = TimeSpan.FromSeconds(15)
            };

            //向Consul注册服务
            var register = new AgentServiceRegistration()
            {
                Checks = new[] { check },
                ID = $"{Guid.NewGuid()}",
                Address = server.Host,
                Port = server.Port,
                Name = name,
                Tags = new[] { $"{name}" }
            };

            //代理注册服务
            client.Agent.ServiceRegister(register).Wait();

            //程序停止时、取消注册服务
            life.ApplicationStopping.Register(() =>
            {
                client.Agent.ServiceDeregister(register.ID).Wait();
            });

            return app;
        }
    }
}
