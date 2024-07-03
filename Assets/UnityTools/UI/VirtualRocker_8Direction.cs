#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace UnityTools.UI
{
    [AddComponentMenu("UnityTools/UI/8方向虚拟摇杆")]
    public partial class VirtualRocker_8Direction : VirtualRocker
    {
        public override Vector2 Direction { get; }
        protected override void ResetRocker() { throw new System.NotImplementedException(); }
    }
}