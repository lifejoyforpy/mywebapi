using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MyWebApi.Controllers
{
    /// <summary>
    /// ValuesController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {

        private  readonly ILogger<ValuesController> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;

        }
        /// <summary>
        /// GET api/values
        /// </summary>
        // GET api/values
        [HttpGet]
        //[EnableCors("CorsTest")]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.LogWarning("asdsasadsadasdassad");
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //  GET api/values/5
       [EnableCors("CorsTest")]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            _logger.LogError("asdsasadsadasdassad");
            return "100";
        }

        /// <summary>
        ///  POST api/values
        /// </summary>

        [HttpPost]
        public void Post([FromBody] string value)
        {
            //_logger.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
