using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWebApi.Core.EventBus;
using MyWebApi.Event;
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
        private readonly IEventPublish _eventPublish;
        private readonly IEventSubscriptionManager _eventSubscription;
        /// <summary>
        /// 构造函数
        /// </summary>

        public ValuesController(ILogger<ValuesController> logger,IEventPublish eventPublish,IEventSubscriptionManager eventSubscription )
        {
            _logger = logger;
            _eventPublish = eventPublish;
            _eventSubscription = eventSubscription;
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("publish")]
        public IActionResult Publish()
        {
            _eventPublish.Publish("test", new TestEvent());
            return Ok();
        }

        [HttpGet("subscribe")]
        public IActionResult Subscribe()
        {
            _eventSubscription.Subscribe<TestEvent, TestEventHandler>();
            return Ok("add subscribe");
        }
    }
}
