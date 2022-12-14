namespace UnityTools.Extend
{
    /// <summary>
    /// UnityEngine命名空间下类扩展方法
    /// </summary>
    public static class UnityEngineExtend
    {
        /// <summary>
        /// 获取Component，如果没有则Add一个
        /// </summary>
        /// <returns>UnityEngine.Component</returns>
        public static UnityEngine.Component MateComponent(this UnityEngine.GameObject go, System.Type type)
        {
            UnityEngine.Component t = go.GetComponent(type);
            if (t == null)
                t = go.AddComponent(type);
            return t;
        }
        /// <summary>
        /// 获取Component，如果没有则Add一个
        /// </summary>
        /// <returns>T</returns>
        public static T MateComponent<T>(this UnityEngine.GameObject go) where T : UnityEngine.Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
                t = go.AddComponent<T>();
            return t;
        }
    }
}
