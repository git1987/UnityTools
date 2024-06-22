using System;
using System.Collections.Generic;
using UnityEngine;
using UnityTools.Extend;
namespace UnityTools.UI
{
    /// <summary>
    /// 显示层：MVC中的V
    /// <para>UI面板的基类</para>
    /// </summary>
    public abstract class BasePanel : MonoBehaviour
    {
        /// <summary>
        /// 面板是否是显示状态
        /// </summary>
        public bool isShow { protected set; get; }
        public virtual string PanelName => GetType().Name;
        /// <summary>
        /// 设置的面板等级
        /// </summary>
        public int panelLv = 1;
        /// <summary>
        /// 隐藏面板前显示状态的GameObject
        /// </summary>
        private GameObject[] hideObjs;
        protected virtual void Awake()
        {
            hideObjs = new GameObject[transform.childCount];
        }
        public virtual void SetPanelLv()
        {
            UIManager.SetPanelLv(this, panelLv);
        }
        /// <summary>
        /// 由UIManager驱动：打开并显示面板
        /// </summary>
        /// <param name="objs"></param>
        public void OpenWithManager(params object[] objs)
        {
            Open(objs);
        }
        /// <summary>
        /// 打开并显示面板
        /// </summary>
        protected virtual void Open(params object[] objs)
        {
            isShow = true;
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 由UIManager驱动：关闭并隐藏面板
        /// </summary>
        public void CloseWithManager()
        {
            Close();
        }
        /// <summary>
        /// 关闭并隐藏面板
        /// </summary>
        protected virtual void Close()
        {
            isShow = false;
            gameObject.SetActive(false);
            UIManager.SetHidePanel(this);
        }
        /// <summary>
        /// 显示面板
        /// </summary>
        public void Show()
        {
            for (int i = 0; i < hideObjs.Length; i++)
            {
                GameObject obj = hideObjs[i];
                if (obj != null)
                {
                    obj.SetActive(true);
                    hideObjs[i] = null;
                }
            }
        }
        /// <summary>
        /// 隐藏面板
        /// </summary>
        public void Hide()
        {
            if (hideObjs.Length != transform.childCount)
            {
                Debug.LogError("初始化面板子级数量和当前子级数量不同，如果有动态生成的GameObject请使用面板子级物体当作父级");
                return;
            }
            for (int i = 0; i < hideObjs.Length; i++)
            {
                GameObject childObj = transform.GetChild(i).gameObject;
                if (childObj.activeSelf)
                {
                    childObj.SetActive(false);
                    hideObjs[i] = childObj;
                }
                else
                {
                    hideObjs[i] = null;
                }
            }
        }
        /// <summary>
        /// 移除面板
        /// </summary>
        public virtual void Disable()
        {
            Destroy(this.gameObject);
        }
    }
}