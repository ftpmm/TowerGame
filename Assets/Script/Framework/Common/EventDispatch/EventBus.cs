using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lzengine
{
    public class EventBus : Singleton<EventBus>
    {
        private class ListenerWrapper
        {
            public Delegate Callback { get; set; }
            public int Priority { get; set; }
            public bool IsPaused { get; set; }
        }

        private class QueuedEvent
        {
            public int EventId { get; set; }
            public IEventData Data { get; set; }
            public EventPriority Priority { get; set; }
            public float Delay { get; set; }
            public float CreatedTime { get; set; }
        }

        private readonly Dictionary<int, List<ListenerWrapper>> _listeners = new Dictionary<int, List<ListenerWrapper>>();
        private readonly List<QueuedEvent> _eventQueue = new List<QueuedEvent>();
        private readonly List<QueuedEvent> _pendingEvents = new List<QueuedEvent>();
        private readonly HashSet<int> _processingEvents = new HashSet<int>();
        
        private bool _isProcessing;
        private bool _enableDebugLog = false;

        #region Add Listeners

        public void AddListener(int eventId, EventCallback callback, EventPriority priority = EventPriority.Normal)
        {
            InternalAddListener(eventId, callback, (int)priority);
        }

        public void AddListener<T>(int eventId, EventCallback<T> callback, EventPriority priority = EventPriority.Normal) where T : IEventData
        {
            InternalAddListener(eventId, callback, (int)priority);
        }

        private void InternalAddListener(int eventId, Delegate callback, int priority)
        {
            if (!_listeners.TryGetValue(eventId, out var listeners))
            {
                listeners = new List<ListenerWrapper>();
                _listeners[eventId] = listeners;
            }

            if (listeners.Any(l => l.Callback == callback))
            {
                LZDebug.LogWarning($"Listener already registered for event {eventId}");
                return;
            }

            listeners.Add(new ListenerWrapper { Callback = callback, Priority = priority, IsPaused = false });
            listeners.Sort((a, b) => b.Priority.CompareTo(a.Priority));

            if (_enableDebugLog)
            {
                LZDebug.Log($"[EventBus] Added listener for event {eventId}, total {listeners.Count} listeners");
            }
        }

        #endregion

        #region Remove Listeners

        public void RemoveListener(int eventId, EventCallback callback)
        {
            InternalRemoveListener(eventId, callback);
        }

        public void RemoveListener<T>(int eventId, EventCallback<T> callback) where T : IEventData
        {
            InternalRemoveListener(eventId, callback);
        }

        public void RemoveAllListeners(int eventId)
        {
            if (_listeners.ContainsKey(eventId))
            {
                _listeners[eventId].Clear();
                _listeners.Remove(eventId);
                
                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Removed all listeners for event {eventId}");
                }
            }
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
            _eventQueue.Clear();
            _pendingEvents.Clear();
            
            if (_enableDebugLog)
            {
                LZDebug.Log("[EventBus] Removed all listeners and cleared event queue");
            }
        }

        private void InternalRemoveListener(int eventId, Delegate callback)
        {
            if (!_listeners.TryGetValue(eventId, out var listeners))
                return;

            var wrapper = listeners.FirstOrDefault(l => l.Callback == callback);
            if (wrapper != null)
            {
                listeners.Remove(wrapper);
                
                if (listeners.Count == 0)
                {
                    _listeners.Remove(eventId);
                }
                
                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Removed listener for event {eventId}");
                }
            }
        }

        #endregion

        #region Dispatch

        public void Dispatch(int eventId)
        {
            InternalDispatch(eventId, null, false, 0f, EventPriority.Normal);
        }

        public void Dispatch<T>(int eventId, T data) where T : IEventData
        {
            InternalDispatch(eventId, data, false, 0f, EventPriority.Normal);
        }

        public void DispatchDelayed(int eventId, float delay)
        {
            InternalDispatch(eventId, null, true, delay, EventPriority.Normal);
        }

        public void DispatchDelayed<T>(int eventId, T data, float delay) where T : IEventData
        {
            InternalDispatch(eventId, data, true, delay, EventPriority.Normal);
        }

        public void DispatchWithPriority(int eventId, EventPriority priority)
        {
            InternalDispatch(eventId, null, false, 0f, priority);
        }

        public void DispatchWithPriority<T>(int eventId, T data, EventPriority priority) where T : IEventData
        {
            InternalDispatch(eventId, data, false, 0f, priority);
        }

        private void InternalDispatch(int eventId, IEventData data, bool delayed, float delay, EventPriority priority)
        {
            if (delayed)
            {
                var queuedEvent = new QueuedEvent
                {
                    EventId = eventId,
                    Data = data,
                    Priority = priority,
                    Delay = delay,
                    CreatedTime = Time.time
                };

                if (_isProcessing)
                {
                    _pendingEvents.Add(queuedEvent);
                }
                else
                {
                    _eventQueue.Add(queuedEvent);
                }
                
                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Queued event {eventId} with {delay}s delay");
                }
                return;
            }

            if (_enableDebugLog)
            {
                LZDebug.Log($"[EventBus] Dispatching event {eventId}");
            }

            ProcessEvent(eventId, data);
        }

        private void ProcessEvent(int eventId, IEventData data)
        {
            if (!_listeners.TryGetValue(eventId, out var listeners))
                return;

            _isProcessing = true;
            _processingEvents.Add(eventId);

            try
            {
                var activeListeners = listeners.Where(l => !l.IsPaused).ToList();
                
                foreach (var wrapper in activeListeners)
                {
                    try
                    {
                        if (data == null)
                        {
                            ((EventCallback)wrapper.Callback)();
                        }
                        else
                        {
                            wrapper.Callback.DynamicInvoke(data);
                        }
                    }
                    catch (Exception ex)
                    {
                        LZDebug.LogError($"[EventBus] Exception in event handler: {ex}");
                    }
                }
            }
            finally
            {
                _processingEvents.Remove(eventId);
                _isProcessing = false;
                
                if (_pendingEvents.Count > 0)
                {
                    _eventQueue.AddRange(_pendingEvents);
                    _pendingEvents.Clear();
                    _eventQueue.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                }
            }
        }

        #endregion

        #region Update

        public void Update(float deltaTime)
        {
            if (_eventQueue.Count == 0)
                return;

            float currentTime = Time.time;
            var readyEvents = _eventQueue.Where(e => currentTime - e.CreatedTime >= e.Delay).ToList();

            foreach (var evt in readyEvents)
            {
                _eventQueue.Remove(evt);
                ProcessEvent(evt.EventId, evt.Data);
            }
        }

        #endregion

        #region Pause/Resume

        public void PauseEvent(int eventId)
        {
            if (_listeners.TryGetValue(eventId, out var listeners))
            {
                foreach (var wrapper in listeners)
                {
                    wrapper.IsPaused = true;
                }
                
                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Paused event {eventId}");
                }
            }
        }

        public void ResumeEvent(int eventId)
        {
            if (_listeners.TryGetValue(eventId, out var listeners))
            {
                foreach (var wrapper in listeners)
                {
                    wrapper.IsPaused = false;
                }
                
                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Resumed event {eventId}");
                }
            }
        }

        public void PauseAllEvents()
        {
            foreach (var listeners in _listeners.Values)
            {
                foreach (var wrapper in listeners)
                {
                    wrapper.IsPaused = true;
                }
            }
            
            if (_enableDebugLog)
            {
                LZDebug.Log("[EventBus] Paused all events");
            }
        }

        public void ResumeAllEvents()
        {
            foreach (var listeners in _listeners.Values)
            {
                foreach (var wrapper in listeners)
                {
                    wrapper.IsPaused = false;
                }
            }
            
            if (_enableDebugLog)
            {
                LZDebug.Log("[EventBus] Resumed all events");
            }
        }

        #endregion

        #region Debug

        public void SetDebugLog(bool enable)
        {
            _enableDebugLog = enable;
        }

        public int GetListenerCount(int eventId)
        {
            return _listeners.TryGetValue(eventId, out var listeners) ? listeners.Count : 0;
        }

        public List<int> GetActiveEventIds()
        {
            return new List<int>(_listeners.Keys);
        }

        #endregion
    }
}
