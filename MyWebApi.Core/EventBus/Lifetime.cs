using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApi.Core.EventBus
{   
    /// <summary>
    /// 事件生命周期
    /// </summary>
    public enum Lifetime
    {  
        /// <summary>
        /// 一次http请求一个实例
        /// </summary>
        Scope,
        /// <summary>
        /// 单例
        /// </summary>
        Singlton,
        /// <summary>
        ///  一次依赖注入一次实例
        /// </summary>
        Tansient
    }
}
