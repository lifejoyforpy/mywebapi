using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyWebApi.Core.EventBus
{
    /// <summary>
    /// 当前事件总线实现方式不支持生命周期，反射性能差。
    /// </summary>
   public  class EventBusManager
    {
        private  ConcurrentDictionary<Type, List<Type>> _mappingDictionary;

        public EventBusManager()
        {
            _mappingDictionary = new ConcurrentDictionary<Type, List<Type>>();
            this.InitialEvent();
        }
        /// <summary>
        /// 事件初始化注册实现了IEventData
        /// 通过反射将事件处理源和事件绑定存入线程安全字典
        /// </summary>
        public void InitialEvent()
        {
            //加载程序集
            Assembly assembly = Assembly.GetEntryAssembly();
            foreach(var type in assembly.GetTypes())
            {
                //判断当前类型 是否继承了IEventHandler接口
                if (typeof(IEventHandler).IsAssignableFrom(type))
                {
                   Type handlerInterface=  type.GetInterface("IEventHandler`1");
                   if(handlerInterface!=null)
                    {
                      Type eventDataType=  handlerInterface.GetGenericArguments()[0];
                        if (!_mappingDictionary.ContainsKey(eventDataType))
                        {
                            var handlerTypes = new List<Type> {
                                type
                            };
                            _mappingDictionary[eventDataType] = handlerTypes;
                        }
                        else {
                              List<Type> handlerTypes = _mappingDictionary[eventDataType];
                              handlerTypes.Add(type);
                             _mappingDictionary[eventDataType] = handlerTypes;
                        }
                    }
                }

            }
        }
        /// <summary>
        /// IEventData 事件源，eventHandler 事件，手动绑定事件源与事件
        /// </summary>
        /// <typeparam name="IEventData"></typeparam>
        /// <param name="eventHandler"></param>
        public void Register<IEventData>(Type eventHandler)
        {
            List<Type> handlerTypes = _mappingDictionary[typeof(IEventData)];
            if(!handlerTypes.Contains(eventHandler))
            {
                handlerTypes.Add(eventHandler);
                _mappingDictionary[typeof(IEventData)] = handlerTypes;
            }
        }
        /// <summary>
        /// 手动接触绑定的事件和事件源
        /// </summary>
        /// <typeparam name="IEventData"></typeparam>
        /// <param name="eventHandler"></param>
        public void UnRegister<IEventData>(Type eventHandler)
        {
            List<Type> handlerTypes = _mappingDictionary[typeof(IEventData)];
            if (handlerTypes.Contains(eventHandler))
            {
                 handlerTypes.Remove(eventHandler);
                _mappingDictionary[typeof(IEventData)] = handlerTypes;
            }
        }

        public void Trigger<TEventData>(TEventData eventData) where TEventData:IEventData
        {
            var handlerTypes = _mappingDictionary[typeof(TEventData)];
            if (handlerTypes != null && handlerTypes.Count > 0)
            {
                foreach (var handlerType in handlerTypes)
                {
                    // 如果存在Handler 方法
                    MethodInfo methodInfo = handlerType.GetMethod("Handler");
                    if(methodInfo!=null)
                    {
                        object handler = Activator.CreateInstance(handlerType);
                        methodInfo.Invoke(handler, new object[] { eventData });
                    }
                }

            }
        }
    }
}
