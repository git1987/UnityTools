using System;
using System.Collections.Generic;
using UnityEngine;
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
        private static readonly Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();

        /// <summary>
        /// 当前场景的UICtrl
        /// </summary>

        public static UICtrl uiCtrl { private set; get; }

        /// <summary>
        /// 根据面板名称的全称自动加载panel prefab的委托
        /// </summary>
        private static EventFunction<string, GameObject> getPanelPrefabFunction = null;

        /// <summary>
        /// 设置自动加载panel prefab的委托
        /// </summary>
        /// <param name="function"></param>
        public static void SetBuildPanelFunction(EventFunction<string, GameObject> function)
        {
            getPanelPrefabFunction = function;
        }
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
            else { Debuger.LogError("之前场景没有清空UICtrl" + uiCtrl.GetType().Name); }
            uiCtrl = _uiCtrl;
        }
        public static void RemoveUICtrl(UICtrl currentUICtrl)
        {
            if (currentUICtrl == null) { Debuger.LogError("currentUICtrl is null"); }
            else if (uiCtrl == null) { Debuger.LogError("UIManager.uiCtrl是空的，当前场景没有注册UICtrl"); }
            else if (uiCtrl != currentUICtrl) { Debuger.LogError("当前场景uiCtrl和UIManger.uiCtrl不相同"); }
            uiCtrl = null;
        }
        /// <summary>
        /// 泛型获取当前场景的UICtrl
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <returns></returns>
        public static C GetUICtrl<C>() where C : UICtrl
        {
            if (uiCtrl is C) { return uiCtrl as C; }
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
        public static P CreatePanel<P>(GameObject panelPrefab) where P : BasePanel
        {
            P p;
            string panelName = typeof(P).Name;
            if (panels.TryGetValue(panelName, out BasePanel basePanel)) { p = basePanel as P; }
            else
            {
                if (panelPrefab == null) { throw new NullReferenceException($"panelPrefab is null!"); }
                GameObject panelObj = GameObject.Instantiate<GameObject>(panelPrefab);
                panelObj.SetActive(false);
                p = panelObj.GetComponent<P>();
                if (p == null) { throw new NullReferenceException($"{panelName} is null!"); }
                panels.Add(panelName, p);
                RectTransform rect = panelObj.GetComponent<RectTransform>();
                rect.SetParentReset(uiCtrl.rect);
                Tools.RectTransformSetSurround(rect);
            }
            return p;
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        /// <typeparam name="P">面板类</typeparam>
        /// <param name="panelLv">设置的面板等级</param>
        /// <returns></returns>
        public static P OpenPanel<P>(int panelLv = 1) where P : BasePanel
        {
            P panel = null;
            string panelName = typeof(P).Name;
            if (panels.TryGetValue(panelName, out BasePanel basePanel)) { panel = basePanel as P; }
            else
            {
                if (getPanelPrefabFunction == null) { Debuger.LogError("没有设置自动加载panel prefab的委托"); }
                else { panel = CreatePanel<P>(getPanelPrefabFunction(panelName)); }
            }
            if (panel != null)
            {
                Transform panelParent = GetPanelParent(panelLv);
                panel.transform.SetParent(panelParent);
                panel.transform.SetAsLastSibling();
                panel.gameObject.SetActive(true);
                panel.Show();
                panel.SetPanelLv(panelLv);
            }
            else { UnityTools.Debuger.LogError($"{panelName}不存在"); }
            return panel;
        }
        /// <summary>
        /// 获取面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <returns></returns>
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
                        Tools.RectTransformSetSurround(rect);
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