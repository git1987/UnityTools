using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityTools
{
    /// <summary>
    /// 静态方法
    /// </summary>
    public class Tools
    {
        /// <summary>
        /// 指定tran的轴向朝向目标点的Quaternion
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="target"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        static public Quaternion AxisLookAt(Transform tran, Vector3 target, Vector3 axis)
        {
            Vector3 targetDir = target - tran.position;
            ///指定哪根轴朝向目标,自行修改Vector3的方向
            Vector3 fromDir = tran.rotation * axis;
            ///计算垂直于当前方向和目标方向的轴
            Vector3 direction = Vector3.Cross(fromDir, targetDir).normalized;
            ///计算当前方向和目标方向的夹角
            float angle = Vector3.Angle(fromDir, targetDir);
            ///将当前朝向向目标方向旋转一定角度，这个角度值可以做插值
            return Quaternion.AngleAxis(angle, direction) * tran.rotation;
        }
    }
}
