using UnityEngine;

namespace UnityTools.UI
{
    /// <summary>
    /// 面板背景适配脚本：根据CanvasScaler设定的屏幕比进行满屏显示裁剪
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class BGImageAdapter : MonoBehaviour
    {
        private void Start()
        {
            if (UIManager.uiCtrl == null)
            {
                Debug.LogError("没有UICtrl");
                return;
            }
            float screenWidth = Screen.width,
                  screenHeight = Screen.height;
            //当前屏幕分辨率
            float screenRatio = screenWidth / screenHeight;
            float baseWidth = UIManager.uiCtrl.canvasScaler.referenceResolution.x,
                  baseHeight = UIManager.uiCtrl.canvasScaler.referenceResolution.y;
            ///根据CanvasScaler计算标准屏幕比
            float baseRatio = baseWidth / baseHeight;
            float x, y;
            if (screenRatio > 1)
            {
                //横屏
                if (screenRatio < baseRatio)
                {
                    //长屏：竖向变长
                    x = baseWidth * screenRatio;
                    y = baseHeight * screenRatio;
                }
                else if (screenRatio > baseRatio)
                {
                    //宽屏：横向边长
                    x = baseHeight * screenRatio;
                    y = baseHeight * x / baseWidth;
                }
                else
                {
                    //等比
                    x = baseWidth;
                    y = baseHeight;
                }
            }
            else
            {
                //竖屏
                if (screenRatio < baseRatio)
                {
                    //长屏：竖向变长
                    y = baseHeight / screenRatio;
                    x = baseWidth * y / baseHeight;
                }
                else if (screenRatio > baseRatio)
                {
                    //宽屏：横向边长
                    x = screenWidth;
                    y = x / baseRatio;
                }
                else
                {
                    //等比
                    x = baseWidth;
                    y = baseHeight;
                }
            }
            (transform as RectTransform).sizeDelta = new(x, y);
        }
    }
}