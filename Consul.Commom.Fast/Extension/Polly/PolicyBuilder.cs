using System;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace Consul.Commom.Fast.Extension.Polly
{
    public class PolicyBuilder
    {
        public static Policy CreatePolly()
        {
            //超时
            var timeout = Policy.Timeout(5);//Handle<TimeoutRejectedException>();

            //var retry = Policy.Handle<Exception>().WaitAndRetry(5, (i,e) =>
            //{
            //    e
            //});

            //重试
            var retry = Policy.Handle<Exception>().Retry(5, (e, i) =>
            {
                Console.WriteLine(e);
            });

            //等待重试
            var wait = Policy.Handle<Exception>().WaitAndRetry(5,//重试次数
                i => TimeSpan.FromSeconds(3*i),//间隔3秒*次数
                (ex, time) => { Console.WriteLine(ex); });

            //降级
            var fallback = Policy.Handle<Exception>().Fallback(() =>
            {
                Console.WriteLine("降级");
            }, ex =>
            {
                Console.WriteLine(ex.Message);
            });

            //5次错误熔断，10秒重试
            var _break = Policy.Handle<Exception>().CircuitBreaker(5, TimeSpan.FromSeconds(10),
                (e, t) =>//OPEN打开熔断
                {
                    Console.WriteLine("开始熔断");
                }, () =>//COLSE 关闭熔断
                {
                    Console.WriteLine("关闭熔断");
                }, () =>//HALF-OPEN 重试半开
                {
                    Console.WriteLine("重试半开");
                });

            //高级熔断器
            var breaks = Policy.Handle<Exception>()
                .AdvancedCircuitBreaker(
                    0.5, //故障阈值50%
                    TimeSpan.FromSeconds(10), //故障采样时间
                    6, //最小吞吐量【10秒最少执行6次】
                    TimeSpan.FromSeconds(15));//熔断时间

            //舱壁隔离【限流】 控制并发
          var bulk=  Policy.Bulkhead(
              10, //最大并发通过量
              20);//最大排队数量

            return Policy.Wrap(fallback, _break, retry, timeout);
        }
    }
}
