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
        public int panelLv { set; get; }
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
        public virtual void Show() { gameObject.SetActive(true); }
        /// <summary>
        /// 关闭面板
        /// </summary>
        public virtual void Hide() { gameObject.SetActive(false); }
        /// <summary>
        /// 移除面板
        /// </summary>
        public virtual void Disable() { Destroy(this.gameObject); }
    }
}