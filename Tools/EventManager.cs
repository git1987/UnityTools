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
        static public List<EventAction> eventManagerClear;
        static readonly Dictionary<string, List<EventAction>> eventActions;
        static EventManager()
        {
            eventManagerClear = new List<EventAction>();
            eventActions = new Dictionary<string, List<EventAction>>();
        }
        /// <summary>
        /// 标记key，添加一个事件监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void AddListener(string key, EventAction action)
        {
            if (eventActions.ContainsKey(key))
            {
                if (eventActions[key].Contains(action))
                    Debuger.LogError(key + "重复添加事件监听");
                else
                    eventActions[key].Add(action);
            }
            else
            {
                eventActions.Add(key, new List<EventAction>() { action });
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
            return eventActions.Remove(key);
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
        static public bool RemoveListener(EventAction action)
        {
            foreach (string key in eventActions.Keys)
            {
                for (int i = 0; i < eventActions[key].Count; i++)
                {
                    if (eventActions[key][i] == action)
                    {
                        eventActions[key].RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void RemoveListener(string key, EventAction action)
        {
            if (eventActions.ContainsKey(key))
            {
                if (eventActions[key].Remove(action))
                {
                }
            }
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
            if (eventActions.ContainsKey(key))
            {
                foreach (EventAction ea in eventActions[key])
                {
                    ea?.Invoke();
                }
            }
            else
            {
                Debuger.LogError($"没有[{key}]类型的委托");
            }
        }
        /// <summary>
        /// 清除所有委托
        /// </summary>
        static public void Clear()
        {
            eventActions.Clear();
            for (int i = 0; i < eventManagerClear.Count; i++)
                eventManagerClear[i]?.Invoke();
        }
    }

    /// <summary>
    /// 1参数事件监听
    /// </summary>
    public static class EventManager<T>
    {
        static readonly Dictionary<string, List<EventAction<T>>> eventActions;
        static EventManager()
        {
            eventActions = new Dictionary<string, List<EventAction<T>>>();
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
            if (eventActions.ContainsKey(key))
            {
                if (eventActions[key].Contains(action))
                    Debuger.LogError(key + "重复添加事件监听");
                else
                    eventActions[key].Add(action);
            }
            else
            {
                eventActions.Add(key, new List<EventAction<T>>() { action });
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
            return eventActions.Remove(key);
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
        static public bool RemoveListener(EventAction<T> action)
        {
            foreach (string key in eventActions.Keys)
            {
                for (int i = 0; i < eventActions[key].Count; i++)
                {
                    if (eventActions[key][i] == action)
                    {
                        eventActions[key].RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void RemoveListener(string key, EventAction<T> action)
        {
            if (eventActions.ContainsKey(key))
            {
                if (eventActions[key].Remove(action))
                {
                }
            }
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
            if (eventActions.ContainsKey(key))
            {
                foreach (EventAction<T> ea in eventActions[key])
                {
                    ea?.Invoke(t);
                }
            }
            else
            {
                Debuger.LogError($"没有[{key}]类型的委托");
            }
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
            eventActions.Clear();
        }
    }

    /// <summary>
    /// 2参数事件监听
    /// </summary>
    public static class EventManager<T1, T2>
    {
        static readonly Dictionary<string, List<EventAction<T1, T2>>> eventActions;
        static EventManager()
        {
            eventActions = new Dictionary<string, List<EventAction<T1, T2>>>();
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
            if (eventActions.ContainsKey(key))
            {
                if (eventActions[key].Contains(action))
                    Debuger.LogError(key + "重复添加事件监听");
                else
                    eventActions[key].Add(action);
            }
            else
            {
                eventActions.Add(key, new List<EventAction<T1, T2>>() { action });
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
            return eventActions.Remove(key);
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
        static public bool RemoveListener(EventAction<T1, T2> action)
        {
            foreach (string key in eventActions.Keys)
            {
                for (int i = 0; i < eventActions[key].Count; i++)
                    if (eventActions[key][i] == action)
                    {
                        eventActions[key].RemoveAt(i);
                        return true;
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
        /// 根据标记移除监听
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        static public void RemoveListener(string key, EventAction<T1, T2> action)
        {
            if (eventActions.ContainsKey(key))
            {
                if (eventActions[key].Remove(action))
                {
                }
            }
        }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        static public void Broadcast<E>(E key, T1 t1, T2 t2) where E : System.Enum
        { Broadcast(key.ToString(), t1, t2); }
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        static public void Broadcast(string key, T1 t1, T2 t2)
        {
            if (eventActions.ContainsKey(key))
            {
                foreach (EventAction<T1, T2> ea in eventActions[key])
                {
                    ea?.Invoke(t1, t2);
                }
            }
            else
            {
                Debuger.LogError($"没有[{key}]类型的委托");
            }
        }
        /// <summary>
        /// 清除所有委托
        /// </summary>
        static public void Clear()
        {
            eventActions.Clear();
        }
    }
}
