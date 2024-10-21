using UnityEngine;

namespace lzengine
{
    public class LZDebug
    {
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void LogFormat(string message, params object[] args)
        {
            Debug.LogFormat(message, args);
        }

        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        public static void LogErrorFormat(string message, params object[] args)
        {
            Debug.LogErrorFormat(message, args);
        }

        public static void LogWarning(string message) 
        {
            Debug.LogWarning(message);
        }
    }
}
