using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Domain
{
    /// <summary>
    /// 基类实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Entity<T>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public T Id { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow.AddHours(8);

        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow.AddHours(8);
    }
}
