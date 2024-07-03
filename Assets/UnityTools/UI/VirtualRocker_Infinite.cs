#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityTools.Extend;
namespace UnityTools.UI
{
    [AddComponentMenu("UnityTools/UI/无极虚拟摇杆")]
    public partial class VirtualRocker_Infinite : VirtualRocker
    {
#if UNITY_EDITOR
        [CustomEditor(typeof(VirtualRocker_Infinite))]
        public class VirtualRockerEditor : Editor
        {
            VirtualRocker_Infinite vr;
            private void OnEnable()
            {
                vr = target as VirtualRocker_Infinite;
            }
            private void OnSceneGUI() { }
            public override void OnInspectorGUI()
            {
                // base.OnInspectorGUI();
                VariableView();
                GUILayout.Space(5);
                if (Application.isPlaying)
                {
                    IsPlayingView();
                }
                else
                {
                    IsNotPlayingView();
                }
            }
            //变量值
            private void VariableView()
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    if (vr.canvasRect == null)
                    {
                        Canvas c = vr.GetComponentInParent<Canvas>();
                        if (c == null)
                        {
                            EditorGUILayout.HelpBox("进入编辑模式或者实例化在场景中，并设置父级为Canvas",
                                                    MessageType.Warning
                                                   );
                            return;
                        }
                        vr.canvasRect = c.transform as RectTransform;
                    }
                    if (vr._point == null || vr._pointer == null || vr._pointBg == null || vr.areaRect == null)
                    {
                        EditorGUILayout.HelpBox("子级丢失，请重新创建", MessageType.Error);
                        return;
                    }
                    if (vr.canvasRect == null)
                    {
                        EditorGUILayout.HelpBox("放在Canvas子级里", MessageType.Error);
                        return;
                    }
                    EditorGUILayout.ObjectField("Canvas", vr.canvasRect, typeof(RectTransform), true);
                    if (vr.gp != null)
                    {
                        EditorGUILayout.ObjectField("GraphicPointer", vr.gp, typeof(GraphicPointer), true);
                    }
                    if (vr.areaRect == null)
                        GUILayout.Label("虚拟摇杆触发区域丢失，请重新创建");
                    else
                        EditorGUILayout.ObjectField("虚拟摇杆触发区域", vr.areaRect, typeof(RectTransform), true);
                    EditorGUILayout.ObjectField("虚拟摇杆背景", vr._pointBg, typeof(RectTransform), true);
                    EditorGUILayout.ObjectField("虚拟摇杆点", vr._point, typeof(RectTransform), true);
                    EditorGUILayout.ObjectField("虚拟摇杆指针", vr._pointer, typeof(RectTransform), true);
                }
            }
            //运行状态下显示的内容
            private void IsPlayingView()
            {
                vr.unenableHide =
                    EditorGUILayout.Toggle("未激活时隐藏摇杆", vr.unenableHide);
                if (vr.unenableHide)
                {
                    vr.operationShow =
                        EditorGUILayout.Toggle("摇杆操作时才显示", vr.operationShow);
                }
                if (!vr.unenableHide)
                {
                    vr.backCenterPos = EditorGUILayout.Toggle("是否回到区域中心位置", vr.backCenterPos);
                    if (!vr.backCenterPos)
                    {
                        vr.restPos = EditorGUILayout.Toggle("未激活时是否回到原位", vr.restPos);
                        if (vr.restPos)
                        {
                            vr.oldPos = EditorGUILayout.Vector2Field("要回到的指定位置", vr.oldPos);
                        }
                    }
                }
                vr.showGUI = EditorGUILayout.Toggle("显示Log信息", vr.showGUI);
            }
            //非运行状态显示内容
            private void IsNotPlayingView()
            {
                vr.unenableHide = EditorGUILayout.Toggle("未激活时隐藏摇杆", vr.unenableHide);
                if (vr.unenableHide)
                {
                    vr.operationShow =
                        EditorGUILayout.Toggle("摇杆操作时才显示", vr.operationShow);
                }
                else
                {
                    vr.backCenterPos = EditorGUILayout.Toggle("是否回到区域中心位置", vr.backCenterPos);
                    if (!vr.backCenterPos)
                    {
                        vr.restPos = EditorGUILayout.Toggle("未激活时是否回到指定位置", vr.restPos);
                        if (vr.restPos)
                        {
                            vr.oldPos = EditorGUILayout.Vector2Field("要回到的指定位置", vr.oldPos);
                        }
                    }
                }
                if (vr) GUILayout.Space(5);
                vr.ignoreUI = EditorGUILayout.Toggle("是否忽略UI遮挡", vr.ignoreUI);
                vr._pointBg.gameObject.SetActive(!vr.unenableHide);
                RectTransform area = vr.transform.Find("area") as RectTransform;
                if (area == null) throw new System.NullReferenceException("area is null!");
                area.GetComponent<Graphic>().raycastTarget = !vr.ignoreUI;
                if (!vr.ignoreUI)
                {
                    vr.gp = area.gameObject.MateComponent<GraphicPointer>();
                }
                else
                {
                    if (vr.gp != null) DestroyImmediate(vr.gp);
                    vr.gp = null;
                }
                if (GUILayout.Button("保存"))
                {
                    EditorUtility.SetDirty(vr.gameObject);
                    AssetDatabase.SaveAssetIfDirty(vr.gameObject);
                }
            }
        }

        public void Set_pointBg(RectTransform rect)
        {
            _pointBg = rect;
        }
        public void Set_pointer(RectTransform rect)
        {
            _pointer = rect;
        }
        public void Set_point(RectTransform rect)
        {
            _point = rect;
        }
        public void SetareaRect(RectTransform rect)
        {
            areaRect = rect;
        }
        bool showGUI;
        private void OnGUI()
        {
            if (isClick && showGUI)
            {
                EditorGUILayout.Vector2Field("方向", Direction);
            }
        }
#endif
        /// <summary>
        /// 当作初始化用途的Vector2
        /// </summary>
        private Vector2 InitialVector2 => new Vector2(int.MaxValue, int.MaxValue);
        ///首次点击之后是否移动操作
        private bool isClickMove;
        public override Vector2 Direction => _point.anchoredPosition / ((_pointBg.sizeDelta - _point.sizeDelta) / 2);
        /// <summary>
        /// 未激活时是否隐藏
        /// </summary>
        [SerializeField]
        private bool unenableHide;
        /// <summary>
        /// 是否为操作时才显示
        /// </summary>
        [SerializeField]
        private bool operationShow;
        /// <summary>
        /// 是否忽略UI遮挡
        /// </summary>
        [SerializeField]
        private bool ignoreUI;
        /// <summary>
        /// 未激活时是否回到指定位置
        /// </summary>
        [SerializeField]
        private bool restPos;
        /// <summary>
        /// 是否回到区域中心位置
        /// </summary>
        [SerializeField]
        private bool backCenterPos;
        public Vector2 oldPos;
        [SerializeField]
        private GraphicPointer gp;
        [SerializeField]
        private RectTransform _pointBg;
        /// <summary>
        /// 摇杆区域背景
        /// </summary>
        public RectTransform pointBg => _pointBg;
        [SerializeField]
        private RectTransform _point;
        /// <summary>
        /// 摇杆点
        /// </summary>
        public RectTransform point => _point;
        ///是否显示虚拟摇杆点
        private bool showPoint;
        [SerializeField]
        private RectTransform _pointer;
        /// <summary>
        /// 摇杆方向指针
        /// </summary>
        public RectTransform pointer => _pointer;
        ///是否显示虚拟摇杆指针
        private bool showPointer;
        /// <summary>
        /// 可触发摇杆的区域
        /// </summary>
        [SerializeField]
        private RectTransform areaRect;
        ///点击时屏幕的位置点
        private Vector2 clickMousePos;
        ///当前鼠标、手势在屏幕中的位置
        private Vector2 currentMousePos;
        protected override void Awake()
        {
            base.Awake();
            if (gp != null)
            {
                gp.SetDownAction(() =>
                                 {
                                     if (canvasRect == null)
                                     {
                                         Debug.LogError("没有设置canvas[SetCanvas()]");
                                         return;
                                     }
                                     else
                                     {
                                         clickMousePos = Config.screenPosition;
                                         SetPoint();
                                     }
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
        /// 设置虚拟摇杆状态参数
        /// </summary>
        /// <param name="_unenableHide">未激活时是否隐藏</param>
        /// <param name="_restPos">是否重置位置</param>
        /// <param name="_backCenterPos">是否返回中心点</param>
        /// <param name="_backPos">返回的位置点坐标</param>
        public void SetState(bool _unenableHide, bool _restPos, bool _backCenterPos, Vector2 _backPos)
        {
            unenableHide  = _unenableHide;
            restPos       = _restPos;
            backCenterPos = _backCenterPos;
            oldPos        = _backPos;
            ResetRocker();
        }
        /// <summary>
        /// 是否触发虚拟摇杆UI
        /// </summary>
        protected virtual void CheckShowRocker()
        {
            if (canvasRect == null) throw new System.Exception("没有设置canvas[SetCanvas()]");
            //area的边界
            float minX = areaRect.anchorMin.x * canvasRect.sizeDelta.x + areaRect.offsetMin.x,
                  maxX = areaRect.anchorMax.x * canvasRect.sizeDelta.x + areaRect.offsetMax.x,
                  minY = areaRect.anchorMin.y * canvasRect.sizeDelta.y + areaRect.offsetMin.y,
                  maxY = areaRect.anchorMax.y * canvasRect.sizeDelta.y + areaRect.offsetMax.y;
            clickMousePos = Config.screenPosition;
            Vector2 canvasPos = clickMousePos * canvasRect.sizeDelta / new Vector2(Screen.width, Screen.height);
            if (canvasPos.x >= minX && canvasPos.x <= maxX && canvasPos.y >= minY && canvasPos.y <= maxY)
            {
                SetPoint();
            }
        }
        /// <summary>
        /// 设置当前点击的位置点，设置UI显示状态
        /// </summary>
        protected void SetPoint()
        {
            isClick = true;
            if (!operationShow)
            {
                pointBg.gameObject.SetActive(showPoint || showPointer);
            }
            pointBg.anchoredPosition = clickMousePos * canvasRect.sizeDelta / new Vector2(Screen.width, Screen.height);
        }
        /// <summary>
        /// 重置虚拟摇杆UI
        /// </summary>
        protected override void ResetRocker()
        {
            if (canvasRect == null)
            {
                //Debug.LogError("没有设置canvas[SetCanvas()]");
                return;
            }
            isClick                = false;
            isClickMove            = false;
            currentMousePos        = InitialVector2;
            point.anchoredPosition = Vector2.zero;
            if (unenableHide)
            {
                pointBg.gameObject.SetActive(false);
            }
            else
            {
                if (backCenterPos)
                {
                    //area的边界
                    float minX = areaRect.anchorMin.x * canvasRect.sizeDelta.x + areaRect.offsetMin.x,
                          maxX = areaRect.anchorMax.x * canvasRect.sizeDelta.x + areaRect.offsetMax.x,
                          minY = areaRect.anchorMin.y * canvasRect.sizeDelta.y + areaRect.offsetMin.y,
                          maxY = areaRect.anchorMax.y * canvasRect.sizeDelta.y + areaRect.offsetMax.y;
                    pointBg.anchoredPosition = new Vector2(minX + (maxX - minX) / 2, minY + (maxY - minY) / 2);
                }
                else if (restPos)
                {
                    pointBg.anchoredPosition = oldPos;
                }
                pointBg.gameObject.SetActive(showPoint || showPointer);
                pointer.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// 更新虚拟摇杆UI
        /// </summary>
        protected virtual void UpdateRocker()
        {
            if (!isClick) return;
            RockerBroadcast();
            if (currentMousePos == Config.screenPosition)
            {
                return;
            }
            else
            {
                if (!isClickMove && currentMousePos != InitialVector2)
                {
                    isClickMove = true;
                    //首次点击移动
                    if (operationShow)
                    {
                        pointBg.gameObject.SetActive(showPoint || showPointer);
                    }
                }
            }
            currentMousePos = Config.screenPosition;
            /*更新point位置点*/
            point.anchoredPosition = currentMousePos - clickMousePos;
            if (point.anchoredPosition.magnitude > pointBg.sizeDelta.x / 2 - point.sizeDelta.x / 2)
                point.anchoredPosition = point.anchoredPosition.normalized *
                                         (pointBg.sizeDelta.x / 2 - point.sizeDelta.x / 2);
            /*设置pointer的方向*/
            if (showPointer)
            {
                pointer.gameObject.SetActive(true);
                float angle = Vector2.SignedAngle(Vector2.up, Direction);
                pointer.eulerAngles = new Vector3(0, 0, angle);
                pointer.gameObject.SetActive(Direction != Vector2.zero);
            }
        }
        protected override void Update()
        {
            if (gp == null)
            {
                if (Config.leftMouseDown)
                {
                    CheckShowRocker();
                }
                else if (Config.leftMouseUp)
                {
                    ResetRocker();
                }
            }
            UpdateRocker();
        }
    }
}