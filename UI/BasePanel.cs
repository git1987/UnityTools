using UnityEngine;
namespace UnityTools.UI
{
    /// <summary>
    /// UI面板的基类
    /// </summary>
    public abstract class BasePanel : MonoBehaviour
    {
        /// <summary>
        /// 面板等级
        /// </summary>
        public int panelLv { private set; get; }
        /// <summary>
        /// 设置面板等级
        /// </summary>
        /// <param name="lv"></param>
        public void SetPanelLv(int lv) { panelLv = lv; }
        /// <summary>
        /// 面板打开BasePanel，直接调用UIManager.OpenPanel，面板等级==当前面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <returns></returns>
        protected P Open<P>() where P : BasePanel
        {
            if (panelLv == 0)
            {
                UnityTools.Debuger.LogError(GetType().Name + "面板没有在UIManager中初始化无法打开其他面板");
                panelLv = 1;
            }
            return UIManager.OpenPanel<P>(panelLv);
        }
        public abstract void Show();
        public abstract void Hide();
    }
}
