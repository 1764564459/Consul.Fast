using System;
using System.Collections.Generic;
using System.Text;
using Consul.Commom.Fast.Extension.Models.Discovery;

namespace Consul.Commom.Fast.Extension.Consul.Discovery
{
    /// <summary>
    /// 随机获取服务
    /// </summary>
    public class ConsulRand : IConsulBalance
    {
        /// <summary>
        /// 随机获取
        /// </summary>
        /// <param name="services">服务列表</param>
        /// <returns></returns>
        public AgentService Resolve(List<AgentService> services)
        {
            AgentService consul = null;
            Random rand = new Random();
            if (services.Count > 0)
                consul = services[rand.Next(services.Count)];
            return consul;
        }
    }

    /// <summary>
    /// 轮询获取服务
    /// </summary>
    public class ConsulPolling : IConsulBalance
    {
        /// <summary>
        /// 当前服务
        /// </summary>
        public int _index = 0;

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public AgentService Resolve(List<AgentService> services)
        {
            AgentService consul = null;
            lock (_lock)
            {
                if (_index >= services.Count)
                    _index = 0;
                if (services.Count > 0)
                    consul = services[_index++];
                return consul;
            }
        }
    }
}
