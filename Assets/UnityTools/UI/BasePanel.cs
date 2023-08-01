using UnityEngine;
namespace UnityTools.UI
{
    /// <summary>
    /// UI面板的基类
    /// </summary>
    public abstract class BasePanel : MonoBehaviour
    {
        /// <summary>
        /// 面板等级：默认等级是1级，特殊需求放在面板最底层，则设置为0级
        /// </summary>
        public int panelLv;
        /// <summary>
        /// 面板打开BasePanel，直接调用UIManager.OpenPanel，面板等级==当前面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <returns></returns>
        protected P Open<P>() where P : BasePanel
        {
            if (panelLv > 0) return UIManager.OpenPanel<P>(panelLv);
            Debuger.LogError(this.GetType().Name + "面板创建后没有在UIManager中打开过，可能没有初始化");
            return UIManager.OpenPanel<P>();
        }
        /// <summary>
        /// 显示面板
        /// </summary>
        public abstract void Show();
        /// <summary>
        /// 隐藏面板
        /// </summary>
        public abstract void Hide();
    }
}