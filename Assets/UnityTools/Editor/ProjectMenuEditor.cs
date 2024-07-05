using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
namespace UnityTools.Editor
{
    /// <summary>
    /// Project面板右键菜单功能
    /// </summary>
    public class ProjectMenuEditor
    {
        [MenuItem("Assets/UnityTools/CreateBasePanelClass")]
        static void BasePanelCSharp()
        {
            Object folder = Selection.activeObject;
            if (folder != null && folder is DefaultAsset)
            {
                //判断是文件夹
                string path     = AssetDatabase.GetAssetPath(folder);
                string filePath = EditorTools.UnityAssetPathToFilePath(path);
                Debug.Log(filePath + "中创建panel和model子类脚本");
                CreateCSharpFile(filePath, folder.name);
            }
            AssetDatabase.Refresh();
        }
        private static void CreateCSharpFile(string folderPath, string fileName)
        {
            //panel
            string panelPath = folderPath + $"/{fileName}Panel.cs";
            if (File.Exists(panelPath))
            {
                Debug.LogError(new FileInfo(panelPath).Name + "已存在",
                               AssetDatabase.LoadAssetAtPath<Object>(EditorTools.FilePathToUnityAssetPath(panelPath)));
            }
            else
            {
                StringBuilder script = new();
                script.AppendLine("using UnityEngine;");
                script.AppendLine("using UnityTools.UI;");
                script.AppendLine("public class " + fileName + "Panel : BasePanel");
                script.AppendLine("{");
                script.AppendLine("");
                script.AppendLine("}");
                FileStream fs = new FileStream(panelPath, System.IO.FileMode.Create);
                //创建一个写入流，写入文件为fs
                StreamWriter sw  = new StreamWriter(fs);
                string       str = script.ToString();
                sw.Write(str);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                //关闭文件
                fs.Close();
                AssetDatabase.Refresh();
                Debug.Log($"{fileName}Panel.cs创建成功",
                          AssetDatabase.LoadAssetAtPath<MonoScript>(EditorTools.FilePathToUnityAssetPath(panelPath)));
            }
            //model
            string modelPath = folderPath + $"/{fileName}Model.cs";
            if (File.Exists(modelPath))
            {
                Debug.LogError(new FileInfo(modelPath).Name + "已存在",
                               AssetDatabase.LoadAssetAtPath<Object>(EditorTools.FilePathToUnityAssetPath(modelPath)));
            }
            else
            {
                StringBuilder script = new();
                script.AppendLine("using UnityEngine;");
                script.AppendLine("using UnityTools.UI;");
                script.AppendLine("public class " + fileName + "Model : BaseModel");
                script.AppendLine("{");
                script.AppendLine("\tprivate static " + fileName + "Model _instance;");
                //单例
                script.AppendLine("\tpublic static " + fileName + "Model instance");
                script.AppendLine("\t{");
                script.AppendLine("\t\tget");
                script.AppendLine("\t\t{");
                script.AppendLine("\t\t\tif (_instance == null) _instance = new();");
                script.AppendLine("\t\t\treturn _instance;");
                script.AppendLine("\t\t}");
                script.AppendLine("\t}");
                //Disable
                script.AppendLine("\tprotected override void Disable()");
                script.AppendLine("\t{");
                script.AppendLine("\t\t_instance = null;");
                script.AppendLine("\t}");
                script.AppendLine("}");
                FileStream fs = new FileStream(modelPath, System.IO.FileMode.Create);
                //创建一个写入流，写入文件为fs
                StreamWriter sw  = new StreamWriter(fs);
                string       str = script.ToString();
                sw.Write(str);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                //关闭文件
                fs.Close();
                AssetDatabase.Refresh();
                Debug.Log($"{fileName}Model.cs创建成功",
                          AssetDatabase.LoadAssetAtPath<MonoScript>(EditorTools.FilePathToUnityAssetPath(modelPath)));
            }
        }
    }
}