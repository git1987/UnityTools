﻿using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [MenuItem("GameObject/UnitTools/创建虚拟摇杆", priority = 1)]
        static void CreateVirtualRocker()
        {
            GameObject[] gos = SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < gos.Length; i++)
            {
                Canvas c = gos[i].GetComponentInChildren<Canvas>();
                if (c != null)
                {
                    GameObject vrObj = VirtualRocker.CreateGameObject(c.gameObject);
                    Debuger.Log("创建虚拟摇杆成功", vrObj);
                    Debuger.Log("Canvas", c.gameObject);
                    return;
                }
            }
            Debuger.LogError("先创建Canvas!");
        }
        [MenuItem("GameObject/UnitTools/GameObjectPool #&P", priority = 2)]
        static void CreateGameObjectPool()
        {
            GameObject[] gos = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject pool = null;
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
    }
}