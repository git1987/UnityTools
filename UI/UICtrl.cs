using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityTools.Extend;
namespace UnityTools.UI
{
    /// <summary>
    /// 场景UI控制器
    /// </summary>
    public abstract class UICtrl : MonoBehaviour
    {
        //进入场景时候相机的动画
        protected Animator cameraAnimator;
        //private Schedule cameraSchedule;
        private RectTransform maskRect;
        public Canvas canvas { private set; get; }
        public RectTransform rect { private set; get; }
        protected virtual void Awake()
        {
            canvas = GetComponent<Canvas>();
            rect = transform as RectTransform;
            UIManager.SetUICtrl(this);
            GameObject mask = new GameObject("mask");
            maskRect = mask.transform.gameObject.AddComponent<RectTransform>();
            //maskRect.gameObject.AddComponent<CanvasRenderer>();
            Image image = maskRect.GetComponent<Image>();
            //image.color = new Color(0, 0, 0, 0);
            maskRect.SetParentReset(rect);
            maskRect.anchorMin = Vector2.zero;
            maskRect.anchorMax = Vector2.one;
            maskRect.offsetMin = Vector2.zero;
            maskRect.offsetMax = Vector2.zero;
            maskRect.localScale = Vector3.one;
            mask.SetActive(false);
        }
        /// <summary>
        /// 先实现子类的Start方法，最后调用父类base.Start()
        /// </summary>
        protected virtual void Start()
        {
            //if (SceneCtrl.goToSceneName == null || SceneCtrl.goToSceneName == string.Empty)
                GameBegin();
        }
        /// <summary>
        /// 动态加载场景，手动开始游戏
        /// </summary> 
        public virtual void GameBegin()
        {
            //如果有场景动画，在播放完场景动画之后初始化UICtrl
            if (cameraAnimator != null)
            {
                StartCoroutine(StopCameraAnimation(cameraAnimator.GetCurrentAnimatorStateInfo(0).length));
                //cameraSchedule = Schedule.GetInstance(this.gameObject);
                //cameraSchedule.Once(() =>
                //{
                //    cameraSchedule = null;
                //    cameraAnimator.enabled = false;
                //    Init();
                //}, cameraAnimator.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                Init();
            }
        }
        /// <summary>
        /// 初始化UICtrl
        /// </summary>
        protected abstract void Init();
        protected IEnumerator StopCameraAnimation(float time)
        {
            float timer = time;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    timer = 0;
                    Init();
                    cameraAnimator.Play("opening_animation", -1, cameraAnimator.GetCurrentAnimatorStateInfo(0).length);
                    //cameraSchedule = cameraSchedule.Stop();
                }
                yield return null;
            }
            cameraAnimator.enabled = false;
        }
        /// <summary>
        /// UI控制内打开面板
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="panelLv"></param>
        protected virtual P OpenPanel<P>(int panelLv = 1) where P : BasePanel
        {
            return UIManager.OpenPanel<P>(panelLv);
        }
        /// <summary>
        /// 设置遮罩状态:过度一些动画，防止在动画中点击触发了事件
        /// </summary>
        /// <param name="active"></param>
        public virtual void SetMask(bool active)
        {
            if (active)
            {
                maskRect.gameObject.SetActive(true);
                maskRect.SetAsLastSibling();
            }
            else
            {
                maskRect.gameObject.SetActive(false);
                maskRect.SetAsFirstSibling();
            }
        }
    }
}
