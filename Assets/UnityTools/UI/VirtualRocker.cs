using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
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
        [CustomEditor(typeof(VirtualRocker))]
        public class VirtualRockerEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                VirtualRocker vr = target as VirtualRocker;
                if (Application.isPlaying)
                {
                    EditorGUILayout.Vector2Field("当前方向", vr.Direction);
                    return;
                }
                base.OnInspectorGUI();
                RectTransform area = vr.transform.Find("area") as RectTransform;
                if (area == null) throw new NullReferenceException("area is null!");
                area.GetComponent<Image>().raycastTarget = !vr.ignoreUI;
                if (!vr.ignoreUI)
                {
                    vr.gp = area.gameObject.MateComponent<GraphicPointer>();
                }
                else
                {
                    if (vr.gp != null) DestroyImmediate(vr.gp);
                    vr.gp = null;
                }
                if (vr.ignoreUI)
                {
                    if (area.anchorMax.x != area.anchorMin.x)
                    {
                        area.anchorMin = new Vector2(area.anchorMax.x, area.anchorMin.y);
                    }
                    if (area.anchorMax.y != area.anchorMin.y)
                    {
                        area.anchorMin = new Vector2(area.anchorMin.x, area.anchorMax.y);
                    }
                }
            }
        }

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
            CanvasScaler cs = canvasObj.GetComponent<CanvasScaler>();
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
            rect.anchoredPosition                  = cs.referenceResolution / 2f;
            rect.anchorMin                         = Vector2.zero;
            rect.anchorMax                         = Vector2.zero;
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
            rect.SetParentReset(rocker.transform);
            rect.sizeDelta = Vector2.one * 500;
            Image areaImage = area.AddComponent<Image>();
            areaImage.color         = Vector4.one * .5f;
            areaImage.raycastTarget = false;
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
        [SerializeField]
        bool ignoreUI;

        [SerializeField]
        private GraphicPointer gp;

        /// <summary>
        /// 摇杆区域背景
        /// </summary>
        [SerializeField]
        private RectTransform _pointBg;

        public RectTransform pointBg => _pointBg;

        /// <summary>
        /// 摇杆点
        /// </summary>
        [SerializeField]
        private RectTransform _point;

        public RectTransform point => _point;

        //是否显示虚拟摇杆点
        private bool showPoint;

        /// <summary>
        /// 摇杆方向指针
        /// </summary>
        [SerializeField]
        private RectTransform _pointer;

        public RectTransform pointer => _pointer;

        //是否显示虚拟摇杆指针
        private bool showPointer;

        /// <summary>
        /// 可触发摇杆的区域
        /// </summary>
        [SerializeField]
        private RectTransform areaRect;

        public Vector2 clickMousePos;
        // public Vector2 rate = new Vector2(1080, 1920);

        public Vector2 Direction => _point.anchoredPosition / _pointBg.sizeDelta.x / 2;
        protected virtual void Awake()
        {
            CanvasScaler cs = this.GetComponentInParent<CanvasScaler>();
            if (gp != null)
            {
                gp.SetDownAction(CheckShowRocker);
                gp.SetUpAction(ResetRocker);
            }
            // if (cs == null)
            // Debuger.LogError("CanvasScaler Component is null!");
            // else
            // {
            // if (cs.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize &&
            //     cs.screenMatchMode != CanvasScaler.ScreenMatchMode.Shrink)
            // {
            //     UnityTools.Debuger.LogError("需要将UIScaleMode设置为ScaleWithScreenSize然后将ScreenMatchMode设置为Shrink");
            // }
            // rate = cs.referenceResolution;
            ResetRocker();
            // }
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
            // if (ignoreUI || !EventSystem.current.IsPointerOverGameObject())
            {
                isClick = true;
                _pointBg.gameObject.SetActive(true);
                clickMousePos             = Configs.screenPosition;
                _pointBg.anchoredPosition = clickMousePos;
                // float screenRate = Screen.height * rate.x / (Screen.width * rate.y);
                // int width, height;
                // if (screenRate == 1)
                // {
                //     //9/16：分辨率不变
                //     width  = (int)rate.x;
                //     height = (int)rate.y;
                // }
                // else if (screenRate < 1)
                // {
                //     //高度变矮：高度发生变化（降低）
                //     width  = (int)rate.x;
                //     height = (int)((int)rate.y * screenRate);
                // }
                // else
                // {
                //     //高度拉高：宽度发生变化（增加）
                //     width  = (int)((int)rate.x / screenRate);
                //     height = (int)rate.y;
                // }
                // //(transform as RectTransform).sizeDelta = new Vector2(width, height);
                // clickMousePos = Configs.screenPosition;
                // float x = clickMousePos.x * width / Screen.width - (width / 2);
                // float y = clickMousePos.y * height / Screen.height;
                // //
                // if (Mathf.Abs(x) <= areaRect.sizeDelta.x / 2 &&
                //     y >= areaRect.anchoredPosition.y - areaRect.sizeDelta.y / 2 &&
                //     y <= areaRect.anchoredPosition.y + areaRect.sizeDelta.y / 2)
                // {
                //     isClick                   = true;
                //     _pointBg.anchoredPosition = new Vector2(x, y);
                //     _pointBg.gameObject.SetActive(true);
                // }
            }
        }
        /// <summary>
        /// 重置虚拟摇杆UI
        /// </summary>
        protected virtual void ResetRocker()
        {
            isClick = false;
            if (unenableHide)
            {
                _pointBg.gameObject.SetActive(false);
            }
            else
            {
                _pointBg.SetParentReset(areaRect);
                _pointBg.SetParent(this.transform);
                _pointBg.anchoredPosition += areaRect.sizeDelta / 2;
                _point.anchoredPosition   =  Vector2.zero;
                _pointBg.gameObject.SetActive(true);
            }
        }
        /// <summary>
        /// 更新虚拟摇杆UI
        /// </summary>
        protected virtual void UpdateRocker()
        {
            if (!isClick) return;
            /*设置point位置点*/
            if (showPoint)
            {
                _point.anchoredPosition = Configs.screenPosition - clickMousePos;
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

        protected virtual void Update()
        {
            if (gp == null)
            {
                if (Configs.leftMouseDown) { CheckShowRocker(); }
                else if (Configs.leftMouseUp) { ResetRocker(); }
            }
            UpdateRocker();
        }
    }
}