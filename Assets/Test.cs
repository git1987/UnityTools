using System.Collections.Generic;
using UnityEngine;
using UnityTools;
using UnityTools.Extend;
using UnityTools.UI;

public class Test : MonoBehaviour
{
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(Test))]
    class TestEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Test test = target as Test;
            if (GUILayout.Button("TestButton"))
            {
                test.OnClickTest();
            }
        }
    }
#endif
    [ReadOnly]
    public Transform _transform;
    public RectTransform _rectTransform;
    public RectTransform[] points;
    public void OnClickTest()
    {
        TestModel.instance.ToString();
        BaseModel.RemoveModel<TestModel>();
    }
}