using System.Collections.Generic;
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
        /// <param name="go"></param>
        /// <param name="resetTransform">是否重置transform</param>
        public static void Recover(GameObject go, bool resetTransform = false)
        {
            if (go == null) return;
            if (instance == null) Destroy(go);
            else instance.RecoverObj(go, resetTransform);
        }
        /// <summary>
        /// 获取一个GameObject对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject Get(string name)
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
            get { return this.GetObj(name); }
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
                    Debuger.LogWarning($"[{prefab.name}] already exists", this.gameObject);
                }
                else
                {
                    Queue<GameObject> goQueue = new Queue<GameObject>();
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
                        goQueue.Enqueue(go);
                    }
                    pools.Add(prefab.name, goQueue);
                    poolPrefab.Add(prefab.name, prefab);
#if UNITY_EDITOR
                    tempList.Add(prefab);
#endif
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
            if (pools.TryGetValue(name, out Queue<GameObject> queue))
            {
                if (poolCount.ContainsKey(name))
                {
                    poolCount[name] = Mathf.Max(count, queue.Count);
                }
                else
                {
                    poolCount.Add(name, Mathf.Max(count, queue.Count));
                }
            }
            else
            {
                Debuger.LogError($"[{name}] does not exist");
            }
        }
        /// <summary>
        /// 库存：对象池中当前对象的个数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int Stock(string name)
        {
            if (poolCount.TryGetValue(name, out int count))
            {
                return count;
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
            if (poolCount.TryGetValue(name, out int count))
            {
                poolCount[name] = Mathf.Max(count + cashier, 0);
                Debuger.LogWarning($"[{name}]remaining number：{poolCount[name]}");
            }
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetObj(string name)
        {
            GameObject temp = null;
            if (poolPrefab.TryGetValue(name, out GameObject go))
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
                        temp = Instantiate(go);
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
                Debuger.LogError($"[{name}] does not exist");
            }
            return temp;
        }
        /// <summary>
        /// 回收对象
        /// <para>resetTransform:</para>
        /// <para>localPosition = Vector3.zero</para>
        /// <para>localRotation = Quaternion.identity[Vector3.zero]</para>
        /// <para>localScale = Vector3.one</para>
        /// </summary>
        /// <param name="go"></param>
        /// <param name="resetTransform">重置transform</param>
        private void RecoverObj(GameObject go, bool resetTransform)
        {
            if (go == null) return;
            if (poolPrefab.ContainsKey(go.name))
            {
                go.transform.SetParent(this.transform);
                if (resetTransform)
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
                Debug.LogWarning($"[{go.name}] is not in Pool");
                Destroy(go);
            }
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
                pools[name].Clear();
                pools.Remove(name);
                poolPrefab.Remove(name);
                Debuger.LogWarning($"remove [{name}]");
            }
            else
            {
                Debuger.LogError($"[{name}] does not exist");
            }
        }
    }
}