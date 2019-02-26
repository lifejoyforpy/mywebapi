using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.RedisQueue
{
    public class RedisConfigurationOptions
    {
        public RedisConfigurationOptions()
        {

        }
        /// <summary>
        /// 如果为true，Connect 没有服务器可用时将不会创建连接  默认真 true  (Azure 上默认值为 false)
        /// </summary>
        public bool AbortOnConnectFail { get; set; }
        /// <summary>
        /// 启用被认为具有风险的一系列命令 
        /// </summary>
        public bool AllowAdmin { get; set; } = false;

        /// <summary>
        /// 所有发布/订阅操作的可选频道前缀
        /// </summary>
        public string channelPrefix { get; set; } = string.Empty;
        /// <summary>
        /// 在初始 Connect 期间重复连接尝试的次数
        /// </summary>
        public int ConnectRetry { get; set; } = 3;
        /// <summary>
        /// 连接操作的超时时间（ms）
        /// </summary>
        public int ConnectTimeout { get; set; } = 5000;
        /// <summary>
        /// 用于传达配置更改的广播通道名称
        /// </summary>
        public string ConfigurationChannel { get; set; } = "__Booksleeve_MasterChanged";
        /// <summary>
        /// 检查配置的时间（秒）。如果支持的话，这会以交互式套接字的方式保持活动。
        /// </summary>
        public int ConfigCheckSeconds { get; set; } = 60;
        /// <summary>
        /// 默认数据库索引, 从 0 到 databases - 1（0 到 Databases.Count -1）
        /// </summary>
        public int? defaultDatabase { get; set; }
        /// <summary>
        /// 发送消息以帮助保持套接字活动的时间（秒）（默认时间60s）
        /// </summary>
        public int KeepAlive { get; set; } = -1;
        /// <summary>
        /// 标识 redis 中的连接
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// redis 服务器的密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 正在使用的代理类型（如果有）; 例如“twemproxy”
        /// </summary>
        public Proxy Proxy { get; set; }
        /// <summary>
        /// 指定DNS解析应该是显式和热切，而不是隐式
        /// </summary>
        public bool ResolveDns { get; set; } = false;
        /// <summary>
        /// 目前尚未实施（预期与sentinel一起使用）
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 指定应使用SSL加密
        /// </summary>
        public bool Ssl { get; set; } = false;
        /// <summary>
        /// 在服务器证书上强制执行特定的SSL主机标识
        /// </summary>
        public string SslHost { get; set; }
        /// <summary>
        /// 允许同步操作的时间（ms）
        /// </summary>
        public int SyncTimeout { get; set; } = 1000;
        /// <summary>
        /// 用于在不明确的主场景中选择服务器的键
        /// </summary>
        public string TieBreaker { get; set; } = "__Booksleeve_TieBreak";
        /// <summary>
        /// Redis版本级别（当服务器要使用的版本默认不可用时使用） (3.0 in Azure, else 2.0)
        /// </summary>
        public string DefaultVersion { get; set; } = "2.0";
        /// <summary>
        /// 输出缓冲区的大小
        /// </summary>
        public int WriteBuffer { get; set; } = 4096;
        /// <summary>
        /// 重新连接重试策略  retry to re-connect after time in milliseconds
        /// </summary>
        public IReconnectRetryPolicy ReconnectRetryPolicy { get; set; } = new ExponentialRetry(5000);
    }


    //public enum Pxory {
    //    /// <summary>
    //    /// 不使用代理
    //    /// </summary>
    //    None,
    //    Twemproxy,
    //}
}
