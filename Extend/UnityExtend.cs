using System;
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
        /// <summary>
        /// 根据名字获取子级的transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform GetChildByName(this Transform transform, string name)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.name == name)
                    return child;
                child = child.GetChildByName(name);
                if (child != null)
                    return child;
            }
            return null;
        }
        /// <summary>
        /// 遍历transform的子级
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="action">将子级作为参数的回调</param>
        /// <param name="includeGrandchildren">是否包含孙集</param>
        public static void ForAction(this Transform transform, EventAction<Transform> action, bool includeGrandchildren = false)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                action?.Invoke(child);
                if (includeGrandchildren)
                {
                    child.ForAction(action, includeGrandchildren);
                }
            }
        }
        #endregion
    }
}
