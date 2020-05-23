using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul.Consumer.Fast.Extension.Interface;

namespace Consul.Consumer.Fast.Extension.Consul
{
    /// <summary>
    /// consul配置
    /// </summary>
    public class ConsulConfig
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public string server { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 检查服务定时时间【单位秒】
        /// </summary>
        public int timespan { get; set; }

        /// <summary>
        /// 负载均衡
        /// </summary>
        public IConsulBase balancer { get; set; } = ConsulBalance.Polling;
    }
}
