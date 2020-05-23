using System;
using System.Collections.Generic;
using System.Text;

namespace Consul.Commom.Fast.Extension.Models.Discovery
{
    /// <summary>
    /// 均衡基类
    /// </summary>
    public interface IConsulBalance
    {
        /// <summary>
        /// 使用均衡
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        AgentService Resolve(List<AgentService> services);
    }
}
