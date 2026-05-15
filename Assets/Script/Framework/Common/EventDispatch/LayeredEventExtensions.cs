using UnityEngine;

namespace lzengine
{
    public static class LayeredEventExtensions
    {
        public static void Emit(this int eventId)
        {
            LayeredEventBus.Instance.Dispatch(eventId);
        }

        public static void Emit(this int eventId, EventLayer layer)
        {
            LayeredEventBus.Instance.Dispatch(eventId, layer);
        }

        public static void Emit<T>(this int eventId, T data) where T : IEventData
        {
            LayeredEventBus.Instance.Dispatch(eventId, data);
        }

        public static void Emit<T>(this int eventId, T data, EventLayer layer) where T : IEventData
        {
            LayeredEventBus.Instance.Dispatch(eventId, data, layer);
        }

        public static void EmitBothLayers(this int eventId)
        {
            LayeredEventBus.Instance.DispatchBothLayers(eventId);
        }

        public static void EmitBothLayers<T>(this int eventId, T data) where T : IEventData
        {
            LayeredEventBus.Instance.DispatchBothLayers(eventId, data);
        }

        public static void EmitDelayed(this int eventId, float delay)
        {
            LayeredEventBus.Instance.DispatchDelayed(eventId, delay);
        }

        public static void EmitDelayed(this int eventId, float delay, EventLayer layer)
        {
            LayeredEventBus.Instance.DispatchDelayed(eventId, delay, layer);
        }

        public static void EmitDelayed<T>(this int eventId, T data, float delay) where T : IEventData
        {
            LayeredEventBus.Instance.DispatchDelayed(eventId, data, delay);
        }

        public static void EmitDelayed<T>(this int eventId, T data, float delay, EventLayer layer) where T : IEventData
        {
            LayeredEventBus.Instance.DispatchDelayed(eventId, data, delay, layer);
        }

        public static void Register(this int eventId, EventCallback callback, EventPriority priority = EventPriority.Normal)
        {
            LayeredEventBus.Instance.AddListener(eventId, callback, priority);
        }

        public static void Register(this int eventId, EventCallback callback, EventLayer layer, EventPriority priority = EventPriority.Normal)
        {
            LayeredEventBus.Instance.AddListener(eventId, callback, priority, layer);
        }

        public static void Register<T>(this int eventId, EventCallback<T> callback, EventPriority priority = EventPriority.Normal) where T : IEventData
        {
            LayeredEventBus.Instance.AddListener(eventId, callback, priority);
        }

        public static void Register<T>(this int eventId, EventCallback<T> callback, EventLayer layer, EventPriority priority = EventPriority.Normal) where T : IEventData
        {
            LayeredEventBus.Instance.AddListener(eventId, callback, priority, layer);
        }

        public static void Unregister(this int eventId, EventCallback callback)
        {
            LayeredEventBus.Instance.RemoveListener(eventId, callback);
        }

        public static void Unregister(this int eventId, EventCallback callback, EventLayer layer)
        {
            LayeredEventBus.Instance.RemoveListener(eventId, callback, layer);
        }

        public static void Unregister<T>(this int eventId, EventCallback<T> callback) where T : IEventData
        {
            LayeredEventBus.Instance.RemoveListener(eventId, callback);
        }

        public static void Unregister<T>(this int eventId, EventCallback<T> callback, EventLayer layer) where T : IEventData
        {
            LayeredEventBus.Instance.RemoveListener(eventId, callback, layer);
        }
    }
}
