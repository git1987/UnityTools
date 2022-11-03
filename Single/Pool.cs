using UnityEngine;
namespace UnityTools.Single
{
    /// <summary>
    /// GameObject对象池
    /// </summary>
    public class Pool : SingleMono<Pool>
    {
        /// <summary>
        /// 回收GameObject对象
        /// </summary>
        public static void Recover(GameObject go, bool reset = false)
        {
            if (go == null) return;
            if (instance == null) Destroy(go);
            else instance.RecoverObj(go, reset);
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject GetObj(string name)
        {
            if (instance != null) return instance[name];
            Debuger.LogError("There is no Pool component in the scene");
            return null;
        }
        private Dictionary<string, GameObject> poolPrefab = new Dictionary<string, GameObject>();
        private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, int> poolCount = new Dictionary<string, int>();
#if UNITY_EDITOR
        /// <summary>
        /// 便于编辑器内查看
        /// </summary>
        [SerializeField]
        private List<GameObject> tempList = new List<GameObject>();
#endif
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject this[string name]
        {
            get { return GetObj(name); }
            set { RecoverObj(value, false); }
        }
        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="prefab">Prefab</param>
        /// <param name="count">初始化数量</param>
        /// <param name="reset">重置transform</param>
        /// <returns>Pool</returns>
        public Pool Init(GameObject prefab, int count = 0, bool reset = false)
        {
            if (prefab == null)
            {
                Debuger.LogError("the prefab is null");
            }
            else
            {
                if (poolPrefab.ContainsKey(prefab.name))
                {
                    Queue<GameObject> gos = new Queue<GameObject>();
                    for (int i = 0; i < count; i++)
                    {
                        GameObject go = Instantiate(prefab);
                        if (reset)
                        {
                            go.transform.localPosition = Vector3.zero;
                            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
                            go.transform.localScale = Vector3.one;
                        }
                        go.transform.SetParent(this.transform);
                        go.SetActive(false);
                        gos.Enqueue(go);
                    }
                    pools.Add(prefab.name, gos);
                    poolPrefab.Add(prefab.name, prefab);
#if UNITY_EDITOR
                    tempList.Add(prefab);
#endif
                }
                else
                {
                    Debuger.LogWarning(prefab.name + "a lready exists", this.gameObject);
                }
            }
            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Has(string name)
        {
            return poolPrefab.ContainsKey(name);
        }
        /// <summary>
        /// 设置对象容量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        public void SetResize(string name, int count)
        {
            if (pools.ContainsKey(name))
            {
                if (poolCount.ContainsKey(name))
                {
                    poolCount[name] = Mathf.Max(count, pools[name].Count);
                }
                else
                {
                    poolCount.Add(name, Mathf.Max(count, pools[name].Count));
                }
            }
            else
            {
                Debuger.LogErrorFormat("{0} does not exist", name);
            }
        }
        /// <summary>
        /// 库存：对象池中当前对象的个数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int Stock(string name)
        {
            if (poolCount.ContainsKey(name))
            {
                return poolCount[name];
            }
            return int.MaxValue;
        }
        /// <summary>
        /// 对象出纳：+1入库，-1出库
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cashier"></param>
        private void Transfer(string name, short cashier)
        {
            if (poolCount.ContainsKey(name))
                poolCount[name] = Mathf.Max(poolCount[name] + cashier, 0);
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetObj(string name)
        {
            GameObject temp = null;
            if (poolPrefab.ContainsKey(name))
            {
                if (Stock(name) > 0)
                {
                    if (pools[name].Count > 0)
                    {
                        temp = pools[name].Dequeue();
                        temp.transform.SetParent(null);
                    }
                    else
                    {
                        temp = Instantiate(poolPrefab[name]);
                        temp.name = name;
                    }
                    temp.SetActive(true);
                    Transfer(name, -1);
                }
                else
                {
                    Debuger.LogWarning("not enough " + name);
                }
            }
            else
            {
                Debuger.LogErrorFormat("{0} does not exist", name);
            }
            return temp;
        }
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="reset">重置transform</param>
        void RecoverObj(GameObject go, bool reset)
        {
            if (go == null) return;
            if (poolPrefab.ContainsKey(go.name))
            {
                go.transform.SetParent(this.transform);
                if (reset)
                {
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.identity;
                    go.transform.localScale = Vector3.one;
                }
                go.SetActive(false);
                pools[go.name].Enqueue(go);
                Transfer(name, 1);
                return;
            }
            else
            {
                Destroy(go);
            }
        }
        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="go"></param>
        public void Reomve(GameObject go)
        {
            if (go != null)
                Reomve(go.name);
        }
        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="name"></param>
        public void Reomve(string name)
        {
            if (poolPrefab.ContainsKey(name))
            {
                foreach (GameObject go in pools[name])
                {
                    Destroy(go);
                }
#if UNITY_EDITOR
                tempList.Remove(poolPrefab[name]);
#endif
                pools[name] = null;
                pools.Remove(name);
                poolPrefab.Remove(name);
            }
        }
    }
}