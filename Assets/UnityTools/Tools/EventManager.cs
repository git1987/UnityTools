using System;
using System.Collections.Generic;
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
    /// 3参无返回值委托
    /// </summary>
    public delegate void EventAction<A, B, C>(A a, B b, C c);

    /// <summary>
    /// 4参无返回值委托
    /// </summary>
    public delegate void EventAction<A, B, C, D>(A a, B b, C c, D d);

    /// <summary>
    /// 5参无返回值委托
    /// </summary>
    public delegate void EventAction<A, B, C, D, E>(A a, B b, C c, D d, E e);

    /// <summary>
    /// 6参无返回值委托
    /// </summary>
    public delegate void EventAction<A, B, C, D, E, F>(A a, B b, C c, D d, E e, F f);

    /// <summary>
    /// 无参1返回值委托
    /// </summary>
    public delegate R EventFunction<R>();

    /// <summary>
    /// 1参1返回值委托
    /// </summary>
    public delegate R EventFunction<in A, R>(A a);

    /// <summary>
    /// 2参1返回值委托
    /// </summary>
    public delegate R EventFunction<in A, in B, R>(A a, B b);

    /// <summary>
    /// 无参数事件监听
    /// </summary>
    public static class EventManager
    {
        class EventData
        {
            public string key;
            public List<EventAction> actionList;
        }

        /// <summary>
        /// 带参数的EventManager事件监听清除
        /// </summary>
        static public List<EventAction> eventManagerClear { private set; get; }
        static private List<EventData> eventList;
        static EventManager()
        {
            eventManagerClear = new List<EventAction>();
            eventList         = new List<EventData>();
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener(string key, EventAction action)
        {
            bool isKey = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.key == key)
                {
                    if (eventData.actionList.Contains(action))
                    {
                        Debuger.LogError(key + "重复添加事件监听");
                    }
                    else
                    {
                        eventData.actionList.Add(action);
                    }
                    isKey = true;
                }
            }, () => isKey);
            if (!isKey)
            {
                eventList.Add(new EventData()
                {
                    key = key,
                    actionList = new List<EventAction>
                    {
                        action
                    }
                });
            }
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener<E>(E key, EventAction action) where E : System.Enum
        {
            AddListener(key.ToString(), action);
        }
        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool RemoveListener(string key)
        {
            bool isRemove = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.key == key)
                {
                    eventData.actionList = null;
                    eventList.RemoveAt(index);
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
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.actionList.Remove(action))
                {
                    removeCount++;
                }
            });
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
            bool isRemove = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.actionList.Remove(action))
                {
                    isRemove = true;
                }
            }, () => isRemove);
            if (!isRemove) Debuger.LogWarning($"{key}中不包含指定的回调");
            return isRemove;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void RemoveListener<E>(E key, EventAction action) where E : System.Enum
        {
            RemoveListener(key.ToString(), action);
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        static public void Broadcast(string key)
        {
            bool isKey = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.key == key)
                {
                    eventData.actionList.ForAction(e => e?.Invoke());
                }
            }, () => isKey);
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        static public void Broadcast<E>(E key) where E : System.Enum
        {
            Broadcast(key.ToString());
        }
        /// <summary>
        /// 清除所有委托
        /// </summary>
        static public void Clear()
        {
            eventList.Clear();
            for (int i = 0; i < eventManagerClear.Count; i++) eventManagerClear[i]?.Invoke();
        }
    }

    /// <summary>
    /// 1参数事件监听
    /// </summary>
    public static class EventManager<T>
    {
        class EventData
        {
            public string key;
            public List<EventAction<T>> actionList;
        }

        static private List<EventData> eventList;
        static EventManager()
        {
            eventList = new List<EventData>();
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
            bool isKey = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.key == key)
                {
                    if (eventData.actionList.Contains(action))
                    {
                        Debuger.LogError(key + "重复添加事件监听");
                    }
                    else
                    {
                        eventData.actionList.Add(action);
                    }
                    isKey = true;
                }
            }, () => isKey);
            if (!isKey)
            {
                eventList.Add(new EventData()
                {
                    key = key,
                    actionList = new List<EventAction<T>>
                    {
                        action
                    }
                });
            }
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener<E>(E key, EventAction<T> action) where E : System.Enum
        {
            AddListener(key.ToString(), action);
        }
        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool RemoveListener(string key)
        {
            bool isRemove = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.key == key)
                {
                    eventData.actionList = null;
                    eventList.RemoveAt(index);
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
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.actionList.Remove(action))
                {
                    removeCount++;
                }
            });
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
            bool isRemove = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.actionList.Remove(action))
                {
                    isRemove = true;
                }
            }, () => isRemove);
            if (!isRemove) Debuger.LogWarning($"{key}中不包含指定的回调");
            return isRemove;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void RemoveListener<E>(E key, EventAction<T> action) where E : System.Enum
        {
            RemoveListener(key.ToString(), action);
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        static public void Broadcast(string key, T t)
        {
            bool isKey = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.key == key)
                {
                    eventData.actionList.ForAction(e => e?.Invoke(t));
                }
            }, () => isKey);
            if (!isKey)
            {
                Debuger.LogWarning($"不存在{key}的事件监听");
            }
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        static public void Broadcast<E>(E key, T t) where E : System.Enum
        {
            Broadcast(key.ToString(), t);
        }
        /// <summary>
        /// 清除所有委托
        /// </summary>
        static public void Clear()
        {
            Debuger.LogWarning($"clear <{typeof(T).Name}> all events!");
            eventList.Clear();
        }
    }

    /// <summary>
    /// 2参数事件监听
    /// </summary>
    public static class EventManager<T1, T2>
    {
        class EventData
        {
            public string key;
            public List<EventAction<T1, T2>> actionList;
        }

        static private List<EventData> eventList;
        static EventManager()
        {
            eventList = new List<EventData>();
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
            bool isKey = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.key == key)
                {
                    if (eventData.actionList.Contains(action))
                    {
                        Debuger.LogError(key + "重复添加事件监听");
                    }
                    else
                    {
                        eventData.actionList.Add(action);
                    }
                    isKey = true;
                }
            }, () => isKey);
            if (!isKey)
            {
                eventList.Add(new EventData()
                {
                    key = key,
                    actionList = new List<EventAction<T1, T2>>
                    {
                        action
                    }
                });
            }
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener<E>(E key, EventAction<T1, T2> action) where E : System.Enum
        {
            AddListener(key.ToString(), action);
        }
        /// <summary>
        /// 移除委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public bool RemoveListener(string key)
        {
            bool isRemove = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.key == key)
                {
                    eventData.actionList = null;
                    eventList.RemoveAt(index);
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
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.actionList.Remove(action))
                {
                    removeCount++;
                }
            });
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
            bool isRemove = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.actionList.Remove(action))
                {
                    isRemove = true;
                }
            }, () => isRemove);
            if (!isRemove) Debuger.LogWarning($"{key}中不包含指定的回调");
            return isRemove;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void RemoveListener<E>(E key, EventAction<T1, T2> action) where E : System.Enum
        {
            RemoveListener(key.ToString(), action);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        static public void Broadcast(string key, T1 t1, T2 t2)
        {
            bool isKey = false;
            eventList.ForAction((eventData, index) =>
            {
                if (eventData.key == key)
                {
                    eventData.actionList.ForAction(e => e?.Invoke(t1, t2));
                }
            }, () => isKey);
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        static public void Broadcast<E>(E key, T1 t1, T2 t2) where E : System.Enum
        {
            Broadcast(key.ToString(), t1, t2);
        }
        /// <summary>
        /// 清除所有委托
        /// </summary>
        static public void Clear()
        {
            Debuger.LogWarning($"clear <{typeof(T1).Name}, {typeof(T2).Name}> all events!");
            eventList.Clear();
        }
    }
}