using UnityEngine;
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
            public float interval;
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
        private bool over;

        //开关
        private bool enable;

        //计时器
        private float timer;

        //当前循环调用的方法次数
        private int repeatIndex;
        private bool dontDestroy;
        ScheduleData scheduleData;
        /// <summary>
        /// 设置计时任务完成后不Destroy
        /// </summary>
        public void SetDontDestroy() { dontDestroy = true; }
        /// <summary>
        /// 延迟调用一次
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time">等待时间</param>
        public void Once(EventAction action, float time)
        {
            if (action == null)
            {
                Debuger.LogError("callback is null！");
                return;
            }
            scheduleData = new ScheduleData()
            {
                maxTime = time + Time.deltaTime, action = action,
            };
            if (time <= 0)
            {
                Debuger.Log("time<=0？");
                Stop(true);
                action.Invoke();
            }
            else
            {
                enable = true;
                /*当前帧要在Update里执行timer -= Time.DeltaTime，所以在当前帧要把deltaTime补回来*/
                timer = time;
            }
        }
        /// <summary>
        /// 重复调用方法
        /// </summary>
        /// <param name="repeatedAction"></param>
        /// <param name="startTime">第一次调用方法的时间</param>
        /// <param name="intervalTime">两次调用方法的间隔时间</param>
        /// <param name="repeat"></param>
        /// <param name="maxTime">计时任务的最大时间</param>
        /// <param name="finish"></param>
        public void Repeated(EventAction<int> repeatedAction, float startTime, float intervalTime, int repeat,
                             float maxTime, EventAction finish = null)
        {
            enable = true;
            scheduleData = new ScheduleData()
            {
                maxTime        = maxTime + Time.deltaTime,
                repeatedAction = repeatedAction,
                interval       = intervalTime,
                repeat         = repeat,
                finish         = finish
            };
            /*当前帧要在Update里执行timer -= Time.DeltaTime，所以在当前帧要把deltaTime补回来*/
            timer       = startTime;
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
            if (isComplete)
            {
                scheduleData.finish?.Invoke();
            }
            if (!dontDestroy)
                Destroy(this);
            else
            {
                enable = false;
            }
        }
        private void Update()
        {
            if (!enable) return;
            if (scheduleData.maxTime < float.MaxValue) scheduleData.maxTime -= Time.deltaTime;
            switch (timer)
            {
                case <= 0:
                    scheduleData.action?.Invoke();
                    scheduleData.repeatedAction?.Invoke(repeatIndex++);
                    if (scheduleData.repeat < int.MaxValue) scheduleData.repeat--;
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
                        timer += scheduleData.interval;
                    }
                    break;
                case < float.MaxValue:
                    timer -= Time.deltaTime;
                    break;
            }
        }
        private void OnDisable()
        {
            //如果计时器没有结束，被隐藏了，自动调用Stop，并且不触发回调
            if (!over) Stop(false);
        }
    }
}