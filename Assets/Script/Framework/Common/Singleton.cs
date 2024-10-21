using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lzengine
{
    public class Singleton<T> where T:class,new()
    {
        private static T _instance;

#if !UNITY_WEBGL
        private static readonly object _sysLock = new object();
#endif

        public static T Instance { 
            get {
#if !UNITY_WEBGL
                lock (_sysLock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
#else
                if (_instance == null)
                {
                    _instance = new T();
                }
#endif
                return _instance; 
            } 
        }
    }
}
