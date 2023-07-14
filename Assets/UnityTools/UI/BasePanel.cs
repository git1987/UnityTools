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
        public void SetPanelLv(int lv)
        {
            if (lv != this.panelLv)
            {
                panelLv = lv;
                UIManager.SetPanelLv(this, lv);
            }
        }
        /// <summary>
        /// 面板打开BasePanel，直接调用UIManager.OpenPanel，面板等级==当前面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <returns></returns>
        protected P Open<P>() where P : BasePanel
        {
            if (panelLv != 0) return UIManager.OpenPanel<P>(panelLv);
            Debuger.LogError(this.GetType().Name + "面板没有在UIManager中打开过，没有设置面板层级，无法打开其他面板");
            return UIManager.OpenPanel<P>(1);
        }
        /// <summary>
        /// 打开面板
        /// </summary>
        public abstract void Show();
        /// <summary>
        /// 关闭面板
        /// </summary>
        public abstract void Hide();
    }
}