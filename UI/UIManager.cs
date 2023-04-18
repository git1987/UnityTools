using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityTools.Extend;

namespace UnityTools.UI
{
    /// <summary>
    /// UI总控制器
    /// </summary>
    public sealed class UIManager
    {
        /// <summary>
        /// 当前场景中的所有 UIPanel集合
        /// </summary>
        private static Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();
        /// <summary>
        /// 当前场景的UICtrl
        /// </summary>

        public static UICtrl uiCtrl { private set; get; }
        /// <summary>
        /// 设置当前场景的UICtrl
        /// </summary>
        /// <param name="_uiCtrl"></param>
        public static void SetUICtrl(UICtrl _uiCtrl)
        {
            if (uiCtrl == null)
            {
                //设置UICtrl时进入到新场景，将旧的面板词典清空
                panels.Clear();
            }
            else
            {
                Debuger.LogError("之前场景没有清空UICtrl" + uiCtrl.GetType().Name);
            }
            uiCtrl = _uiCtrl;
        }
        public static void RemoveUICtrl(UICtrl currentUICtrl)
        {
            if (currentUICtrl == null)
            {
                Debuger.LogError("currentUICtrl is null");
            }
            else if (uiCtrl == null)
            {
                Debuger.LogError("UIManager.uiCtrl是空的，当前场景没有注册UICtrl");
            }
            else if (uiCtrl != currentUICtrl)
            {
                Debuger.LogError("当前场景uiCtrl和UIMangaer.uiCtrl不相同");
            }
            uiCtrl = null;
        }
        /// <summary>
        /// 泛型获取当前场景的UICtrl
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <returns></returns>
        public static C GetUICtrl<C>() where C : UICtrl
        {
            if (uiCtrl is C)
            {
                return uiCtrl as C;
            }
            else
            {
                Debuger.LogError($"场景中的UICtrl[{uiCtrl.GetType().Name}]不是{typeof(C).Name}");
                return null;
            }
        }
        /// <summary>
        /// 创建面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="panelPrefab"></param>
        public static void CreatePanel<P>(GameObject panelPrefab) where P : BasePanel
        {
            string panelName = typeof(P).Name;
            if (!panels.ContainsKey(panelName))
            {
                GameObject panelObj = GameObject.Instantiate<GameObject>(panelPrefab);
                if (panelObj != null)
                {
                    panelObj.SetActive(false);
                    panels.Add(panelName, panelObj.GetComponent<P>());
                    RectTransform rect = panelObj.GetComponent<RectTransform>();
                    rect.SetParentReset(uiCtrl.rect);
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;
                    rect.localScale = Vector3.one;
                }
            }
        }
        /// <summary>
        /// 打开面板
        /// </summary>
        /// <typeparam name="P">面板类</typeparam>
        /// <param name="panelLv">设置的面板等级</param>
        /// <returns></returns>
        public static P OpenPanel<P>(int panelLv = 1) where P : BasePanel
        {
            string panelName = typeof(P).Name;
            if (!panels.ContainsKey(panelName))
            {
                //CreatePanel<P>();
            }
            P panel = panels[panelName] as P;
            Transform panelParent = GetPanelParent(panelLv);
            panel.transform.SetParent(panelParent);
            panel.transform.SetAsLastSibling();
            panel.gameObject.SetActive(true);
            panel.Show();
            panel.SetPanelLv(panelLv);
            return panel;
        }
        public static P GetPanel<P>() where P : BasePanel
        {
            string panelName = typeof(P).Name;
            if (panels.ContainsKey(panelName))
                return panels[panelName] as P;
            Debuger.LogFormat("没有初始化{0}面板", panelName);
            return null;
        }
        static RectTransform GetPanelParent(int panelLv)
        {
            Transform panelParent = uiCtrl.rect.Find("Panel" + panelLv);
            if (panelParent == null)
            {
                int index = 1;
                while (index <= panelLv)
                {
                    if (index == panelLv || uiCtrl.rect.Find("Panel" + index) == null)
                    {
                        GameObject go = new GameObject("Panel" + index);
                        RectTransform rect = go.AddComponent<RectTransform>();
                        rect.SetParentReset(uiCtrl.rect);
                        rect.anchorMin = Vector2.zero;
                        rect.anchorMax = Vector2.one;
                        rect.offsetMin = Vector2.zero;
                        rect.offsetMax = Vector2.zero;
                    }
                    index++;
                }
                panelParent = uiCtrl.rect.Find("Panel" + panelLv);
            }
            if (panelParent == null) Debuger.LogError("面板层级错误：" + panelLv);
            return panelParent as RectTransform;
        }
    }
}
