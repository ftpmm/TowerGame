using UnityEngine;

namespace lzengine
{
    public static class EventExtensions
    {
        public static void Emit(this int eventId)
        {
            EventBus.Instance.Dispatch(eventId);
        }

        public static void Emit<T>(this int eventId, T data) where T : IEventData
        {
            EventBus.Instance.Dispatch(eventId, data);
        }

        public static void EmitDelayed(this int eventId, float delay)
        {
            EventBus.Instance.DispatchDelayed(eventId, delay);
        }

        public static void EmitDelayed<T>(this int eventId, T data, float delay) where T : IEventData
        {
            EventBus.Instance.DispatchDelayed(eventId, data, delay);
        }

        public static void Register(this int eventId, EventCallback callback, EventPriority priority = EventPriority.Normal)
        {
            EventBus.Instance.AddListener(eventId, callback, priority);
        }

        public static void Register<T>(this int eventId, EventCallback<T> callback, EventPriority priority = EventPriority.Normal) where T : IEventData
        {
            EventBus.Instance.AddListener(eventId, callback, priority);
        }

        public static void Unregister(this int eventId, EventCallback callback)
        {
            EventBus.Instance.RemoveListener(eventId, callback);
        }

        public static void Unregister<T>(this int eventId, EventCallback<T> callback) where T : IEventData
        {
            EventBus.Instance.RemoveListener(eventId, callback);
        }
    }
}
