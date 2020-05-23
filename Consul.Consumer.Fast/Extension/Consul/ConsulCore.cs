using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul.Consumer.Fast.Extension.Interface;
using Microsoft.Extensions.Options;

namespace Consul.Consumer.Fast.Extension.Consul
{
    public class ConsulCore
    {

        /// <summary>
        /// tokenSource
        /// </summary>
        private readonly CancellationTokenSource _tokenSource;

        /// <summary>
        /// 服务列表
        /// </summary>
        protected static readonly List<AgentService> catalog=new List<AgentService>();

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="config"></param>
        private delegate Task Func(ConsulConfig config);

        ///// <summary>
        ///// 泛型委托【必须有返回值不能为void】
        ///// </summary>
        //private  Func<int,int> _invock; 

        ///// <summary>
        ///// 
        ///// </summary>
        //public int Invock(int i)
        //{
        //    return 0;
        //}

        /// <summary>
        /// 配置
        /// </summary>
        private readonly ConsulConfig _config;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConsulCore(IOptions<ConsulConfig> config)
        {
            _tokenSource = new CancellationTokenSource();
            _config = config.Value;
            //Func func = new Func(StartAsync); //new Func(Invock);
            //func.Invoke(_config);

            //_invock = Invock;
            //_invock.Invoke(10);
        }

        /// <summary>
        /// 负载均衡获取服务
        /// </summary>
        /// <returns></returns>
        public AgentService Resolve()
        {
            return _config.balancer.Resolve(catalog);
        }

       
    }
}
