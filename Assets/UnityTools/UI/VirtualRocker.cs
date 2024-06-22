using UnityEngine;
using UnityEngine.UI;
using UnityTools.Extend;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UnityTools.UI
{
    /// <summary>
    /// 虚拟摇杆
    /// </summary>
    public abstract partial class VirtualRocker : MonoBehaviour
    {
        /// <summary>
        /// 虚拟摇杆当前的Vector2向量
        /// </summary>
        public abstract Vector2 Direction { get; }
        public bool isClick { protected set; get; }
        public bool isDirection => isClick && (Direction.x != 0 || Direction.y != 0);
        [SerializeField]
        protected RectTransform canvasRect;
        protected event EventAction<Vector2> rockerAction;
        protected virtual void Awake()
        {
            ResetRocker();
        }
        /// <summary>
        /// 启用
        /// </summary>
        public void Enable()
        {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 禁用
        /// </summary>
        public void Unenable()
        {
            ResetRocker();
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 重置虚拟摇杆
        /// </summary>
        protected abstract void ResetRocker();
        /// <summary>
        /// 添加摇杆事件监听
        /// </summary>
        /// <param name="action"></param>
        public void AddListener(EventAction<Vector2> action)
        {
            rockerAction += action;
        }
        /// <summary>
        /// 移除摇杆事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveListener(EventAction<Vector2> action)
        {
            rockerAction -= action;
        }
        /// <summary>
        /// 摇杆事件广播
        /// </summary>
        protected void RockerBroadcast()
        {
            if (rockerAction != null)
            {
                if (Direction.x != 0 || Direction.y != 0)
                {
                    rockerAction.Invoke(Direction);
                }
            }
        }
        /// <summary>
        /// 重置当前虚拟摇杆
        /// </summary>
        public void Stop()
        {
            ResetRocker();
        }
        /// <summary>
        /// 设置当前UI的Canvas
        /// </summary>
        /// <param name="canvas"></param>
        public void SetCanvas(RectTransform canvas)
        {
            canvasRect = canvas;
        }
        private void OnDisable()
        {
            ResetRocker();
        }
        protected virtual void Update() { }
    }
}