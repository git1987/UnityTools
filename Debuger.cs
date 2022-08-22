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
    }
}