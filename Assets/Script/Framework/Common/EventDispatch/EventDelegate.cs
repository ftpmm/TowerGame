using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class EventDelegate
    {
        public delegate void EventCallBack();
        public delegate void EventCallBack<T>(T t);
        public delegate void EventCallBack<T, U>(T t, U u);
        public delegate void EventCallBack<T,U,W>(T t, U u, W w);
    }
}
