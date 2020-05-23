using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul.Consumer.Fast.Extension.Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Consul.Consumer.Fast.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConsulController:ControllerBase
    {
        ConsulCore _consul;
        public ConsulController(ConsulCore consul)
        {
            _consul = consul;
        }

        [HttpGet]
        public  IActionResult Consumer()
        {
            var service = _consul.Resolve();
            return Ok($"service:{ JsonConvert.SerializeObject(service)}");
        }
    }
}
