
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWebApi.Application.Dtos.ProductDto;
using System.Net;
using System.Net.NetworkInformation;

namespace MyWebApi.Controllers
{
    //Get, 查询, Attribute: HttpGet, 例如: '/api/product', '/api/product/1'
    //POST, 创建, HttpPost, '/api/product'
    //PUT 整体修改更新 HttpPut, '/api/product/1'
    //PATCH 部分更新, HttpPatch, '/api/product/1'
    //DELETE 删除, HttpDelete, '/api/product/1
    // 200: OK
    //201: Created, 创建了新的资源
    //204: 无内容 No Content, 例如删除成功
    //400: Bad Request, 指的是客户端的请求错误.
    //401: 未授权 Unauthorized.
    //403: 禁止操作 Forbidden. 验证成功, 但是没法访问相应的资源
    //404: Not Found 
    //409: 有冲突 Conflict.
    //500: Internal Server Error, 服务器发生了错误.
    /// <summary>  
    /// </summary>
    /// 
    [Route("api/Test")]
    [ApiController]
    //[Authorize]
    public class TestController : ControllerBase
    {
        private readonly  ILogger<TestController> _logger;
        /// <summary>
        /// constructor
        /// </summary>

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// product add
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("post")]
        public IActionResult Post(ProductCreation product)
        {  
            //验证数据合法性
            if (product == null)
            {
                return BadRequest();
            }
            //验证字段
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //insert
            return CreatedAtRoute("", "", "'");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public IActionResult Get()
        {
            _logger.LogInformation("Es Test");
            return Ok("AuthorizeServer");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/ip")]
        [AllowAnonymous]
        public IActionResult GetIp(NetworkInterfaceType type)
        {
            var netwrokInterfaces=  NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface network in netwrokInterfaces)
            {
                

            }
            return new  BadRequestResult();
        }
    }
}