﻿using System;
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
        private static EventFunction<string, GameObject> loadPanelPrefabFunction = null;
        /// <summary>
        /// 根据面板名称的实例化的panel的委托
        /// </summary>
        private static EventFunction<string, GameObject> createPanelFunction = null;
        /// <summary>
        /// 已经显示的panel
        /// </summary>
        private static Dictionary<string, BasePanel> showPanels = new();
        /// <summary>
        /// 是否设置面板级别
        /// </summary>
        public static bool setPanelLv = true;
        /// <summary>
        /// 设置自动加载panel prefab的委托
        /// </summary>
        /// <param name="function"></param>
        public static void SetLoadPrefabFunction(EventFunction<string, GameObject> function)
        {
            loadPanelPrefabFunction = function;
        }
        /// <summary>
        /// 设置获取panel实例的委托
        /// </summary>
        /// <param name="function"></param>
        public static void SetCreatePanelFunction(EventFunction<string, GameObject> function)
        {
            createPanelFunction = function;
        }
        /// <summary>
        /// 移除面板时的委托
        /// </summary>
        private static event EventAction<string> removePanelAction;
        /// <summary>
        /// 设置移除panel的委托
        /// </summary>
        /// <param name="function"></param>
        public static void AddRemovePanelAction(EventAction<string> action)
        {
            removePanelAction += action;
        }
        public static void RemoveRemovePanelAction(EventAction<string> action)
        {
            removePanelAction -= action;
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
            else
            {
                Debuger.LogError($"之前场景没有清空UICtrl[{uiCtrl}]", uiCtrl.gameObject);
            }
            uiCtrl = _uiCtrl;
        }
        /// <summary>
        /// 移除场景UICtrl控制器
        /// </summary>
        /// <param name="currentUICtrl"></param>
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
                Debuger.LogError("当前场景uiCtrl和UIManger.uiCtrl不相同");
            }
            uiCtrl = null;
            BaseModel.ClearModel();
        }
        /// <summary>
        /// 设置面板为关闭状态
        /// </summary>
        /// <param name="panel"></param>
        public static void SetHidePanel(BasePanel panel)
        {
            if (showPanels.ContainsKey(panel.PanelName))
            {
                showPanels.Remove(panel.PanelName);
            }
        }
        /// <summary>
        /// 获取当前显示的所有面板
        /// </summary>
        /// <returns></returns>
        public static List<BasePanel> GetShowPanelList()
        {
            if (showPanels.Count == 0) return null;
            List<BasePanel> list = new(showPanels.Values);
            return list;
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
        /// 预加载面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        public static void CreatePanel<P>() where P : BasePanel
        {
            string panelName = typeof(P).Name;
            CreatePanel(panelName);
        }
        /// <summary>
        /// 预加载面板
        /// </summary>
        /// <param name="panelName"></param>
        public static void CreatePanel(string panelName)
        {
            if (panels.ContainsKey(panelName))
            {
                //已经加载了
                return;
            }
            GameObject panelObj;
            if (loadPanelPrefabFunction != null)
            {
                panelObj = GameObject.Instantiate(loadPanelPrefabFunction(panelName));
            }
            else if (createPanelFunction != null)
            {
                panelObj = createPanelFunction(panelName);
            }
            else
            {
                Debug.LogError($"无法自行创建{panelName}面板");
                return;
            }
            panelObj.name = panelName;
            CreatePanel(panelObj);
        }
        /// <summary>
        /// 创建面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="panelObj"></param>
        private static BasePanel CreatePanel(GameObject panelObj)
        {
            if (panelObj == null)
            {
                throw new NullReferenceException($"panel GameObejct is null!");
            }
            string    panelName = panelObj.name;
            BasePanel p;
            if (panels.TryGetValue(panelName, out BasePanel basePanel))
            {
                p = basePanel;
            }
            else
            {
                panelObj.SetActive(false);
                p = panelObj.GetComponent<BasePanel>();
                if (p == null)
                {
                    throw new NullReferenceException($"[{panelName}] Component is null!");
                }
                panels.Add(panelName, p);
                RectTransform rect = panelObj.GetComponent<RectTransform>();
                rect.SetParentReset(uiCtrl.rect);
                rect.SetSurround();
            }
            return p;
        }
        /// <summary>
        /// 打开面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <returns></returns>
        public static P OpenPanel<P>(params object[] objs) where P : BasePanel
        {
            string panelName = typeof(P).Name;
            if (IsOpen(panelName))
            {
                Debug.LogWarning($"{panelName}面板已经打开");
                return GetPanel(panelName) as P;
            }
            P panel = OpenPanel(panelName, objs) as P;
            return panel;
        }
        /// <summary>
        /// 根据面板名称打开面板
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        public static BasePanel OpenPanel(string panelName, params object[] objs)
        {
            if (IsOpen(panelName))
            {
                Debug.LogWarning($"{panelName}面板已经打开");
                return GetPanel(panelName);
            }
            BasePanel panel;
            if (!panels.TryGetValue(panelName, out panel))
            {
                CreatePanel(panelName);
                panel = panels[panelName];
            }
            showPanels.Add(panelName, panel);
            panel.OpenWithManager(objs);
            if (setPanelLv)
            {
                panel.SetPanelLv();
            }
            else
                setPanelLv = true;
            return panel;
        }
        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="p"></param>
        public static void ClosePanel<P>() where P : BasePanel
        {
            ClosePanel(typeof(P).Name);
        }
        public static void ClosePanel(string panelName)
        {
            if (panels.TryGetValue(panelName, out BasePanel panel))
            {
                panel.CloseWithManager();
            }
        }
        /// <summary>
        /// 获取面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <returns></returns>
        public static P GetPanel<P>() where P : BasePanel
        {
            string panelName = typeof(P).Name;
            if (panels.ContainsKey(panelName)) return panels[panelName] as P;
            Debuger.LogFormat("没有初始化{0}面板", panelName);
            return null;
        }
        /// <summary>
        /// 获取面板
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        public static BasePanel GetPanel(string panelName)
        {
            panels.TryGetValue(panelName, out BasePanel panel);
            if (panel == null) Debuger.LogError($"没有初始化{0}面板");
            return panel;
        }
        /// <summary>
        /// 判断面板是否打开中
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <returns></returns>
        public static bool IsOpen<P>()
        {
            return IsOpen(typeof(P).Name);
        }
        /// <summary>
        /// 判断面板是否打开中
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        public static bool IsOpen(string panelName)
        {
            return showPanels.ContainsKey(panelName);
        }
        /// <summary>
        /// 移除面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        public static void RemovePanel<P>() where P : BasePanel
        {
            string panelName = typeof(P).Name;
            RemovePanel(panelName);
        }
        /// <summary>
        /// 移除面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        public static void RemovePanel(string panelName)
        {
            if (panels.TryGetValue(panelName, out BasePanel panel))
            {
                removePanelAction?.Invoke(panelName);
                panels.Remove(panelName);
                panel.Disable();
            }
        }
        /// <summary>
        /// 设置面板等级
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="panelLv"></param>
        public static void SetPanelLv(BasePanel panel, int panelLv)
        {
            Transform panelParent = GetPanelParent(panelLv);
            panel.transform.SetParent(panelParent);
            panel.transform.SetAsLastSibling();
        }
        static RectTransform GetPanelParent(int panelLv)
        {
            Transform panelParent = uiCtrl.rect.Find("Panel" + panelLv);
            if (panelParent == null)
            {
                int index = 0;
                while (index <= panelLv)
                {
                    if (index == panelLv || uiCtrl.rect.Find("Panel" + index) == null)
                    {
                        GameObject    go   = new GameObject("Panel" + index);
                        RectTransform rect = go.AddComponent<RectTransform>();
                        rect.SetParentReset(uiCtrl.rect);
                        rect.SetSurround();
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