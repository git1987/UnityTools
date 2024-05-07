using UnityEngine;

namespace UnityTools
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomPropertyDrawer(typeof(LabelNameAttribute))]
    public class LabelNameDrawer : PropertyDrawer
    {
        private GUIContent guiContent;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = (attribute as LabelNameAttribute).editor;
            if (guiContent == null)
            {
                string name = (attribute as LabelNameAttribute).name;
                guiContent = new GUIContent(name);
            }
            EditorGUI.PropertyField(position, property, guiContent, true);
            GUI.enabled = true;
        }
    }
#endif
    /// <summary>
    /// 设定变量在Inspector中显示的名称
    /// </summary>
    public class LabelNameAttribute : PropertyAttribute
    {
        public string name;
        public bool editor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">在Inspector中显示的Label</param>
        /// <param name="editor">在Inspector中是否可编辑</param>
        public LabelNameAttribute(string name, bool editor = true)
        {
            this.name   = name;
            this.editor = editor;
        }
    }
}