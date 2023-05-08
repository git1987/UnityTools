using UnityEngine;
using UnityTools.Extend;

namespace UnityTools.MonoComponent
{
    /// <summary>
    /// 计时任务
    /// </summary>
    public class Schedule : MonoBehaviour
    {
        public static Schedule GetInstance(GameObject go)
        {
            Schedule schedule = go.AddComponent<Schedule>();
            return schedule;
        }
        bool enable;
        float timer;
        EventAction action;
        EventAction finish;
        /// <summary>
        /// 延迟调用一次
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time">等待时间</param>
        public void Once(EventAction action, float time)
        {
            if (time <= 0)
            {
                UnityTools.Debuger.Log("time<=0？");
                Stop(false);
                action?.Invoke();
            }
            else
            {
                enable = true;
                if (action == null)
                {
                    UnityTools.Debuger.LogError("callback is null！");
                    return;
                }
                this.action = action;
                timer = time;
            }
        }
        /// <summary>
        /// 暂停计时任务
        /// </summary>
        public void Pause() { enable = false; }
        /// <summary>
        /// 继续计时任务
        /// </summary>
        public void KeepOn() { enable = true; }
        /// <summary>
        /// 停止计时任务
        /// </summary>
        /// <param name="isComplete">是否执行完成计时任务回调</param>
        public void Stop(bool isComplete)
        {
            Destroy(this);
            if (isComplete) { finish?.Invoke(); }
        }
        void Update()
        {
            if (!enable) return;
            if (timer >= 0)
            {
                action?.Invoke();
            }
        }
        void OnDisable()
        {
            Stop(true);
        }
    }
}
