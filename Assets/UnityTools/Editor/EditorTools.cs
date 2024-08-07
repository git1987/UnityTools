using Codice.Client.BaseCommands.BranchExplorer;
using UnityEngine;
using UnityEditor;
namespace UnityTools.Editor
{
    public class EditorTools
    {
        /// <summary>
        /// 获取编辑器的内置资源
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="O"></typeparam>
        /// <returns></returns>
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
        /// <summary>
        /// 路径是否在工程内
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool InUnityProject(string path)
        {
            return path.IndexOf(Application.dataPath) > -1;
        }
        /// <summary>
        /// unity资源路径转文件本地路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string UnityAssetPathToFilePath(string path)
        {
            return Application.dataPath + "/" + path.Substring("Assets/".Length);
        }
        /// <summary>
        /// 本地文件路径转unity资源路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FilePathToUnityAssetPath(string path)
        {
            if (path.IndexOf(Application.dataPath) > -1)
            {
                string unityPath = "Assets/" + path.Substring(Application.dataPath.Length);
                return unityPath;
            }
            else
            {
                Debug.LogError($"不是本地工程路径：{path}");
                return string.Empty;
            }
        }
    }
}