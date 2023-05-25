using System;
using UnityEngine;
using Object = UnityEngine.Object;
namespace UnityTools
{
    /// <summary>
    /// 自定义Debug类
    /// </summary>
    public class Debuger
    {
        /// <summary>
        /// 输出日志开关
        /// </summary>
        public static bool disable { private set; get; }
        /// <summary>
        /// 打开日志输出
        /// </summary>
        public static void Enable() { disable = false; }
        /// <summary>
        /// 关闭日志输出
        /// </summary>
        public static void UnEnable() { disable = true; }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void Log(object message)
        {
            if (!disable) Debug.Log(message);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void Log(object message, Object context)
        {
            if (!disable) Debug.Log(message, context);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogFormat(string format, params object[] args)
        {
            if (!disable) Debug.LogFormat(format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogFormat(Object context, string format, params object[] args)
        {
            if (!disable) Debug.LogFormat(context, format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
        {
            if (!disable) Debug.LogFormat(logType, logOptions, context, format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogError(object message)
        {
            if (!disable) Debug.LogError(message);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogError(object message, Object context)
        {
            if (!disable) Debug.LogError(message, context);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogErrorFormat(string format, params object[] args)
        {
            if (!disable) Debug.LogErrorFormat(format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            if (!disable) Debug.LogErrorFormat(context, format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogException(Exception exception)
        {
            if (!disable) Debug.LogException(exception);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogException(Exception exception, Object context)
        {
            if (!disable) Debug.LogException(exception, context);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogWarning(object message)
        {
            if (!disable) Debug.LogWarning(message);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogWarning(object message, Object context)
        {
            if (!disable) Debug.LogWarning(message, context);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogWarningFormat(string format, params object[] args)
        {
            if (!disable) Debug.LogWarningFormat(format, args);
        }
        /// <summary>
        /// 参考UnityEngine.Debug
        /// </summary>
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            if (!disable) Debug.LogWarningFormat(context, format, args);
        }
    }
}