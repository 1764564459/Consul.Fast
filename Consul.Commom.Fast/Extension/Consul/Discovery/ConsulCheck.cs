using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Consul.Commom.Fast.Extension.Models.Discovery;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Consul.Commom.Fast.Extension.Consul.Discovery
{
    /// <summary>
    /// 服务检查
    /// </summary>
    public class ConsulCheck : ConsulDiscovery
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        private readonly ConsulConfig _config;

        /// <summary>
        /// tokenSource
        /// </summary>
        readonly CancellationTokenSource _tokenSource;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        public ConsulCheck(IOptions<ConsulConfig> config) : base(config)
        {
            _config = config.Value;
            _tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            var consumer = new ConsulClient(option => { option.Address = new Uri(_config.server); });
            await Task.Factory.StartNew(async () =>
            {
                while (!_tokenSource.IsCancellationRequested)
                {
                    try
                    {
                        //passingOnly 健康检查通过的
                        var service =
                            await consumer.Health.Service(_config.name, "",
                                true); //consumer.Catalog.Service(config.name);

                        //所有代理服务
                        var agent = service.Response.Select(p => p.Service).ToList();

                        //循环服务列表
                        foreach (var item in catalog)
                        {
                            //查找服务器服务中存在当前项
                            var consul = agent.FirstOrDefault(p => p.ID == item.ID);

                            //不存在移出
                            if (consul == null)
                            {
                                catalog.Remove(item);
                            }//存在修改信息【并移出该代理服务项】
                            else
                            {
                                JsonConvert.PopulateObject(JsonConvert.SerializeObject(consul), item);
                                agent.Remove(consul);
                            }
                        }

                        //添加代理
                        catalog.AddRange(agent);

                        Console.WriteLine("开始获取服务列表");
                        await Task.Delay(TimeSpan.FromSeconds(_config.timespan), _tokenSource.Token);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"获取服务列表出现错误：{e.Message}");
                    }
                }
            }, _tokenSource.Token);
        }
    }
}
