using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UnityEngine.UI;
namespace UnityEngine.UI
{
    [UnityEngine.RequireComponent(typeof(CanvasRenderer))]
    public class MaskGraphic : Graphic
    {
        public override void Rebuild(CanvasUpdate update)
        {
        }
    }
}
