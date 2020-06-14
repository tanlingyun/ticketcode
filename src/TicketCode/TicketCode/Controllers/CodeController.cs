using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace TicketCode.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeController : ControllerBase
    {

        private IDistributedCache cache = null;

        public CodeController(IDistributedCache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// 获取取票码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //System.Threading.ThreadPool.SetMaxThreads(10,10);
            for (int i = 0; i < 5000; i++)
            {
                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart((state)=>{
                    while (true)
                    {
                        try
                        {
                            string txt = this.cache.GetString("tkc307120");
                            //Response.WriteAsync(txt);
                        }
                        catch(Exception ex)
                        {
                            
                        }
                    }
                })).Start();
            }

            return null;
        }

        /// <summary>
        /// 核销取票码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return null;
        }
    }
}