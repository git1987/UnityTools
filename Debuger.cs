using UnityEngine;
using Object = UnityEngine.Object;
namespace UnityTools
{
    public class Debuger
    {
        private static bool _enable = true;
        public static bool enable { get { return _enable; } }
        public static void Enable() { _enable = true; }
        public static void UnEnable() { _enable = false; }
        public static void Log(object message)
        {
            Debug.Log(message);
        }
        public static void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }
        public static void LogFormat(string format, params object[] args)
        {
            Debug.LogFormat( format, args);
        }
        public static void LogFormat(Object context, string format, params object[] args)
        {
            Debug.LogFormat(context, format, args);
        }
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
        {
            Debug.LogFormat(logType, logOptions, context, format, args);
        }
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
        public static void LogError(object message, Object context)
        {
            Debug.LogError(message, context);
        }
        public static void LogErrorFormat(string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args);
        }
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            Debug.LogErrorFormat(context, format, args);
        }
        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
        public static void LogException(Exception exception, Object context)
        {
            Debug.LogException(exception, context);
        }
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
        public static void LogWarning(object message, Object context)
        {
            Debug.LogWarning(message, context);
        }
        public static void LogWarningFormat(string format, params object[] args)
        {
            Debug.LogWarningFormat(format, args);
        }
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            Debug.LogWarningFormat(context, format, args);
        }
    }
    }