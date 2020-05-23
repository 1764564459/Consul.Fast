using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Consul.Product.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HealthController:ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Health()
        {
            //Console.WriteLine($"心跳检查：{DateTime.Now}");
            return Ok(StatusCodes.Status200OK);
        }
    }
}
