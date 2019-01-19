using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using static StackExchange.Redis.RedisChannel;

namespace MyWebApi.Core.RedisQueue
{
    public static class RedisConst
    {
        //发布订阅频道
        public static readonly RedisChannel channel = new RedisChannel("requests",PatternMode.Auto);
        //默认redis db
        public static readonly int defaultDb = 0;
        //默认zset  排序队列集合的key
        public static readonly string zsetKey = "REQUEST_SET";

        //任务过期时间
        public static readonly TimeSpan response_ts =new TimeSpan(TimeSpan.TicksPerHour * 1) ;
    }
}
