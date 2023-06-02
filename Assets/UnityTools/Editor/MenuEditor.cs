using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTools.UI;

namespace UnityTools.Editor
{
    /// <summary>
    /// 菜单栏功能面板
    /// </summary>
    public class MenuEditor
    {
        [MenuItem("GameObject/创建虚拟摇杆", priority = 150)]
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
    }
}