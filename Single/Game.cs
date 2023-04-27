using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnityTools.Single
{
    public sealed class Game : SingleMono<Game>
    {
        /// <summary>
        /// 清除所有延时调用方法
        /// </summary>
        public static void ClearDelayed()
        {
            instance.actionList.Clear();
            instance.actionTimeList.Clear();
        }
        /// <summary>
        /// 延时调用方法:time==0 时，隔一帧执行监听
        /// </summary>
        /// <param name="action"></param>
        /// <param name="time">== 0 时，隔一帧执行监听</param>
        public static void Delayed(EventAction action, float time = 0)
        {
            if (instance.actionList.Contains(action))
            {
                Debuger.LogError("已经存在回调了，添加了两次");
                return;
            }
            if (time <= 0) instance.DelayedFrame(action);
            else
            {
                instance.actionList.Add(action);
                //新添加的回调，在当前帧就要执行Update，所以要提前加当前deltaTime补上
                instance.actionTimeList.Add(time + Time.deltaTime);
            }
        }
        /// <summary>
        /// 移除延时调用的方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool RemoveDelayed(EventAction action)
        {
            if (instance.actionList.Contains(action))
            {
                int index = instance.actionList.IndexOf(action);
                instance.actionList.RemoveAt(index);
                instance.actionTimeList.RemoveAt(index);
                return true;
            }
            Debuger.LogError("没有该延时调用的方法");
            return false;
        }
        private List<EventAction> actionList = new List<EventAction>();
        private List<float> actionTimeList = new List<float>();
        /// <summary>
        /// 延迟一帧执行监听
        /// </summary>
        /// <param name="action"></param>
        public void DelayedFrame(EventAction action)
        {
            StartCoroutine(_DelayedFrame(action));
        }
        IEnumerator _DelayedFrame(EventAction action)
        {
            yield return null;
            action();
        }

        private void Update()
        {
            if (actionList != null && actionList.Count > 0)
            {
                for (int i = 0; i < actionList.Count; i++)
                {
                    actionTimeList[i] -= Time.deltaTime;
                    if (actionTimeList[i] <= 0)
                    {
                        actionList[i].Invoke();
                        instance.actionList.RemoveAt(i);
                        instance.actionTimeList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}
