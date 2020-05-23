using Consul.Consumer.Fast.Extension.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consul.Consumer.Fast.Extension.Consul
{
    /// <summary>
    /// 随机获取服务
    /// </summary>
    public class ConsulRand:IConsulBase
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
            if(services.Count>0)
                consul= services[rand.Next(services.Count)];
            return consul;
        }
    }
}
