using System;
using System.Collections.Generic;
using System.Text;

namespace Consul.Commom.Fast.Extension.Models.Register
{
    public class ConsulSetting
    {
        /// <summary>
        /// 程序运行主机
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// consul 服务地址
        /// </summary>
        public string server { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string health { get; set; }
    }
}
