using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace MyWebApi.Core.EventBus
{
    public class EventConsumer : BackgroundService
    {

        private readonly EventQueue.EventQueue _eventQueue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventConsumer> _logger;
        private readonly int maxExcuteNum = 10;
        private readonly EventStore.EventStore _eventStore;
        public EventConsumer(EventQueue.EventQueue eventQueue ,  EventStore.EventStore eventStore, IServiceProvider serviceProvider,ILogger<EventConsumer> logger)
        {
            _eventQueue = eventQueue;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _eventStore = eventStore;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var semaphore = new SemaphoreSlim(maxExcuteNum))
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var _queueKeys = _eventQueue.QueueKeys;
                    if (_queueKeys.Count > 0)
                    {
                        foreach (var _queueKey in _queueKeys)
                        {
                            if (!_eventQueue.ContainsQueue(_queueKey))
                            {
                                return;
                            }
                            try
                            {
                                await semaphore.WaitAsync(stoppingToken);
                                if (_eventQueue.TryDeQueue(_queueKey, out var @event))
                                {
                                    var handler = _eventStore.GetEventHanlder(@event, _serviceProvider);
                                    if (handler is IEventHandler eventHanlder)
                                    {
                                        try
                                        {
                                            await eventHanlder.Hanlder(@event as IEventData);
                                        }
                                        catch (Exception e)
                                        {
                                            _logger.LogError(e, "event {} handle exception", @event.EventId);
                                        }

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "event consumer run error");
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        }


                    }
                    await Task.Delay(50, stoppingToken);
                }
            
            }
        }


       
    }
}
