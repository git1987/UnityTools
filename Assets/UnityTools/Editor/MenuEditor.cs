﻿using UnityEditor;
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
            rocker.transform.SetParentReset(canvasRect);
            RectTransform rect = rocker.AddComponent<RectTransform>();
            rect.SetSurround();
            GameObject bg = new GameObject("pointBg");
            rect = bg.AddComponent<RectTransform>();
            virtualRocker.Set_pointBg(rect);
            rect.SetParentReset(rocker.transform);
            rect.sizeDelta = Vector2.one * 200;
            rect.anchoredPosition = new Vector2(canvasRect.sizeDelta.x / 2f, canvasRect.sizeDelta.y / 2f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            bg.MateComponent<Image>().raycastTarget = false;
            bg.MateComponent<Image>().color = new Color(1, 1, 1, .5f);
            bg.MateComponent<Image>().sprite = EditorTools.GetResourcesAsset<Sprite>("Knob");
            GameObject pointer = new GameObject("pointer");
            rect = pointer.AddComponent<RectTransform>();
            virtualRocker.Set_pointer(rect);
            rect.SetParentReset(bg.transform);
            rect.sizeDelta                              = new Vector2(10, 100);
            rect.anchoredPosition                       = Vector2.zero;
            rect.pivot                                  = new Vector2(.5f, 0);
            pointer.AddComponent<Image>().raycastTarget = false;
            GameObject point = new GameObject("point");
            rect = point.AddComponent<RectTransform>();
            virtualRocker.Set_point(rect);
            rect.SetParentReset(bg.transform);
            rect.sizeDelta                             = Vector2.one * 50;
            rect.anchoredPosition                      = Vector2.zero;
            point.MateComponent<Image>().raycastTarget = false;
            point.MateComponent<Image>().color         = Color.red;
            point.MateComponent<Image>().sprite        = EditorTools.GetResourcesAsset<Sprite>("Knob");
            GameObject area = new GameObject("area");
            rect = area.AddComponent<RectTransform>();
            virtualRocker.SetareaRect(rect);
            rect.SetParentReset(rocker.transform);
            rect.SetAsFirstSibling();
            rect.sizeDelta = Vector2.one * 500;
            MaskGraphic areaMask = area.AddComponent<MaskGraphic>();
            areaMask.raycastTarget = false;
            return rocker;
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
                    Debuger.LogError("暂无8方向虚拟摇杆!");
                    // GameObject vrObj = Create8DirectionGameObject(c.gameObject);
                    // Debuger.Log("创建虚拟摇杆成功", vrObj);
                    // Debuger.Log("Canvas", c.gameObject);
                    return;
                }
            }
            Debuger.LogError("先创建Canvas!");
        }
        public static GameObject Create8DirectionGameObject(GameObject canvasObj)
        {
            return null;
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