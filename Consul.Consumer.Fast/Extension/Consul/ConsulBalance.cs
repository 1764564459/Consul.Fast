using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul.Consumer.Fast.Extension.Interface;

namespace Consul.Consumer.Fast.Extension.Consul
{
    /// <summary>
    /// 负载均衡
    /// </summary>
    public class ConsulBalance
    {
        /// <summary>
        /// 随机
        /// </summary>
        public static IConsulBase Rand = new ConsulRand();

        /// <summary>
        /// 轮询
        /// </summary>
        public static IConsulBase Polling = new ConsulPolling();
    }
}
