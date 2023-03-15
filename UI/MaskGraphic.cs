using UnityEngine;
using UnityEngine.UI;
namespace UnityTools
{
    /// <summary>
    /// 不渲染的Graphic，raycastTarget有效
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class MaskGraphic : Graphic
    {
        public override void Rebuild(CanvasUpdate update) { }
        protected override void OnPopulateMesh(VertexHelper vh) { vh.Clear(); }
    }
}
