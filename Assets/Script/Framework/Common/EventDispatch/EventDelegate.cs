using System;

namespace lzengine
{
    public delegate void EventCallback();
    public delegate void EventCallback<in T>(T data) where T : IEventData;
}
