using UnityEngine;
using UnityEngine.UI;
namespace UnityTools.UI
{
    /// <summary>
    /// 面板背景适配脚本：根据CanvasScaler设定的屏幕比进行满屏显示裁剪
    /// 使用ScaleWithScreenSize-Expand适配方式，背景图模拟Shrink适配方式
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class BGImageAdapter : MonoBehaviour
    {
        private void Start()
        {
            if (UIManager.uiCtrl == null)
            {
                Debug.LogError("没有UICtrl");
                return;
            }
            if (UIManager.uiCtrl.canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                Debug.LogWarning("没有使用ScaleWithScreenSize的ScaleMode类型");
                return;
            }
            else if (UIManager.uiCtrl.canvasScaler.screenMatchMode != CanvasScaler.ScreenMatchMode.Expand)
            {
                Debug.LogWarning("没有使用Expand的ScreenMatchMode类型");
                return;
            }
            float baseWidth  = UIManager.uiCtrl.canvasScaler.referenceResolution.x,
                  baseHeight = UIManager.uiCtrl.canvasScaler.referenceResolution.y;
            float UICanvasWidth = UIManager.uiCtrl.rect.sizeDelta.x,
                  UICanvasHeigh = UIManager.uiCtrl.rect.sizeDelta.y;
            float width, height;
            if (UICanvasWidth == baseWidth && UICanvasHeigh == baseHeight)
            {
                height = UICanvasHeigh;
                width  = UICanvasWidth;
            }
            else
            {
                if (UICanvasWidth == baseWidth)
                {
                    height = UICanvasHeigh;
                    width  = baseWidth * height / baseHeight;
                }
                else
                {
                    width  = UICanvasWidth;
                    height = baseHeight * width / baseWidth;
                }
            }
            (transform as RectTransform).sizeDelta = new(width, height);
        }
    }
}