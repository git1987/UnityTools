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
        [UnityEditor.CustomEditor(typeof(VirtualRocker))]
        public class VirtualRockerEditor : UnityEditor.Editor
        {
            private Vector2 direction, rate;
            private void OnSceneGUI()
            {
                VirtualRocker vr = target as VirtualRocker;
                direction = vr.Direction;
            }
            public override void OnInspectorGUI()
            {
                if (Application.isPlaying)
                {
                    direction = UnityEditor.EditorGUILayout.Vector2Field("当前方向", direction);
                    VirtualRocker virtualRocker = target as VirtualRocker;
                    virtualRocker.unenableHide =
                        UnityEditor.EditorGUILayout.Toggle("未激活时隐藏摇杆", virtualRocker.unenableHide);
                    return;
                }
                // base.OnInspectorGUI();
                VirtualRocker vr = target as VirtualRocker;
                using (new UnityEditor.EditorGUI.DisabledScope(true))
                {
                    if (vr.canvasRect == null)
                    {
                        Canvas c = vr.GetComponentInParent<Canvas>();
                        vr.canvasRect = c.transform as RectTransform;
                    }
                    if (vr._point == null || vr._pointer == null || vr._pointBg == null || vr.areaRect == null)
                    {
                        UnityEditor.EditorGUILayout.HelpBox("子级丢失，请重新创建", UnityEditor.MessageType.Error);
                        return;
                    }
                    UnityEditor.EditorGUILayout.ObjectField("Canvas", vr.canvasRect, typeof(RectTransform), true);
                    if (vr.gp != null)
                    {
                        UnityEditor.EditorGUILayout.ObjectField("GraphicPointer", vr.gp, typeof(GraphicPointer), true);
                    }
                    if (vr.areaRect == null)
                        GUILayout.Label("虚拟摇杆触发区域丢失，请重新创建");
                    else
                        UnityEditor.EditorGUILayout.ObjectField("虚拟摇杆触发区域", vr.areaRect, typeof(RectTransform), true);
                    UnityEditor.EditorGUILayout.ObjectField("虚拟摇杆背景", vr._pointBg, typeof(RectTransform), true);
                    UnityEditor.EditorGUILayout.ObjectField("虚拟摇杆点", vr._point, typeof(RectTransform), true);
                    UnityEditor.EditorGUILayout.ObjectField("虚拟摇杆指针", vr._pointer, typeof(RectTransform), true);
                }
                vr.unenableHide = UnityEditor.EditorGUILayout.Toggle("未激活时隐藏摇杆", vr.unenableHide);
                GUILayout.Space(5);
                vr.ignoreUI = UnityEditor.EditorGUILayout.Toggle("是否忽略UI遮挡", vr.ignoreUI);
                vr._pointBg.gameObject.SetActive(!vr.unenableHide);
                RectTransform area = vr.transform.Find("area") as RectTransform;
                if (area == null) throw new System.NullReferenceException("area is null!");
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
                    if (area.anchorMin.x != .5f && area.anchorMin.x != 0 && area.anchorMin.x != 1)
                    {
                        area.anchorMin        = new Vector2(.5f, area.anchorMin.y);
                        area.anchoredPosition = Vector2.zero;
                    }
                    if (area.anchorMin.y != .5f && area.anchorMin.y != 0 && area.anchorMin.y != 1)
                    {
                        area.anchorMin        = new Vector2(area.anchorMin.x, .5f);
                        area.anchoredPosition = Vector2.zero;
                    }
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
            RectTransform canvasRect = canvas.transform as RectTransform;
            GameObject rocker = new GameObject("VirtualRocker");
            VirtualRocker virtualRocker = rocker.AddComponent<VirtualRocker>();
            virtualRocker.canvasRect = canvasRect;
            rocker.transform.SetParentReset(canvasRect);
            RectTransform rect = rocker.AddComponent<RectTransform>();
            Tools.RectTransformSetSurround(rect);
            GameObject bg = new GameObject("pointBg");
            rect                   = bg.AddComponent<RectTransform>();
            virtualRocker._pointBg = rect;
            rect.SetParentReset(rocker.transform);
            rect.sizeDelta = Vector2.one * 200;
            rect.anchoredPosition = new Vector2(canvasRect.sizeDelta.x / 2f, canvasRect.sizeDelta.y / 2f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            bg.AddComponent<Image>().raycastTarget = false;
            GameObject pointer = new GameObject("pointer");
            rect                   = pointer.AddComponent<RectTransform>();
            virtualRocker._pointer = rect;
            rect.SetParentReset(bg.transform);
            rect.sizeDelta                              = new Vector2(10, 100);
            rect.anchoredPosition                       = Vector2.zero;
            rect.pivot                                  = new Vector2(.5f, 0);
            pointer.AddComponent<Image>().raycastTarget = false;
            GameObject point = new GameObject("point");
            rect                 = point.AddComponent<RectTransform>();
            virtualRocker._point = rect;
            rect.SetParentReset(bg.transform);
            rect.sizeDelta                            = Vector2.one * 50;
            rect.anchoredPosition                     = Vector2.zero;
            point.AddComponent<Image>().raycastTarget = false;
            GameObject area = new GameObject("area");
            rect = area.AddComponent<RectTransform>();
            rect.SetAsFirstSibling();
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

        [SerializeField]
        private RectTransform canvasRect;

        /// <summary>
        /// 未激活时是否隐藏
        /// </summary>
        public bool unenableHide;

        /// <summary>
        /// 是否忽略UI遮挡
        /// </summary>
        [SerializeField]
        private bool ignoreUI;

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

        private Vector2 clickMousePos;
        public Vector2 Direction => _point.anchoredPosition / _pointBg.sizeDelta.x / 2;
        protected virtual void Awake()
        {
            CanvasScaler cs = this.GetComponentInParent<CanvasScaler>();
            if (gp != null)
            {
                gp.SetDownAction(() =>
                    {
                        clickMousePos = Configs.screenPosition;
                        SetPoint();
                    }
                );
                gp.SetUpAction(ResetRocker);
            }
            showPoint = _point.GetComponent<Image>().sprite != null;
            _point.gameObject.SetActive(showPoint);
            showPointer = _pointer.GetComponent<Image>().sprite != null;
            _pointer.gameObject.SetActive(showPointer);
            ResetRocker();
        }
        /// <summary>
        /// 是否触发虚拟摇杆UI
        /// </summary>
        protected virtual void CheckShowRocker()
        {
            //area的边界
            float minX = areaRect.anchoredPosition.x + areaRect.anchorMin.x * canvasRect.sizeDelta.x -
                      areaRect.sizeDelta.x * areaRect.pivot.x,
                  maxX = areaRect.anchoredPosition.x + areaRect.anchorMin.x * canvasRect.sizeDelta.x +
                      areaRect.sizeDelta.x * areaRect.pivot.x,
                  minY = areaRect.anchoredPosition.y + areaRect.anchorMin.y * canvasRect.sizeDelta.y -
                      areaRect.sizeDelta.y * areaRect.pivot.y,
                  maxY = areaRect.anchoredPosition.y + areaRect.anchorMin.y * canvasRect.sizeDelta.y +
                      areaRect.sizeDelta.y * areaRect.pivot.y;
            clickMousePos = Configs.screenPosition;
            Vector2 canvasPos = clickMousePos * canvasRect.sizeDelta / new Vector2(Screen.width, Screen.height);
            if (canvasPos.x >= minX && canvasPos.x <= maxX && canvasPos.y >= minY && canvasPos.y <= maxY)
            {
                SetPoint();
            }
        }
        void SetPoint()
        {
            isClick = true;
            _pointBg.gameObject.SetActive(true);
            _pointBg.anchoredPosition = clickMousePos * canvasRect.sizeDelta / new Vector2(Screen.width, Screen.height);
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
                if (Configs.leftMouseDown)
                {
                    CheckShowRocker();
                }
                else if (Configs.leftMouseUp)
                {
                    ResetRocker();
                }
            }
            UpdateRocker();
        }
    }
}