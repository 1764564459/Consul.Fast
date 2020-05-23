using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul.Consumer.Fast.Extension.Consul;

namespace Consul.Consumer.Fast.Extension.Interface
{
    /// <summary>
    /// 均衡基类
    /// </summary>
    public interface IConsulBase
    {
        /// <summary>
        /// 使用均衡
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        AgentService Resolve(List<AgentService> services);
    }
}
