using UnityEngine;
namespace UnityTools.Single
{
    /// <summary>
    /// MonoBehaviour单列基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingleMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        static private T _instance = null;
        /// <summary>
        /// 单例类对象：需要自行判断是否为空，若想使用不为空的单例，使用GetInstance()
        /// </summary>
        static public T instance
        {
            get { return _instance; }
        }
        /// <summary>
        /// 获取单例类对象，若没有则创建一个
        /// </summary>
        /// <returns></returns>
        static public T GetInstance()
        {
            if (_instance == null)
            {
                if (Application.isPlaying)
                {
                    GameObject temp = new GameObject(typeof(T).Name);
                    Debuger.LogWarning("创建了一个" + typeof(T).Name, temp);
                    _instance = temp.AddComponent<T>();
                }
            }
            return _instance;
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                T t = this.GetComponent<T>();
                _instance = t;
            }
            else
            {
                if (_instance != this)
                {
                    Debuger.LogErrorFormat(string.Format("The [{0}] already exists", this.GetType().Name), this.gameObject);
                    Destroy(this);
                }
            }
        }
        /// <summary>
        /// 设置为切换场景不删除
        /// </summary>
        public void SetDontDestroy()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
