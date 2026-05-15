using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lzengine
{
    public class LayeredEventBus : Singleton<LayeredEventBus>
    {
        private class ListenerWrapper
        {
            public Delegate Callback { get; set; }
            public int Priority { get; set; }
            public bool IsPaused { get; set; }
            public EventLayer Layer { get; set; }
        }

        private class QueuedEvent
        {
            public int EventId { get; set; }
            public IEventData Data { get; set; }
            public EventPriority Priority { get; set; }
            public float Delay { get; set; }
            public float CreatedTime { get; set; }
            public EventLayer TargetLayer { get; set; }
        }

        private readonly Dictionary<EventLayer, Dictionary<int, List<ListenerWrapper>>> _listeners = new Dictionary<EventLayer, Dictionary<int, List<ListenerWrapper>>>();
        private readonly Dictionary<int, List<ListenerWrapper>> _frameworkListeners = new Dictionary<int, List<ListenerWrapper>>();
        private readonly Dictionary<int, List<ListenerWrapper>> _gameListeners = new Dictionary<int, List<ListenerWrapper>>();
        private readonly List<QueuedEvent> _eventQueue = new List<QueuedEvent>();
        private readonly List<QueuedEvent> _pendingEvents = new List<QueuedEvent>();
        private bool _isProcessing;
        private bool _enableDebugLog = false;

        public LayeredEventBus()
        {
            _listeners[EventLayer.Framework] = _frameworkListeners;
            _listeners[EventLayer.Game] = _gameListeners;
        }

        #region Add Listeners

        public void AddListener(int eventId, EventCallback callback, EventPriority priority = EventPriority.Normal)
        {
            AddListener(eventId, callback, priority, EventLayer.Framework);
        }

        public void AddListener(int eventId, EventCallback callback, EventPriority priority, EventLayer layer)
        {
            InternalAddListener(eventId, callback, (int)priority, layer);
        }

        public void AddListener<T>(int eventId, EventCallback<T> callback, EventPriority priority = EventPriority.Normal) where T : IEventData
        {
            AddListener(eventId, callback, priority, EventLayer.Framework);
        }

        public void AddListener<T>(int eventId, EventCallback<T> callback, EventPriority priority, EventLayer layer) where T : IEventData
        {
            InternalAddListener(eventId, callback, (int)priority, layer);
        }

        private void InternalAddListener(int eventId, Delegate callback, int priority, EventLayer layer)
        {
            if (!_listeners.TryGetValue(layer, out var listeners))
            {
                listeners = new Dictionary<int, List<ListenerWrapper>>();
                _listeners[layer] = listeners;
            }

            if (!listeners.TryGetValue(eventId, out var callbacks))
            {
                callbacks = new List<ListenerWrapper>();
                listeners[eventId] = callbacks;
            }

            if (callbacks.Any(l => l.Callback == callback))
            {
                LZDebug.LogWarning($"Listener already registered for event {eventId} in layer {layer}");
                return;
            }

            callbacks.Add(new ListenerWrapper { Callback = callback, Priority = priority, IsPaused = false, Layer = layer });
            callbacks.Sort((a, b) => b.Priority.CompareTo(a.Priority));

            if (_enableDebugLog)
            {
                LZDebug.Log($"[EventBus] Added listener for event {eventId} in layer {layer}, total {callbacks.Count} listeners");
            }
        }

        #endregion

        #region Remove Listeners

        public void RemoveListener(int eventId, EventCallback callback)
        {
            RemoveListener(eventId, callback, EventLayer.Framework);
        }

        public void RemoveListener(int eventId, EventCallback callback, EventLayer layer)
        {
            InternalRemoveListener(eventId, callback, layer);
        }

        public void RemoveListener<T>(int eventId, EventCallback<T> callback) where T : IEventData
        {
            RemoveListener(eventId, callback, EventLayer.Framework);
        }

        public void RemoveListener<T>(int eventId, EventCallback<T> callback, EventLayer layer) where T : IEventData
        {
            InternalRemoveListener(eventId, callback, layer);
        }

        public void RemoveAllListeners(int eventId)
        {
            RemoveAllListeners(eventId, EventLayer.Framework);
            RemoveAllListeners(eventId, EventLayer.Game);
        }

        public void RemoveAllListeners(int eventId, EventLayer layer)
        {
            if (_listeners.TryGetValue(layer, out var listeners))
            {
                if (listeners.ContainsKey(eventId))
                {
                    listeners[eventId].Clear();
                    listeners.Remove(eventId);

                    if (_enableDebugLog)
                    {
                        LZDebug.Log($"[EventBus] Removed all listeners for event {eventId} in layer {layer}");
                    }
                }
            }
        }

        public void RemoveAllListeners(EventLayer layer)
        {
            if (_listeners.TryGetValue(layer, out var listeners))
            {
                listeners.Clear();
            }
            _eventQueue.RemoveAll(e => e.TargetLayer == layer);
        }

        public void RemoveAllListeners()
        {
            _frameworkListeners.Clear();
            _gameListeners.Clear();
            _eventQueue.Clear();
            _pendingEvents.Clear();

            if (_enableDebugLog)
            {
                LZDebug.Log("[EventBus] Removed all listeners from all layers");
            }
        }

        private void InternalRemoveListener(int eventId, Delegate callback, EventLayer layer)
        {
            if (!_listeners.TryGetValue(layer, out var listeners))
                return;

            if (!listeners.TryGetValue(eventId, out var callbacks))
                return;

            var wrapper = callbacks.FirstOrDefault(l => l.Callback == callback);
            if (wrapper != null)
            {
                callbacks.Remove(wrapper);

                if (callbacks.Count == 0)
                {
                    listeners.Remove(eventId);
                }

                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Removed listener for event {eventId} in layer {layer}");
                }
            }
        }

        #endregion

        #region Dispatch

        public void Dispatch(int eventId)
        {
            Dispatch(eventId, EventLayer.Framework);
        }

        public void Dispatch(int eventId, EventLayer targetLayer)
        {
            InternalDispatch(eventId, null, false, 0f, EventPriority.Normal, targetLayer);
        }

        public void Dispatch<T>(int eventId, T data) where T : IEventData
        {
            Dispatch(eventId, data, EventLayer.Framework);
        }

        public void Dispatch<T>(int eventId, T data, EventLayer targetLayer) where T : IEventData
        {
            InternalDispatch(eventId, data, false, 0f, EventPriority.Normal, targetLayer);
        }

        public void DispatchBothLayers(int eventId)
        {
            Dispatch(eventId, EventLayer.Framework);
            Dispatch(eventId, EventLayer.Game);
        }

        public void DispatchBothLayers<T>(int eventId, T data) where T : IEventData
        {
            Dispatch(eventId, data, EventLayer.Framework);
            Dispatch(eventId, data, EventLayer.Game);
        }

        public void DispatchDelayed(int eventId, float delay)
        {
            DispatchDelayed(eventId, delay, EventLayer.Framework);
        }

        public void DispatchDelayed(int eventId, float delay, EventLayer targetLayer)
        {
            InternalDispatch(eventId, null, true, delay, EventPriority.Normal, targetLayer);
        }

        public void DispatchDelayed<T>(int eventId, T data, float delay) where T : IEventData
        {
            DispatchDelayed(eventId, data, delay, EventLayer.Framework);
        }

        public void DispatchDelayed<T>(int eventId, T data, float delay, EventLayer targetLayer) where T : IEventData
        {
            InternalDispatch(eventId, data, true, delay, EventPriority.Normal, targetLayer);
        }

        public void DispatchWithPriority(int eventId, EventPriority priority)
        {
            DispatchWithPriority(eventId, priority, EventLayer.Framework);
        }

        public void DispatchWithPriority(int eventId, EventPriority priority, EventLayer targetLayer)
        {
            InternalDispatch(eventId, null, false, 0f, priority, targetLayer);
        }

        public void DispatchWithPriority<T>(int eventId, T data, EventPriority priority) where T : IEventData
        {
            DispatchWithPriority(eventId, data, priority, EventLayer.Framework);
        }

        public void DispatchWithPriority<T>(int eventId, T data, EventPriority priority, EventLayer targetLayer) where T : IEventData
        {
            InternalDispatch(eventId, data, false, 0f, priority, targetLayer);
        }

        private void InternalDispatch(int eventId, IEventData data, bool delayed, float delay, EventPriority priority, EventLayer targetLayer)
        {
            if (delayed)
            {
                var queuedEvent = new QueuedEvent
                {
                    EventId = eventId,
                    Data = data,
                    Priority = priority,
                    Delay = delay,
                    CreatedTime = Time.time,
                    TargetLayer = targetLayer
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
                    LZDebug.Log($"[EventBus] Queued event {eventId} for layer {targetLayer} with {delay}s delay");
                }
                return;
            }

            if (_enableDebugLog)
            {
                LZDebug.Log($"[EventBus] Dispatching event {eventId} to layer {targetLayer}");
            }

            ProcessEvent(eventId, data, targetLayer);
        }

        private void ProcessEvent(int eventId, IEventData data, EventLayer targetLayer)
        {
            if (!_listeners.TryGetValue(targetLayer, out var listeners))
                return;

            if (!listeners.TryGetValue(eventId, out var callbacks))
                return;

            _isProcessing = true;

            try
            {
                var activeListeners = callbacks.Where(l => !l.IsPaused).ToList();

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
                        LZDebug.LogError($"[EventBus] Exception in event handler for event {eventId}: {ex}");
                    }
                }
            }
            finally
            {
                _isProcessing = false;

                if (_pendingEvents.Count > 0)
                {
                    _eventQueue.AddRange(_pendingEvents);
                    _pendingEvents.Clear();
                    _eventQueue.Sort((a, b) => 
                    {
                        int layerCompare = a.TargetLayer.CompareTo(b.TargetLayer);
                        if (layerCompare != 0) return layerCompare;
                        return b.Priority.CompareTo(a.Priority);
                    });
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
                ProcessEvent(evt.EventId, evt.Data, evt.TargetLayer);
            }
        }

        #endregion

        #region Pause/Resume

        public void PauseEvent(int eventId)
        {
            PauseEvent(eventId, EventLayer.Framework);
            PauseEvent(eventId, EventLayer.Game);
        }

        public void PauseEvent(int eventId, EventLayer layer)
        {
            if (_listeners.TryGetValue(layer, out var listeners) && listeners.TryGetValue(eventId, out var callbacks))
            {
                foreach (var wrapper in callbacks)
                {
                    wrapper.IsPaused = true;
                }

                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Paused event {eventId} in layer {layer}");
                }
            }
        }

        public void ResumeEvent(int eventId)
        {
            ResumeEvent(eventId, EventLayer.Framework);
            ResumeEvent(eventId, EventLayer.Game);
        }

        public void ResumeEvent(int eventId, EventLayer layer)
        {
            if (_listeners.TryGetValue(layer, out var listeners) && listeners.TryGetValue(eventId, out var callbacks))
            {
                foreach (var wrapper in callbacks)
                {
                    wrapper.IsPaused = false;
                }

                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Resumed event {eventId} in layer {layer}");
                }
            }
        }

        public void PauseAllEvents()
        {
            PauseAllEvents(EventLayer.Framework);
            PauseAllEvents(EventLayer.Game);
        }

        public void PauseAllEvents(EventLayer layer)
        {
            if (_listeners.TryGetValue(layer, out var listeners))
            {
                foreach (var callbacks in listeners.Values)
                {
                    foreach (var wrapper in callbacks)
                    {
                        wrapper.IsPaused = true;
                    }
                }

                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Paused all events in layer {layer}");
                }
            }
        }

        public void ResumeAllEvents()
        {
            ResumeAllEvents(EventLayer.Framework);
            ResumeAllEvents(EventLayer.Game);
        }

        public void ResumeAllEvents(EventLayer layer)
        {
            if (_listeners.TryGetValue(layer, out var listeners))
            {
                foreach (var callbacks in listeners.Values)
                {
                    foreach (var wrapper in callbacks)
                    {
                        wrapper.IsPaused = false;
                    }
                }

                if (_enableDebugLog)
                {
                    LZDebug.Log($"[EventBus] Resumed all events in layer {layer}");
                }
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
            return GetListenerCount(eventId, EventLayer.Framework) + GetListenerCount(eventId, EventLayer.Game);
        }

        public int GetListenerCount(int eventId, EventLayer layer)
        {
            if (_listeners.TryGetValue(layer, out var listeners) && listeners.TryGetValue(eventId, out var callbacks))
            {
                return callbacks.Count;
            }
            return 0;
        }

        public List<int> GetActiveEventIds()
        {
            var frameworkIds = _frameworkListeners.Keys.ToList();
            var gameIds = _gameListeners.Keys.ToList();
            return frameworkIds.Union(gameIds).Distinct().ToList();
        }

        public List<int> GetActiveEventIds(EventLayer layer)
        {
            if (_listeners.TryGetValue(layer, out var listeners))
            {
                return new List<int>(listeners.Keys);
            }
            return new List<int>();
        }

        #endregion

        #region Bridge (跨层通信)

        public void RegisterCrossLayerListener(int eventId, EventCallback callback, EventLayer listenerLayer, EventPriority priority = EventPriority.Normal)
        {
            AddListener(eventId, callback, priority, listenerLayer);
        }

        public void DispatchCrossLayer(int eventId, EventLayer fromLayer, EventLayer toLayer)
        {
            Dispatch(eventId, toLayer);
        }

        public void DispatchCrossLayer<T>(int eventId, EventLayer fromLayer, EventLayer toLayer, T data) where T : IEventData
        {
            Dispatch(eventId, data, toLayer);
        }

        public void DispatchToAllLayers(int eventId)
        {
            DispatchBothLayers(eventId);
        }

        public void DispatchToAllLayers<T>(int eventId, T data) where T : IEventData
        {
            DispatchBothLayers(eventId, data);
        }

        #endregion
    }
}
