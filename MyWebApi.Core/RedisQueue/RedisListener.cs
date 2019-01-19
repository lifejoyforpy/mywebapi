using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebApi.Core.RedisQueue
{
    public class RedisListener : IHostedService
    {
        private ILogger<RedisListener> _logger;
        private IConnectionMultiplexer _multiplexer;
        public RedisListener(ILogger<RedisListener> logger, IConnectionMultiplexer multiplexer)
        {
            _logger = logger;
            _multiplexer = multiplexer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }

        public virtual bool Process(RedisChannel channel, RedisValue message)
        {
            _logger.LogInformation("start process message " + message);

            var db = _multiplexer.GetDatabase(RedisConst.defaultDb);
            var msg = JToken.Parse(message);
            var requestId = msg["requestId"]?.ToString();
            if (string.IsNullOrEmpty(requestId))
            {
                _logger.LogWarning("request id not in message");
                return false;
            }
            //获取需要处理的数据服务
            // 把处理完数据放在redis处理完成key-value 结构里
            db.StringSet(requestId, "", RedisConst.response_ts);
            _logger.LogInformation("process message finish requestId:" + requestId);
            //把当前任务requestId 从待处理的任务移除
            db.SortedSetRemove(RedisConst.zsetKey, requestId);
            return true;
        }
        // 注册频道
        public void Register()
        {
            _multiplexer.GetSubscriber().Subscribe(
                RedisConst.channel, (ch, value) =>
                {
                    Process(ch, value);
                }
                );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
