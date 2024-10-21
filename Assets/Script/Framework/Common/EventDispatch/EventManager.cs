using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    /// <summary>
    /// 事件派发系统
    /// </summary>
    public class EventManager:Singleton<EventManager>
    {
        private Dictionary<int, Delegate> eventDict = new Dictionary<int, Delegate>();

        public void AddEventListener(int eventId, EventDelegate.EventCallBack callBack)
        {
            if (!eventDict.ContainsKey(eventId))
            {
                eventDict.Add(eventId, null);
            }

            Delegate d = eventDict[eventId];
            if (d != null && d.GetType() != callBack.GetType())
            {
                LZDebug.LogError(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventId, d.GetType().Name, callBack.GetType().Name));
            }
            else
            {
                eventDict[eventId] = (EventDelegate.EventCallBack)eventDict[eventId] + callBack;
            }
        }

        public void AddEventListener<T>(int eventId, EventDelegate.EventCallBack<T> callBack)
        {
            if (!eventDict.ContainsKey(eventId))
            {
                eventDict.Add(eventId, null);
            }

            Delegate d = eventDict[eventId];
            if (d != null && d.GetType() != callBack.GetType())
            {
                LZDebug.LogError(string.Format("AddEventListener failed event type {0}. Current listeners have type {1} and listener being added has type {2}", eventId, d.GetType().Name, callBack.GetType().Name));
            }
            else
            {
                eventDict[eventId] = (EventDelegate.EventCallBack<T>)eventDict[eventId] + callBack;
            }
        }

        public void AddEventListener<T,U>(int eventId, EventDelegate.EventCallBack<T, U> callBack)
        {
            if (!eventDict.ContainsKey(eventId))
            {
                eventDict.Add(eventId, null);
            }

            Delegate d = eventDict[eventId];
            if (d != null && d.GetType() != callBack.GetType())
            {
                LZDebug.LogError(string.Format("AddEventListener failed event type {0}. Current listeners have type {1} and listener being added has type {2}", eventId, d.GetType().Name, callBack.GetType().Name));
            }
            else
            {
                eventDict[eventId] = (EventDelegate.EventCallBack<T,U>)eventDict[eventId] + callBack;
            }
        }

        public void AddEventListener<T,U,W>(int eventId, EventDelegate.EventCallBack<T,U,W> callBack)
        {
            if (!eventDict.ContainsKey(eventId))
            {
                eventDict.Add(eventId, null);
            }

            Delegate d = eventDict[eventId];
            if (d != null && d.GetType() != callBack.GetType())
            {
                LZDebug.LogError(string.Format("AddEventListener failed event type {0}. Current listeners have type {1} and listener being added has type {2}", eventId, d.GetType().Name, callBack.GetType().Name));
            }
            else
            {
                eventDict[eventId] = (EventDelegate.EventCallBack<T,U,W>)eventDict[eventId] + callBack;
            }
        }

        public void RemoveEventListener(int eventId, EventDelegate.EventCallBack callBack)
        {
            if(eventDict.ContainsKey(eventId))
            {
                Delegate d = eventDict[eventId];
                if(d == null)
                {
                    LZDebug.LogError("RemoveEventListener failed!!! No Listen can Remove");
                }
                else if(d.GetType() != callBack.GetType())
                {
                    LZDebug.LogError("RemoveEventListener failed!!! Listener is Diff Type");
                }
                else
                {
                    eventDict[eventId] = (EventDelegate.EventCallBack)eventDict[eventId] - callBack;
                }

                if (eventDict[eventId] == null)
                {
                    eventDict.Remove(eventId);
                }
            }
            else
            {
                LZDebug.LogError("RemoveEventListener failed!!! dict has no the eventId=" + eventId);
            }
        }

        public void RemoveEventListener<T>(int eventId, EventDelegate.EventCallBack<T> callBack)
        {
            if (eventDict.ContainsKey(eventId))
            {
                Delegate d = eventDict[eventId];
                if (d == null)
                {
                    LZDebug.LogError("RemoveEventListener failed!!! No Listen can Remove");
                }
                else if (d.GetType() != callBack.GetType())
                {
                    LZDebug.LogError("RemoveEventListener failed!!! Listener is Diff Type");
                }
                else
                {
                    eventDict[eventId] = (EventDelegate.EventCallBack<T>)eventDict[eventId] - callBack;
                }

                if (eventDict[eventId] == null)
                {
                    eventDict.Remove(eventId);
                }
            }
            else
            {
                LZDebug.LogError("RemoveEventListener failed!!! dict has no the eventId=" + eventId);
            }
        }

        public void RemoveEventListener<T,U>(int eventId, EventDelegate.EventCallBack<T,U> callBack)
        {
            if (eventDict.ContainsKey(eventId))
            {
                Delegate d = eventDict[eventId];
                if (d == null)
                {
                    LZDebug.LogError("RemoveEventListener failed!!! No Listen can Remove");
                }
                else if (d.GetType() != callBack.GetType())
                {
                    LZDebug.LogError("RemoveEventListener failed!!! Listener is Diff Type");
                }
                else
                {
                    eventDict[eventId] = (EventDelegate.EventCallBack<T,U>)eventDict[eventId] - callBack;
                }

                if (eventDict[eventId] == null)
                {
                    eventDict.Remove(eventId);
                }
            }
            else
            {
                LZDebug.LogError("RemoveEventListener failed!!! dict has no the eventId=" + eventId);
            }
        }

        public void RemoveEventListener<T, U, W>(int eventId, EventDelegate.EventCallBack<T, U, W> callBack)
        {
            if (eventDict.ContainsKey(eventId))
            {
                Delegate d = eventDict[eventId];
                if (d == null)
                {
                    LZDebug.LogError("RemoveEventListener failed!!! No Listen can Remove");
                }
                else if (d.GetType() != callBack.GetType())
                {
                    LZDebug.LogError("RemoveEventListener failed!!! Listener is Diff Type");
                }
                else
                {
                    eventDict[eventId] = (EventDelegate.EventCallBack<T, U, W>)eventDict[eventId] - callBack;
                }

                if (eventDict[eventId] == null)
                {
                    eventDict.Remove(eventId);
                }
            }
            else
            {
                LZDebug.LogError("RemoveEventListener failed!!! dict has no the eventId=" + eventId);
            }
        }

        public void Dispatch(int eventId)
        {
            Delegate d;
            if(eventDict.TryGetValue(eventId, out d))
            {
                if(d != null)
                {
                    ((EventDelegate.EventCallBack)d)();
                }
            }
        }

        public void Dispatch<T>(int eventId, T t)
        {
            Delegate d;
            if (eventDict.TryGetValue(eventId, out d))
            {
                if(d != null)
                {
                    ((EventDelegate.EventCallBack<T>)d)(t);
                }
            }
        }

        public void Dispatch<T,U>(int eventId, T t, U u)
        {
            Delegate d;
            if (eventDict.TryGetValue(eventId, out d))
            {
                if (d != null)
                {
                    ((EventDelegate.EventCallBack<T, U>)d)(t, u);
                }
            }
        }

        public void Dispatch<T,U,W>(int eventId, T t, U u, W w)
        {
            Delegate d;
            if (eventDict.TryGetValue(eventId, out d))
            {
                if(d != null)
                {
                    ((EventDelegate.EventCallBack<T, U, W>)d)(t, u, w);
                }
            }
        }
    }
}
