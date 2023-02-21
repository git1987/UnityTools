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
        /// transform的轴向朝向目标点
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="target">目标点</param>
        /// <param name="axis">轴向</param>
        public static void AxisLookAt(this Transform tran, Vector3 target, Vector3 axis)
        {
            Quaternion rotation = tran.rotation;
            Vector3 targetDir = target - tran.position;
            ///指定哪根轴朝向目标,自行修改Vector3的方向
            Vector3 fromDir = tran.rotation * axis;
            ///计算垂直于当前方向和目标方向的轴
            Vector3 direction = Vector3.Cross(fromDir, targetDir).normalized;
            ///计算当前方向和目标方向的夹角
            float angle = Vector3.Angle(fromDir, targetDir);
            ///将当前朝向向目标方向旋转一定角度，这个角度值可以做插值
            tran.rotation = Quaternion.AngleAxis(angle, direction) * rotation;
        }
        #endregion
    }
}
