using UnityEngine;
namespace UnityTools
{
    /// <summary>
    /// 工具类静态方法
    /// </summary>
    public sealed class Tools
    {
        private Tools() { }
        /// <summary>
        /// 指定tran的轴向朝向目标点的Quaternion
        /// </summary>
        /// <param name="tran">Transform</param>
        /// <param name="target">目标点</param>
        /// <param name="axis">朝向目标点的轴向</param>
        /// <param name="relativeTo">坐标系</param>
        /// <returns></returns>
        static public Quaternion AxisLookAt(Transform tran, Vector3 target, Vector3 axis, Space relativeTo = Space.Self)
        {
            Vector3 targetDir = target - tran.position;
            //指定哪根轴朝向目标,自行修改Vector3的方向
            Vector3 fromDir;
            if (relativeTo == Space.Self)
                fromDir = tran.rotation * axis;
            else
                fromDir = axis;
            //计算垂直于当前方向和目标方向的轴
            Vector3 direction = Vector3.Cross(fromDir, targetDir).normalized;
            //计算当前方向和目标方向的夹角
            float angle = Vector3.Angle(fromDir, targetDir);
            //将当前朝向向目标方向旋转一定角度，这个角度值可以做插值
            return Quaternion.AngleAxis(angle, direction) * tran.rotation;
        }
        /// <summary>
        /// 设置RectTransform四周围绕适配
        /// </summary>
        /// <param name="rect"></param>
        static public void RectTransformSetSurround(RectTransform rect)
        {
            rect.anchorMin          = Vector2.zero;
            rect.anchorMax          = Vector2.one;
            rect.anchoredPosition3D = Vector3.zero;
            rect.sizeDelta          = Vector2.zero;
            rect.localScale         = Vector3.one;
            rect.localRotation      = Quaternion.identity;
        }
    }
}