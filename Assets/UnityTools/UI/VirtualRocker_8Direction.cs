#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityTools.Extend;
namespace UnityTools.UI
{
    [DisallowMultipleComponent]
    [AddComponentMenu("UnityTools/UI/8方向虚拟摇杆")]
    public partial class VirtualRocker_8Direction : VirtualRocker
    {
        public enum DirectionCount
        {
            _4,
            _8
        }
#if UNITY_EDITOR
        [CustomEditor(typeof(VirtualRocker_8Direction))]
        public class VirtualRockerEditor : Editor
        {
            VirtualRocker_8Direction vr;
            private void OnEnable()
            {
                vr = target as VirtualRocker_8Direction;
            }
            private void OnSceneGUI() { }
            public override void OnInspectorGUI()
            {
                // base.OnInspectorGUI();
                VariableView();
                GUILayout.Space(5);
            }
            //变量值
            private void VariableView()
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    if (vr.directionCount == DirectionCount._4)
                    {
                        if (vr.center == null
                         || vr.up     == null || vr.down == null || vr.left == null || vr.right == null)
                        {
                            EditorGUILayout.HelpBox("子级丢失，请重新创建", MessageType.Error);
                        }
                        else
                        {
                            EditorGUILayout.ObjectField("中心", vr.center, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("上", vr.up, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("下", vr.down, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("左", vr.left, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("右", vr.right, typeof(GameObject), true);
                        }
                    }
                    else if (vr.directionCount == DirectionCount._8)
                    {
                        if (vr.center == null
                         || vr.up     == null || vr.down    == null || vr.left     == null || vr.right     == null
                         || vr.upLeft == null || vr.upRight == null || vr.downLeft == null || vr.downRight == null)
                        {
                            if (vr.center != null
                             && vr.up     != null && vr.down != null && vr.left != null && vr.right != null)
                            {
                                EditorGUILayout.HelpBox("应该是一个四方向的虚拟摇杆，如果想要使用8方向虚拟摇杆，请重新创建", MessageType.Warning);
                            }
                            else
                            {
                                EditorGUILayout.HelpBox("子级丢失，请重新创建", MessageType.Error);
                            }
                        }
                        else
                        {
                            EditorGUILayout.ObjectField("中心", vr.center, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("上", vr.up, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("下", vr.down, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("左", vr.left, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("右", vr.right, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("上左", vr.upLeft, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("上右", vr.upRight, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("下左", vr.downLeft, typeof(GameObject), true);
                            EditorGUILayout.ObjectField("下右", vr.downRight, typeof(GameObject), true);
                        }
                    }
                }
                vr.directionCount = (DirectionCount)EditorGUILayout.EnumPopup("方向数量", vr.directionCount);
            }
        }

        /// <summary>
        /// center
        /// 设置方向MaskGraphic 4方向：上、下、左、右
        /// 设置方向MaskGraphic 8方向：上、下、左、右、上左、上右、下左、下右
        /// </summary>
        /// <param name="directionRect">0.center</param>
        public void SetDirectionObj(params GameObject[] directionRect)
        {
            if (directionRect.Length == 5)
            {
                directionCount = DirectionCount._4;
                center         = directionRect[0];
                up             = directionRect[1];
                down           = directionRect[2];
                left           = directionRect[3];
                right          = directionRect[4];
            }
            else if (directionRect.Length == 9)
            {
                directionCount = DirectionCount._8;
                center         = directionRect[0];
                up             = directionRect[1];
                down           = directionRect[2];
                left           = directionRect[3];
                right          = directionRect[4];
                upLeft         = directionRect[5];
                upRight        = directionRect[6];
                downLeft       = directionRect[7];
                downRight      = directionRect[8];
            }
            else
            {
                Debug.LogError($"方向数量错误：{directionRect.Length}");
            }
        }
#endif
        [SerializeField]
        private DirectionCount directionCount;
        [SerializeField]
        //分别对应各个方向：center是中线点，可用于激活摇杆，中心点向量为Vector2.zero
        private GameObject center = null,
                           up = null,
                           down = null,
                           left = null,
                           right = null,
                           upLeft = null,
                           upRight = null,
                           downLeft = null,
                           downRight = null;
        public override Vector2 Direction => buttonDirection;
        //按钮对应的方向
        private Vector2 buttonDirection;
        protected override void Awake()
        {
            if (center    != null) center.MateComponent<VirtualRocker_8DirectionButton>().Init(this, Vector2.zero);
            if (up        != null) up.MateComponent<VirtualRocker_8DirectionButton>().Init(this, Vector2.up);
            if (down      != null) down.MateComponent<VirtualRocker_8DirectionButton>().Init(this, Vector2.down);
            if (left      != null) left.MateComponent<VirtualRocker_8DirectionButton>().Init(this, Vector2.left);
            if (right     != null) right.MateComponent<VirtualRocker_8DirectionButton>().Init(this, Vector2.right);
            if (upLeft    != null) upLeft.MateComponent<VirtualRocker_8DirectionButton>().Init(this, new(-1, 1));
            if (upRight   != null) upRight.MateComponent<VirtualRocker_8DirectionButton>().Init(this, new(1, 1));
            if (downLeft  != null) downLeft.MateComponent<VirtualRocker_8DirectionButton>().Init(this, new(-1, -1));
            if (downRight != null) downRight.MateComponent<VirtualRocker_8DirectionButton>().Init(this, new(1, -1));
            base.Awake();
        }
        //在虚拟按键中按下
        public void OnClickDown(Vector2 direction)
        {
            isClick         = true;
            buttonDirection = direction;
        }
        //手势抬起
        public void OnClickUp()
        {
            isClick         = false;
            buttonDirection = Vector2.zero;
        }
        /// <summary>
        /// 点击摇杆按键之后从按键上离开
        /// </summary>
        public void IntoButton(Vector2 direction)
        {
            if (isClick) buttonDirection = direction;
        }
        /// <summary>
        /// 点击摇杆按键之后从按键上离开
        /// </summary>
        public void LeaveButton()
        {
            if (isClick) buttonDirection = Vector2.zero;
        }
        protected override void ResetRocker() { }
        protected override void Update()
        {
            base.Update();
        }
    }

    public class VirtualRocker_8DirectionButton : MonoBehaviour,
                                                  IPointerEnterHandler,
                                                  IPointerExitHandler,
                                                  IPointerDownHandler,
                                                  IPointerUpHandler
    {
        private VirtualRocker_8Direction virtualRocker;
        private Vector2 direction;
        public void Init(VirtualRocker_8Direction virtualRocker, Vector2 direction)
        {
            this.virtualRocker = virtualRocker;
            this.direction     = direction;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            virtualRocker.IntoButton(direction);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            virtualRocker.LeaveButton();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            virtualRocker.OnClickDown(direction);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            virtualRocker.OnClickUp();
        }
    }
}