using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace UnityTools.Editor
{
    public class EditorTools
    {
        public static O GetResourcesAsset<O>(string name) where O : Object
        {
            Object[] objs = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra");
            foreach (var o in objs)
            {
                if (o.name == name)
                {
                    if (o is O)
                    {
                        return o as O;
                    }
                }
            }
            return null;
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