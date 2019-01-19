using System;
using System.Threading.Tasks;
using CorrelationId;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace MyWebApi.Core.RedisQueue
{
    public class RequestService
    {
        private ICorrelationContextAccessor _correlationContextAccessor;
        private IConnectionMultiplexer _redisConnection;
        private IServiceProvider _service;
        private ILogger<RequestService> _logger;
        public RequestService(ICorrelationContextAccessor correlationContextAccessor, IConnectionMultiplexer redisConnection, IServiceProvider service, ILogger<RequestService> logger)
        {
            _correlationContextAccessor = correlationContextAccessor;
            _service = service;
            _redisConnection = redisConnection;
            _logger = logger;
        }
        /// <summary>
        /// 添加请求
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public long? AddRequest(JToken data)
        {
            //获取请求id
            var requestId = _correlationContextAccessor.CorrelationContext.CorrelationId;
            //获取db实例
            IDatabase db = _redisConnection.GetDatabase(RedisConst.defaultDb);
            //判断请求id存在与否
            long? index = db.SortedSetRank(RedisConst.zsetKey, requestId);
            if (!index.HasValue)
            {
                //
                data["requestId"] = requestId;

                db.SortedSetAdd(RedisConst.zsetKey, requestId, GetTotalSeconds());
                PublishMessage(data.ToString());
            }
            return db.SortedSetRank(RedisConst.zsetKey, requestId);
        }
        /// <summary>
        /// 评分数
        /// </summary>
        /// <returns></returns>
        public static long GetTotalSeconds()
        {
            return (long)(DateTime.Now.ToLocalTime() - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="msg"></param>
        private void PublishMessage(string msg)
        {
            Task.Run(
                () =>
                {
                    try
                    {
                        using (var scope = _service.GetRequiredService<IServiceScopeFactory>().CreateScope())
                        {
                            var multiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>();
                            multiplexer.GetSubscriber().PublishAsync(RedisConst.channel, msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                    }
                }
             );
        }
        /// <summary>
        /// 根据requestid 获取请求任务
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public Tuple<JToken, long?> GetRequest(string requestId)
        {
            var db = _redisConnection.GetDatabase(RedisConst.defaultDb);
            long? index = db.SortedSetRank(RedisConst.zsetKey, requestId);
            var response = db.StringGet(requestId);
            if (response.IsNull)
            {
                return Tuple.Create<JToken, long?>(default(JToken), index);
            }
            return Tuple.Create<JToken, long?>(JToken.Parse(response), index);
        }
    }
}
