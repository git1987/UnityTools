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
        /// 回收GameObject对象，如果没有创建Pool则被Destroy掉
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
        private readonly Dictionary<string, GameObject> poolPrefab = new Dictionary<string, GameObject>();
        private readonly Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
        private readonly Dictionary<string, int> poolCount = new Dictionary<string, int>();
#if UNITY_EDITOR
        /// <summary>
        /// 便于编辑器内查看
        /// </summary>
        [SerializeField] private List<GameObject> tempList = new List<GameObject>();
#endif
        /// <summary>
        /// GetObj()
        /// </summary>
        /// <param name="gameObjectName"></param>
        /// <returns></returns>
        public GameObject this[string gameObjectName] { get => this.GetObj(name); set => RecoverObj(value, false); }
        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="prefab">Prefab</param>
        /// <param name="count">初始化数量</param>
        /// <param name="reset">重置transform</param>
        /// <returns>Pool</returns>
        public Pool Init(GameObject prefab, int count = 0, bool reset = false)
        {
            if (prefab == null) { Debuger.LogError("the prefab is null"); }
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
                        Transform tran = go.transform;
                        if (reset)
                        {
                            tran.localPosition = Vector3.zero;
                            tran.localRotation = Quaternion.Euler(Vector3.zero);
                            tran.localScale    = Vector3.one;
                        }
                        tran.SetParent(this.transform);
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
        /// <param name="gameObjectName"></param>
        /// <returns></returns>
        public bool Has(string gameObjectName) { return poolPrefab.ContainsKey(name); }
        /// <summary>
        /// 设置对象容量
        /// </summary>
        /// <param name="gameObjectName"></param>
        /// <param name="count"></param>
        public void SetResize(string gameObjectName, int count)
        {
            if (pools.TryGetValue(gameObjectName, out Queue<GameObject> queue))
            {
                if (poolCount.ContainsKey(gameObjectName))
                {
                    poolCount[gameObjectName] = Mathf.Max(count, queue.Count);
                }
                else { poolCount.Add(gameObjectName, Mathf.Max(count, queue.Count)); }
            }
            else { Debuger.LogError($"[{gameObjectName}] does not exist"); }
        }
        /// <summary>
        /// 库存：对象池中当前对象的个数
        /// </summary>
        /// <param name="gameObjectName"></param>
        /// <returns></returns>
        private int Stock(string gameObjectName)
        {
            if (poolCount.TryGetValue(gameObjectName, out int count)) { return count; }
            return int.MaxValue;
        }
        /// <summary>
        /// 对象出纳：+1入库，-1出库
        /// </summary>
        /// <param name="gameObjectName"></param>
        /// <param name="cashier"></param>
        private void Transfer(string gameObjectName, short cashier)
        {
            if (poolCount.TryGetValue(gameObjectName, out int count))
            {
                poolCount[gameObjectName] = Mathf.Max(count + cashier, 0);
                Debuger.LogWarning($"[{gameObjectName}]remaining number：{poolCount[gameObjectName]}");
            }
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="gameObjectName"></param>
        /// <returns></returns>
        public GameObject GetObj(string gameObjectName)
        {
            GameObject temp = null;
            if (poolPrefab.TryGetValue(gameObjectName, out GameObject go))
            {
                if (Stock(gameObjectName) > 0)
                {
                    if (pools[gameObjectName].Count > 0)
                    {
                        temp = pools[gameObjectName].Dequeue();
                        temp.transform.SetParent(null);
                    }
                    else
                    {
                        temp      = Instantiate(go);
                        temp.name = gameObjectName;
                    }
                    temp.SetActive(true);
                    Transfer(gameObjectName, -1);
                }
                else { Debuger.LogWarning("not enough " + gameObjectName); }
            }
            else { Debuger.LogError($"[{gameObjectName}] does not exist"); }
            return temp;
        }
        /// <summary>
        /// 回收对象：如果不属于对象池的GameObject则被Destroy
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
                    Transform tran = go.transform;
                    tran.localPosition = Vector3.zero;
                    tran.localRotation = Quaternion.identity;
                    tran.localScale    = Vector3.one;
                }
                go.SetActive(false);
                pools[go.name].Enqueue(go);
                Transfer(name, 1);
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
        /// <param name="gameObjectName"></param>
        public void RemoveObj(string gameObjectName)
        {
            if (poolPrefab.ContainsKey(gameObjectName))
            {
                foreach (GameObject go in pools[gameObjectName]) { Destroy(go); }
#if UNITY_EDITOR
                tempList.Remove(poolPrefab[gameObjectName]);
#endif
                pools[gameObjectName].Clear();
                pools.Remove(gameObjectName);
                poolPrefab.Remove(gameObjectName);
                Debuger.LogWarning($"remove [{gameObjectName}]");
            }
            else { Debuger.LogError($"[{gameObjectName}] does not exist"); }
        }
    }
}