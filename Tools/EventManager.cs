using System;
using System.Collections.Generic;
using System.Linq;
using UnityTools.Extend;

namespace UnityTools
{
    /// <summary>
    /// 无参无返回值委托
    /// </summary>
    public delegate void EventAction();
    /// <summary>
    /// 1参无返回值委托
    /// </summary>
    public delegate void EventAction<A>(A a);
    /// <summary>
    /// 2参无返回值委托
    /// </summary>
    public delegate void EventAction<A, B>(A a, B b);
    /// <summary>
    /// 无参1返回值委托
    /// </summary>
    public delegate R EventFunction<R>();
    /// <summary>
    /// 1参1返回值委托
    /// </summary>
    public delegate R EventFunction<in A, R>(A a);
    /// <summary>
    /// 无参数事件监听
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        /// 带参数的EventManager事件监听清除
        /// </summary>
        static public List<EventAction> eventManagerClear { private set; get; }
        static private List<string> eventActionNameList;
        static private List<List<EventAction>> eventActionList;
        static EventManager()
        {
            eventManagerClear = new List<EventAction>();
            eventActionNameList = new List<string>();
            eventActionList = new List<List<EventAction>>();
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener(string key, EventAction action)
        {
            if (eventActionNameList.Contains(key))
            {
                bool isKey = false;
                eventActionNameList.ForAction((item, index) =>
                {
                    if (item == key)
                    {
                        if (eventActionList[index].Contains(action))
                            Debuger.LogError(key + "重复添加事件监听");
                        else
                            eventActionList[index].Add(action);
                        isKey = true;
                    }
                }, () => isKey);
            }
            else
            {
                eventActionNameList.Add(key);
                eventActionList.Add(new List<EventAction>() { action });
            }
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener<E>(E key, EventAction action) where E : System.Enum
        { AddListener(key.ToString(), action); }
        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool RemoveListener(string key)
        {
            bool isRemove = false;
            eventActionNameList.ForAction((item, index) =>
            {
                if (item == key)
                {
                    eventActionNameList.RemoveAt(index);
                    eventActionList.RemoveAt(index);
                    isRemove = true;
                }
            }, () => isRemove);
            return isRemove;
        }
        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool RemoveListener<E>(E key) where E : System.Enum
        {
            return RemoveListener(key.ToString());
        }
        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        [Obsolete("Use RemoveListener(key,action)!")]
        static public bool RemoveListener(EventAction action)
        {
            int removeCount = 0;
            for (int i = 0; i < eventActionList.Count; i++)
            {
                for (int j = 0; j < eventActionList[i].Count; j++)
                {
                    if (eventActionList[i][j] == action)
                    {
                        eventActionList[i].RemoveAt(j);
                        j--;
                        removeCount++;
                    }
                }
            }
            Debuger.LogWarning("移除监听个数：" + removeCount);
            return removeCount > 0;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public bool RemoveListener(string key, EventAction action)
        {
            for (int i = 0; i < eventActionList.Count; i++)
            {
                if (eventActionNameList[i] == key)
                {
                    return eventActionList[i].Remove(action);
                }
            }
            return false;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void RemoveListener<E>(E key, EventAction action) where E : System.Enum
        { RemoveListener(key.ToString(), action); }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        static public void Broadcast<E>(E key) where E : System.Enum
        { Broadcast(key.ToString()); }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        static public void Broadcast(string key)
        {
            bool isKey = false;
            eventActionNameList.ForAction((item, index) =>
            {
                if (item == key)
                {
                    eventActionList[index].ForAction(e => e?.Invoke());
                }
            }, () => isKey);
        }
        /// <summary>
        /// 清除所有委托
        /// </summary>
        static public void Clear()
        {
            eventActionList.Clear();
            eventActionNameList.Clear();
            for (int i = 0; i < eventManagerClear.Count; i++)
                eventManagerClear[i]?.Invoke();
        }
    }

    /// <summary>
    /// 1参数事件监听
    /// </summary>
    public static class EventManager<T>
    {
        //static readonly Dictionary<string, List<EventAction<T>>> eventActions;
        static private List<string> eventActionNameList;
        static private List<List<EventAction<T>>> eventActionList;
        static EventManager()
        {
            eventActionNameList = new List<string>();
            eventActionList = new List<List<EventAction<T>>>();
            //eventActions = new Dictionary<string, List<EventAction<T>>>();
            //通过调用EventManager的静态成员，提前调用EventManager静态的构造方法
            EventManager.eventManagerClear.Add(() => EventManager<T>.Clear());
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener(string key, EventAction<T> action)
        {
            if (eventActionNameList.Contains(key))
            {
                bool isKey = false;
                eventActionNameList.ForAction((item, index) =>
                {
                    if (item == key)
                    {
                        if (eventActionList[index].Contains(action))
                            Debuger.LogError(key + "重复添加事件监听");
                        else
                            eventActionList[index].Add(action);
                        isKey = true;
                    }
                }, () => isKey);
            }
            else
            {
                eventActionNameList.Add(key);
                eventActionList.Add(new List<EventAction<T>>() { action });
            }
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener<E>(E key, EventAction<T> action) where E : System.Enum
        { AddListener(key.ToString(), action); }
        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool RemoveListener(string key)
        {
            bool isRemove = false;
            eventActionNameList.ForAction((item, index) =>
            {
                if (item == key)
                {
                    eventActionNameList.RemoveAt(index);
                    eventActionList.RemoveAt(index);
                    isRemove = true;
                }
            }, () => isRemove);
            return isRemove;
        }
        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool RemoveListener<E>(E key) where E : System.Enum
        {
            return RemoveListener(key.ToString());
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        [Obsolete("Use RemoveListener(key,action)!")]
        static public bool RemoveListener(EventAction<T> action)
        {
            int removeCount = 0;
            for (int i = 0; i < eventActionList.Count; i++)
            {
                for (int j = 0; j < eventActionList[i].Count; j++)
                {
                    if (eventActionList[i][j] == action)
                    {
                        eventActionList[i].RemoveAt(j);
                        j--;
                        removeCount++;
                    }
                }
            }
            Debuger.LogWarning("移除监听个数：" + removeCount);
            return removeCount > 0;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public bool RemoveListener(string key, EventAction<T> action)
        {
            for (int i = 0; i < eventActionList.Count; i++)
            {
                if (eventActionNameList[i] == key)
                {
                    return eventActionList[i].Remove(action);
                }
            }
            return false;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void RemoveListener<E>(E key, EventAction<T> action) where E : System.Enum
        { RemoveListener(key.ToString(), action); }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        static public void Broadcast(string key, T t)
        {
            bool isKey = false;
            eventActionNameList.ForAction((item, index) =>
            {
                if (item == key)
                {
                    eventActionList[index].ForAction(e => e?.Invoke(t));
                }
            }, () => isKey);
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        static public void Broadcast<E>(E key, T t) where E : System.Enum
        { Broadcast(key.ToString(), t); }
        /// <summary>
        /// 清除所有委托
        /// </summary>
        static public void Clear()
        {
            Debuger.LogWarning($"clear <{typeof(T).Name}> all events!");
            eventActionList.Clear();
            eventActionNameList.Clear();
        }
    }

    /// <summary>
    /// 2参数事件监听
    /// </summary>
    public static class EventManager<T1, T2>
    {
        //static readonly Dictionary<string, List<EventAction<T1, T2>>> eventActions;
        static private List<string> eventActionNameList;
        static private List<List<EventAction<T1, T2>>> eventActionList;
        static EventManager()
        {
            eventActionNameList = new List<string>();
            eventActionList = new List<List<EventAction<T1, T2>>>();
            //eventActions = new Dictionary<string, List<EventAction<T1, T2>>>();
            //通过调用EventManager的静态成员，提前调用EventManager静态的构造方法
            EventManager.eventManagerClear.Add(() => EventManager<T1, T2>.Clear());

        }

        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener(string key, EventAction<T1, T2> action)
        {
            if (eventActionNameList.Contains(key))
            {
                bool isKey = false;
                eventActionNameList.ForAction((item, index) =>
                {
                    if (item == key)
                    {
                        if (eventActionList[index].Contains(action))
                            Debuger.LogError(key + "重复添加事件监听");
                        else
                            eventActionList[index].Add(action);
                        isKey = true;
                    }
                }, () => isKey);
            }
            else
            {
                eventActionNameList.Add(key);
                eventActionList.Add(new List<EventAction<T1, T2>>() { action });
            }
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener<E>(E key, EventAction<T1, T2> action) where E : System.Enum
        { AddListener(key.ToString(), action); }
        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool RemoveListener(string key)
        {
            bool isRemove = false;
            eventActionNameList.ForAction((item, index) =>
            {
                if (item == key)
                {
                    eventActionNameList.RemoveAt(index);
                    eventActionList.RemoveAt(index);
                    isRemove = true;
                }
            }, () => isRemove);
            return isRemove;
        }
        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool RemoveListener<E>(E key) where E : System.Enum
        {
            return RemoveListener(key.ToString());
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        [Obsolete("Use RemoveListener(key,action)!")]
        static public bool RemoveListener(EventAction<T1, T2> action)
        {
            int removeCount = 0;
            for (int i = 0; i < eventActionList.Count; i++)
            {
                for (int j = 0; j < eventActionList[i].Count; j++)
                {
                    if (eventActionList[i][j] == action)
                    {
                        eventActionList[i].RemoveAt(j);
                        j--;
                        removeCount++;
                    }
                }
            }
            Debuger.LogWarning("移除监听个数：" + removeCount);
            return removeCount > 0;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public bool RemoveListener(string key, EventAction<T1, T2> action)
        {
            for (int i = 0; i < eventActionList.Count; i++)
            {
                if (eventActionNameList[i] == key)
                {
                    return eventActionList[i].Remove(action);
                }
            }
            return false;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void RemoveListener<E>(E key, EventAction<T1, T2> action) where E : System.Enum
        { RemoveListener(key.ToString(), action); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        static public void Broadcast(string key, T1 t1, T2 t2)
        {
            bool isKey = false;
            eventActionNameList.ForAction((item, index) =>
            {
                if (item == key)
                {
                    eventActionList[index].ForAction(e => e?.Invoke(t1, t2));
                }
            }, () => isKey);
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        static public void Broadcast<E>(E key, T1 t1, T2 t2) where E : System.Enum
        { Broadcast(key.ToString(), t1, t2); }
        /// <summary>
        /// 清除所有委托
        /// </summary>
        static public void Clear()
        {
            Debuger.LogWarning($"clear <{typeof(T1).Name}, {typeof(T2).Name}> all events!");
            eventActionList.Clear();
            eventActionNameList.Clear();
        }
    }
}
