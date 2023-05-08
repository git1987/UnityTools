using System;
using UnityEngine;
using UnityTools.Extend;

namespace UnityTools.MonoComponent
{
    /// <summary>
    /// 计时任务
    /// </summary>
    public class Schedule : MonoBehaviour
    {
        public struct ScheduleData
        {
            /// <summary>
            /// 普通回调
            /// </summary>
            public EventAction action;
            /// <summary>
            /// 重复回调：当前的次数（index）
            /// </summary>
            public EventAction<int> repeatedAction;
            /// <summary>
            /// 任务结束后的回调
            /// </summary>
            public EventAction finish;
            /// <summary>
            /// 持续的最长时间
            /// </summary>
            public float maxTime;
            /// <summary>
            /// 周期
            /// </summary>
            public float periodTime;
            /// <summary>
            /// 重复次数
            /// </summary>
            public int repeat;
        }
        public static Schedule GetInstance(GameObject go)
        {
            Schedule schedule = go.AddComponent<Schedule>();
            return schedule;
        }
        //是否已经结束
        bool over;
        //开关
        bool enable;
        //计时器
        float timer;
        //当前循环调用的方法次数
        int repeatIndex;
        ScheduleData scheduleData;
        /// <summary>
        /// 延迟调用一次
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time">等待时间</param>
        public void Once(EventAction action, float time)
        {
            scheduleData = new ScheduleData()
            {
                maxTime = time,
                action = action,
            };
            if (time <= 0)
            {
                UnityTools.Debuger.Log("time<=0？");
                Stop(true);
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
                timer = time;
            }
        }
        /// <summary>
        /// 重复调用方法
        /// </summary>
        /// <param name="repeatedAction"></param>
        /// <param name="startTime"></param>
        /// <param name="periodTime"></param>
        /// <param name="repeat"></param>
        /// <param name="maxTime"></param>
        /// <param name="finish"></param>
        public void Repeated(EventAction<int> repeatedAction, float startTime, float periodTime, int repeat, float maxTime, EventAction finish)
        {
            scheduleData = new ScheduleData()
            {
                maxTime = maxTime,
                repeatedAction = repeatedAction,
                periodTime = periodTime,
                repeat = repeat,
                finish = finish
            };
            timer = startTime;
            repeatIndex = 0;
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
            over = true;
            if (isComplete) { scheduleData.finish?.Invoke(); }
            Destroy(this);
        }
        private void Update()
        {
            if (!enable) return;
            if (timer <= 0)
            {
                scheduleData.action?.Invoke();
                scheduleData.repeatedAction?.Invoke(repeatIndex++);
                if (scheduleData.repeat < int.MaxValue)
                    scheduleData.repeat--;
                if (scheduleData.maxTime < float.MaxValue)
                    scheduleData.maxTime -= Time.deltaTime;
                if (scheduleData.repeat <= 0)
                {
                    //次数用完了
                    Stop(true);
                }
                else if (scheduleData.maxTime <= 0)
                {
                    //时间到了
                    Stop(true);
                }
                else
                {
                    timer = scheduleData.periodTime;
                }
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        private void OnDisable()
        {
            //如果计时器没有结束，被隐藏了，自动调用Stop，并且不触发回调
            if (!over)
                Stop(false);
        }
    }
}
