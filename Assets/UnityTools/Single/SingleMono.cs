using UnityEngine;

namespace UnityTools.Single
{
    /// <summary>
    /// 普通类单例基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Single<T> where T : Single<T>, new()
    {
        private static string _EventTag_Init = null;
        public static string EventTag_Init
        {
            get
            {
                if (_EventTag_Init is not { Length: > 0 }) _EventTag_Init = typeof(T).Name + "_Init";
                return _EventTag_Init;
            }
        }
        private static T _instance;
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.InitSingleton();
                }
                return _instance;
            }
        }
        protected virtual void InitSingleton() { }
    }

    /// <summary>
    /// MonoBehaviour单列基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingleMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static string _EventTag_Awake = null;
        public static string EventTag_Awake
        {
            get
            {
                if (_EventTag_Awake is not { Length: > 0 }) _EventTag_Awake = typeof(T).Name + "_Awake";
                return _EventTag_Awake;
            }
        }
        private static string _EventTag_Destroy = null;
        public static string EventTag_Destroy
        {
            get
            {
                if (_EventTag_Destroy is not { Length: > 0 }) _EventTag_Destroy = typeof(T).Name + "_Destroy";
                return _EventTag_Destroy;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected static T _instance = null;
        /// <summary>
        /// 单例类对象：需要自行判断是否为空，若想使用不为空的单例，使用GetInstance()
        /// </summary>
        public static T instance => _instance;
        /// <summary>
        /// 
        /// 获取单例类对象，若没有则创建一个
        /// </summary>
        /// <param name="componentGameObject">指定场景中的gameObject</param>
        /// <returns></returns>
        public static T GetInstance(GameObject componentGameObject = null)
        {
            if (_instance == null)
            {
                if (Application.isPlaying)
                {
                    if (componentGameObject == null) componentGameObject = new GameObject(typeof(T).Name);
                    _instance = componentGameObject.AddComponent<T>();
                }
            }
            else
            {
                Debuger.LogWarning($"The [{typeof(T).Name}] already exists!", _instance.gameObject);
            }
            return _instance;
        }
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                T t = this.GetComponent<T>();
                _instance = t;
                EventManager<T>.Broadcast(typeof(T).Name, this as T);
            }
            else
            {
                if (_instance != this)
                {
                    Debuger.LogError($"The [{this.GetType().Name}] already exists", this.gameObject);
                    Destroy(this);
                }
            }
        }
        protected virtual void OnDestroy()
        {
            if (_instance == this) _instance = null;
            EventManager<SingleMono<T>>.Broadcast(EventTag_Destroy, this);
        }
    }
}