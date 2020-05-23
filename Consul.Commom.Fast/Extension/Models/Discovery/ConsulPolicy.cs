using System;
using System.Collections.Generic;
using System.Text;
using Consul.Commom.Fast.Extension.Consul.Discovery;

namespace Consul.Commom.Fast.Extension.Models.Discovery
{
    public class ConsulPolicy
    {
        /// <summary>
        /// 随机
        /// </summary>
        public static IConsulBalance Rand = new ConsulRand();

        /// <summary>
        /// 轮询
        /// </summary>
        public static IConsulBalance Polling = new ConsulPolling();
    }
}
