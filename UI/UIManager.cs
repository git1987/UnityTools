using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityTools.Extend;

namespace UnityTools.UI
{
    /// <summary>
    /// UI总控制器
    /// </summary>
    public sealed class UIManager
    {
        static UIManager()
        {

        }
        /// UIPanel集合
        private static Dictionary<string, BasePanel> panels;

        public static UICtrl uiCtrl { private set; get; }
        public static void SetUICtrl(UICtrl _uiCtrl)
        {
            if (uiCtrl != null)
            {
                UnityTools.Debuger.LogError("之前场景没有清空UI控制器" + uiCtrl.GetType().Name);
            }
            uiCtrl = _uiCtrl;
        }
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
        public static void CreatePanel<P>(GameObject panelPrefab) where P : BasePanel
        {
            string panelName = typeof(P).Name;
            if (!panels.ContainsKey(panelName))
            {
                //GameObject panelObj = ResManager.BuildUIPanel<P>();
                GameObject panelObj = GameObject.Instantiate(panelPrefab);
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
            if (panelParent == null) UnityTools.Debuger.LogError("面板层级错误：" + panelLv);
            return panelParent as RectTransform;
        }
        public static P GetPanel<P>() where P : BasePanel
        {
            string panelName = typeof(P).Name;
            if (panels.ContainsKey(panelName))
                return panels[panelName] as P;
            UnityTools.Debuger.LogFormat("没有初始化{0}面板", panelName);
            return null;
        }
    }
}
