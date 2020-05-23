using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Timeout;

namespace Consul.Product.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PollyController:ControllerBase
    {
        /// <summary>
        /// 降级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Fallback()
        {

            var polly = Policy.Handle<Exception>()
                .Fallback( rest =>
                {
                    Console.WriteLine("throw  Error");
                });

            polly.Execute( () =>
            {
                Console.WriteLine("polly start");
                throw new Exception("");
                Console.WriteLine("polly end");
            });
            return Ok(StatusCodes.Status200OK);
        }

        /// <summary>
        /// 重试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Retry()
        {
            var polly = Policy.Handle<Exception>().Retry(10);

            try
            {
                polly.Execute(() =>
                {
                    Console.WriteLine("Polly Start");
                    if (DateTime.Now.Second % 5 != 0)
                        throw new Exception("Retry Error");
                    Console.WriteLine("Polly End");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Ok();
        }

        /// <summary>
        /// 熔断
        /// </summary>
        [HttpGet]
        public void CircuitBreaker()
        {
            var polly = Policy.Handle<Exception>()
                .CircuitBreaker(5, TimeSpan.FromSeconds(6));
            while (true)
            {
                try
                {
                    polly.Execute(() =>
                    {
                        Console.WriteLine("Retry Start");
                        throw new Exception("Special error occured");
                        Console.WriteLine("Retry End");
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There's one unhandled exception : " + ex.Message);
                }

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 超时
        /// </summary>
        [HttpGet]
        public void TimeOut()
        {
            try
            {
                var polly = Policy.Handle<TimeoutRejectedException>()
                    .Fallback(() =>
                    {
                        Console.WriteLine("Fallback");
                    });
                var timeout = Policy.Timeout(2, TimeoutStrategy.Pessimistic);

                var warp = Policy.Wrap(timeout, polly);
                warp.Execute(() =>
                {
                    Console.WriteLine("Retry Start...");
                    Thread.Sleep(5000);
                    //throw new Exception();
                    Console.WriteLine("Retry End...");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
