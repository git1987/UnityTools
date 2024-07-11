using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityTools.Extend;
using UnityTools.Single;
using UnityTools.UI;
namespace UnityTools.Editor
{
    /// <summary>
    /// 菜单栏功能面板
    /// </summary>
    public class MenuEditor
    {
        [MenuItem("GameObject/UnityTools/无极虚拟摇杆", priority = 1)]
        static void CreateInfiniteVirtualRocker()
        {
            GameObject[] gos = SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < gos.Length; i++)
            {
                Canvas c = gos[i].GetComponentInChildren<Canvas>();
                if (c != null)
                {
                    GameObject vrObj = InfiniteVirtualRockerGameObject(c.gameObject);
                    Debuger.Log("创建虚拟摇杆成功", vrObj);
                    Debuger.Log("Canvas", c.gameObject);
                    return;
                }
            }
            Debuger.LogError("先创建Canvas!");
        }
        /// <summary>
        /// 在CanvasGameObject下创建一个无极虚拟摇杆
        /// </summary>
        /// <param name="canvasObj"></param>
        /// <returns></returns>
        public static GameObject InfiniteVirtualRockerGameObject(GameObject canvasObj)
        {
            Canvas canvas = canvasObj.GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("请选择带有Canvas组件的GameObject");
                return null;
            }
            RectTransform          canvasRect    = canvas.transform as RectTransform;
            GameObject             rocker        = new GameObject("VirtualRocker");
            VirtualRocker_Infinite virtualRocker = rocker.AddComponent<VirtualRocker_Infinite>();
            virtualRocker.SetCanvas(canvasRect);
            RectTransform rect = rocker.AddComponent<RectTransform>();
            rect.SetParentReset(canvasRect);
            rect.SetSurround();
            GameObject    bgObj       = new GameObject("pointBg");
            RectTransform pointBgRect = bgObj.AddComponent<RectTransform>();
            pointBgRect.SetParentReset(rect);
            pointBgRect.sizeDelta = Vector2.one * 200;
            pointBgRect.anchoredPosition = new Vector2(canvasRect.sizeDelta.x / 2f, canvasRect.sizeDelta.y / 2f);
            pointBgRect.anchorMin = Vector2.zero;
            pointBgRect.anchorMax = Vector2.zero;
            bgObj.MateComponent<Image>().raycastTarget = false;
            bgObj.MateComponent<Image>().color = new Color(1, 1, 1, .5f);
            bgObj.MateComponent<Image>().sprite = EditorTools.GetResourcesAsset<Sprite>("Knob");
            GameObject    pointerObj  = new GameObject("pointer");
            RectTransform pointerRect = pointerObj.AddComponent<RectTransform>();
            pointerRect.SetParentReset(bgObj.transform);
            pointerRect.sizeDelta                          = new Vector2(10, 100);
            pointerRect.anchoredPosition                   = Vector2.zero;
            pointerRect.pivot                              = new Vector2(.5f, 0);
            pointerObj.AddComponent<Image>().raycastTarget = false;
            GameObject    pointObj  = new GameObject("point");
            RectTransform pointRect = pointObj.AddComponent<RectTransform>();
            pointRect.SetParentReset(bgObj.transform);
            pointRect.sizeDelta                           = Vector2.one * 50;
            pointRect.anchoredPosition                    = Vector2.zero;
            pointObj.MateComponent<Image>().raycastTarget = false;
            pointObj.MateComponent<Image>().color         = Color.red;
            pointObj.MateComponent<Image>().sprite        = EditorTools.GetResourcesAsset<Sprite>("Knob");
            GameObject    area     = new GameObject("area");
            RectTransform areaRect = area.AddComponent<RectTransform>();
            areaRect.SetParentReset(rect);
            areaRect.SetAsFirstSibling();
            areaRect.sizeDelta = Vector2.one * 500;
            MaskGraphic areaMask = area.AddComponent<MaskGraphic>();
            areaMask.raycastTarget = false;
            virtualRocker.SetRects(areaRect, pointBgRect, pointRect, pointerRect);
            return rocker;
        }
        [MenuItem("GameObject/UnityTools/4方向虚拟摇杆", priority = 1)]
        static void Create4DirectionVirtualRocker()
        {
            GameObject[] gos = SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < gos.Length; i++)
            {
                Canvas c = gos[i].GetComponentInChildren<Canvas>();
                if (c != null)
                {
                    GameObject vrObj = Create4DirectionGameObject(c.gameObject);
                    Debuger.Log("创建虚拟摇杆成功", vrObj);
                    return;
                }
            }
            Debuger.LogError("先创建Canvas!");
        }
        public static GameObject Create4DirectionGameObject(GameObject canvasObj)
        {
            //方向按键、间距的size
            /*
             *       口
             *     口〇口
             *       口 
             */
            float      size  = 50;
            GameObject vrObj = new GameObject("VirtualRocker");
            vrObj.transform.SetParentReset(canvasObj.transform);
            RectTransform vrRect = vrObj.AddComponent<RectTransform>();
            vrRect.anchoredPosition = Vector2.zero;
            vrRect.sizeDelta        = new(size, size);
            VirtualRocker_8Direction vr = vrObj.AddComponent<VirtualRocker_8Direction>();
            string[] directionNames =
            {
                "center", "up", "left", "right", "down"
            };
            Vector2[] posArray =
            {
                Vector2.zero, Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            };
            GameObject[] directionRects = new GameObject[directionNames.Length];
            for (int i = 0; i < directionNames.Length; i++)
            {
                GameObject    directionObj  = new GameObject(directionNames[i]);
                RectTransform directionRect = directionObj.AddComponent<RectTransform>();
                directionRects[i]       = directionObj;
                directionRect.sizeDelta = new(size, size);
                directionRect.SetParentReset(vrRect);
                directionRect.anchoredPosition = posArray[i] * size;
                Image image = directionObj.AddComponent<Image>();
                if (i == 0)
                {
                    image.sprite = EditorTools.GetResourcesAsset<Sprite>("Knob");
                }
            }
            vr.SetDirectionObj(directionRects);
            return vrObj;
        }
        [MenuItem("GameObject/UnityTools/8方向虚拟摇杆", priority = 1)]
        static void Create8DirectionVirtualRocker()
        {
            GameObject[] gos = SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < gos.Length; i++)
            {
                Canvas c = gos[i].GetComponentInChildren<Canvas>();
                if (c != null)
                {
                    GameObject vrObj = Create8DirectionGameObject(c.gameObject);
                    Debuger.Log("创建虚拟摇杆成功", vrObj);
                    return;
                }
            }
            Debuger.LogError("先创建Canvas!");
        }
        public static GameObject Create8DirectionGameObject(GameObject canvasObj)
        {
            //方向按键、间距的size
            /*
             *      o口o
             *     口〇口
             *      o口o
             */
            float      size  = 50;
            GameObject vrObj = new GameObject("VirtualRocker");
            vrObj.transform.SetParentReset(canvasObj.transform);
            RectTransform vrRect = vrObj.AddComponent<RectTransform>();
            vrRect.anchoredPosition = Vector2.zero;
            vrRect.sizeDelta        = new(size, size);
            VirtualRocker_8Direction vr = vrObj.AddComponent<VirtualRocker_8Direction>();
            string[] directionNames =
            {
                "center", "up", "left", "right", "down", "upLeft", "upRight", "downLeft", "downRight"
            };
            Vector2[] posArray =
            {
                Vector2.zero, Vector2.up, Vector2.down, Vector2.left, Vector2.right, new(-1, 1), new(1, 1), new(-1, -1),
                new(1, -1),
            };
            GameObject[] directionRects = new GameObject[directionNames.Length];
            for (int i = 0; i < directionNames.Length; i++)
            {
                GameObject    directionObj  = new GameObject(directionNames[i]);
                RectTransform directionRect = directionObj.AddComponent<RectTransform>();
                directionRects[i]       = directionObj;
                directionRect.sizeDelta = new(size, size);
                directionRect.SetParentReset(vrRect);
                directionRect.anchoredPosition = posArray[i] * size;
                Image image = directionObj.AddComponent<Image>();
                if (i == 0)
                {
                    image.sprite = EditorTools.GetResourcesAsset<Sprite>("Knob");
                }
            }
            vr.SetDirectionObj(directionRects);
            return vrObj;
        }
        [MenuItem("GameObject/UnityTools/GameObjectPool #&P", priority = 2)]
        static void CreateGameObjectPool()
        {
            GameObject[] gos  = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject   pool = null;
            for (int i = 0; i < gos.Length; i++)
            {
                GameObjectPool p = gos[i].GetComponentInChildren<GameObjectPool>();
                if (p != null)
                {
                    pool = p.gameObject;
                    break;
                }
            }
            if (pool == null)
            {
                pool = new GameObject("GameObjectPool");
                pool.MateComponent<GameObjectPool>();
            }
            Debuger.LogWarning("创建GameObjectPool", pool);
        }
        [MenuItem("UnityTools/UI/HideRaycastTarget")]
        //关闭UI中的RaycastTarget射线检测
        private static void HideRaycastTarget()
        {
            GameObject[] gos = Selection.gameObjects;
            if (gos != null)
            {
                foreach (GameObject go in gos)
                {
                    MaskableGraphic[] mgs = go.GetComponentsInChildren<MaskableGraphic>(true);
                    foreach (MaskableGraphic mg in mgs)
                    {
                        mg.raycastTarget = false;
                    }
                }
            }
        }
    }
}