﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityTools.Config;
using UnityTools.Extend;

namespace UnityTools.UI
{
    /// <summary>
    /// 虚拟摇杆
    /// </summary>
    public class VirtualRocker : MonoBehaviour
    {
#if UNITY_EDITOR
        /// <summary>
        /// 选择CanvasGameObject创建一个虚拟摇杆
        /// </summary>
        /// <param name="canvasObj"></param>
        /// <returns></returns>
        public static GameObject CreateGameObject(GameObject canvasObj)
        {
            Canvas canvas = canvasObj.GetComponent<Canvas>();
            if (canvas == null)
            {
                Debuger.LogError("请选择带有Canvas组件的GameObject");
                return null;
            }
            GameObject rocker = new GameObject("VirtualRocker");
            VirtualRocker virtualRocker = rocker.AddComponent<VirtualRocker>();
            rocker.transform.SetParentReset(canvas.transform);
            RectTransform rect = rocker.AddComponent<RectTransform>();
            Tools.RectTransformSetSurround(rect);
            GameObject bg = new GameObject("pointBg");
            rect                   = bg.AddComponent<RectTransform>();
            virtualRocker._pointBg = rect;
            rect.SetParent(rocker.transform);
            rect.sizeDelta                         = Vector2.one * 200;
            rect.anchoredPosition                  = new Vector2(0, 250);
            rect.anchorMin                         = new Vector2(.5f, 0);
            rect.anchorMax                         = new Vector2(.5f, 0);
            bg.AddComponent<Image>().raycastTarget = false;
            GameObject pointer = new GameObject("pointer");
            rect                   = pointer.AddComponent<RectTransform>();
            virtualRocker._pointer = rect;
            rect.SetParent(bg.transform);
            rect.sizeDelta                              = new Vector2(10, 100);
            rect.anchoredPosition                       = Vector2.zero;
            rect.pivot                                  = new Vector2(.5f, 0);
            pointer.AddComponent<Image>().raycastTarget = false;
            GameObject point = new GameObject("point");
            rect                 = point.AddComponent<RectTransform>();
            virtualRocker._point = rect;
            rect.SetParent(bg.transform);
            rect.sizeDelta                            = Vector2.one * 50;
            rect.anchoredPosition                     = Vector2.zero;
            point.AddComponent<Image>().raycastTarget = false;
            GameObject area = new GameObject("area");
            rect                   = area.AddComponent<RectTransform>();
            virtualRocker.areaRect = rect;
            rect.SetParent(rocker.transform);
            rect.sizeDelta              = Vector2.one * 500;
            rect.anchoredPosition       = new Vector2(0, 250);
            rect.anchorMin              = new Vector2(.5f, 0);
            rect.anchorMax              = new Vector2(.5f, 0);
            rocker.transform.localScale = Vector3.one;
            return rocker;
        }
#endif
        public bool isClick { private set; get; }

        /// <summary>
        /// 未激活时是否隐藏
        /// </summary>
        public bool unenableHide;

        /// <summary>
        /// 是否忽略UI遮挡
        /// </summary>
        public bool ignoreUI;

        /// <summary>
        /// 摇杆区域背景
        /// </summary>
        [SerializeField] private RectTransform _pointBg;

        public RectTransform pointBg => _pointBg;

        /// <summary>
        /// 摇杆点
        /// </summary>
        [SerializeField] private RectTransform _point;

        public RectTransform point => _point;

        //是否显示虚拟摇杆点
        private bool showPoint;

        /// <summary>
        /// 摇杆方向指针
        /// </summary>
        [SerializeField] private RectTransform _pointer;

        public RectTransform pointer => _pointer;

        //是否显示虚拟摇杆指针
        private bool showPointer;

        /// <summary>
        /// 可触发摇杆的区域
        /// </summary>
        [SerializeField] private RectTransform areaRect;

        public Vector3 clickMousePos;
        public Vector2 rate = new Vector2(1080, 1920);

        public Vector2 Direction => _point.anchoredPosition / _pointBg.sizeDelta.x / 2;
        protected virtual void Awake()
        {
            CanvasScaler cs = this.GetComponentInParent<CanvasScaler>();
            if (cs == null)
                Debuger.LogError("CanvasScaler Component is null!");
            else
            {
                if (cs.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize &&
                    cs.screenMatchMode != CanvasScaler.ScreenMatchMode.Shrink)
                {
                    UnityTools.Debuger.LogError("需要将UIScaleMode设置为ScaleWithScreenSize然后将ScreenMatchMode设置为Shrink");
                }
                rate = cs.referenceResolution;
                ResetRocker();
            }
            if (_point != null)
            {
                showPoint = _point.GetComponent<Image>().sprite != null;
                _point.gameObject.SetActive(showPoint);
            }
            else { Debug.LogWarning("point is null!"); }
            if (_pointer != null)
            {
                showPointer = _point.GetComponent<Image>().sprite != null;
                _pointer.gameObject.SetActive(showPoint);
            }
            else { Debug.LogWarning("pointer is null!"); }
            if (_pointBg) { _pointBg.gameObject.SetActive(_pointBg.GetComponent<Image>().sprite == null); }
            else { Debug.LogWarning("pointBg is null!"); }
        }
        /// <summary>
        /// 是否触发虚拟摇杆UI
        /// </summary>
        protected virtual void CheckShowRocker()
        {
            if (ignoreUI || !EventSystem.current.IsPointerOverGameObject())
            {
                float screenRate = Screen.height * rate.x / (Screen.width * rate.y);
                int width, height;
                if (screenRate == 1)
                {
                    //9/16：分辨率不变
                    width  = (int)rate.x;
                    height = (int)rate.y;
                }
                else if (screenRate < 1)
                {
                    //高度变矮：高度发生变化（降低）
                    width  = (int)rate.x;
                    height = (int)((int)rate.y * screenRate);
                }
                else
                {
                    //高度拉高：宽度发生变化（增加）
                    width  = (int)((int)rate.x / screenRate);
                    height = (int)rate.y;
                }
                //(transform as RectTransform).sizeDelta = new Vector2(width, height);
                clickMousePos = Input.mousePosition;
                float x = clickMousePos.x * width / Screen.width - (width / 2);
                float y = clickMousePos.y * height / Screen.height;
                //
                if (Mathf.Abs(x) <= areaRect.sizeDelta.x / 2 &&
                    y >= areaRect.anchoredPosition.y - areaRect.sizeDelta.y / 2 &&
                    y <= areaRect.anchoredPosition.y + areaRect.sizeDelta.y / 2)
                {
                    isClick                   = true;
                    _pointBg.anchoredPosition = new Vector2(x, y);
                    _pointBg.gameObject.SetActive(true);
                }
            }
        }
        /// <summary>
        /// 重置虚拟摇杆UI
        /// </summary>
        protected virtual void ResetRocker()
        {
            isClick                   = false;
            _pointBg.anchoredPosition = areaRect.anchoredPosition;
            _point.anchoredPosition   = Vector3.zero;
            if (unenableHide) _pointBg.gameObject.SetActive(false);
        }
        /// <summary>
        /// 更新虚拟摇杆UI
        /// </summary>
        protected virtual void UpdateRocker()
        {
            if (isClick)
            {
                /*设置point位置点*/
                if (showPoint)
                {
                    _point.anchoredPosition = (Vector2)(Input.mousePosition - clickMousePos);
                    if (_point.anchoredPosition.magnitude > _pointBg.sizeDelta.x / 2 - _point.sizeDelta.x / 2)
                        _point.anchoredPosition = _point.anchoredPosition.normalized *
                            (_pointBg.sizeDelta.x / 2 - _point.sizeDelta.x / 2);
                }
                /*设置pointer的方向*/
                if (showPointer)
                {
                    float angle = Vector2.SignedAngle(Vector2.up, Direction);
                    _pointer.eulerAngles = new Vector3(0, 0, angle);
                    _pointer.gameObject.SetActive(Direction != Vector2.zero);
                }
            }
        }

        protected virtual void Update()
        {
            if (Configs.leftMouseDown) { CheckShowRocker(); }
            else if (Configs.leftMouseUp) { ResetRocker(); }
            UpdateRocker();
        }
    }
}