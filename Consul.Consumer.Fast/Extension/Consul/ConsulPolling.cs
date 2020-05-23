using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul.Consumer.Fast.Extension.Interface;

namespace Consul.Consumer.Fast.Extension.Consul
{
    /// <summary>
    /// 轮询获取服务
    /// </summary>
    public class ConsulPolling : IConsulBase
    {
        /// <summary>
        /// 当前服务
        /// </summary>
        public  int _index = 0;

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
                if(services.Count>0)
                    consul= services[_index++];
                return consul;
            }
        }
    }
}
