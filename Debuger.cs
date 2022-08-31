using UnityEngine;
using Object = UnityEngine.Object;
namespace UnityTools
{
    /// <summary>
    /// 自定义Debug类
    /// </summary>
    public class Debuger
    {
        private static bool _enable = true;
        /// <summary>
        /// 输出日志开关
        /// </summary>
        public static bool enable { get { return _enable; } }
        /// <summary>
        /// 打开日志输出
        /// </summary>
        public static void Enable() { _enable = true; }
        /// <summary>
        /// 关闭日志输出
        /// </summary>
        public static void UnEnable() { _enable = false; }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void Log(object message)
        {
            Debug.Log(message);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogFormat(string format, params object[] args)
        {
            Debug.LogFormat( format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogFormat(Object context, string format, params object[] args)
        {
            Debug.LogFormat(context, format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
        {
            Debug.LogFormat(logType, logOptions, context, format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogError(object message, Object context)
        {
            Debug.LogError(message, context);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogErrorFormat(string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            Debug.LogErrorFormat(context, format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogException(Exception exception, Object context)
        {
            Debug.LogException(exception, context);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogWarning(object message, Object context)
        {
            Debug.LogWarning(message, context);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogWarningFormat(string format, params object[] args)
        {
            Debug.LogWarningFormat(format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            Debug.LogWarningFormat(context, format, args);
        }
    }
    }