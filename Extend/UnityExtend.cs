using UnityEngine;
using UnityEngine.UI;
namespace UnityTools.Extend
{
    /// <summary>
    /// UnityEngine命名空间下类扩展方法
    /// </summary>
    public static class UnityEngineExtend
    {
        #region GameObject
        /// <summary>
        /// 获取Component，如果没有则Add一个
        /// </summary>
        /// <returns>UnityEngine.Component</returns>
        public static Component MateComponent(this GameObject go, Type type)
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
        public static T MateComponent<T>(this GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
                t = go.AddComponent<T>();
            return t;
        }
        #endregion
        #region Transform
        /// <summary>
        /// transform，如果是RectTransform，也重置anchoredPosition3D
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="parent">要设置的父级Transform</param>
        public static void SetParentReset(this Transform transform, Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            if (transform is RectTransform)
                ((RectTransform)transform).anchoredPosition3D = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        #endregion
    }
}
