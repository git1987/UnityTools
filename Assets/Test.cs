using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityTools.Extend;

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

    public void OnClickTest()
    {
        UnityTools.Debuger.Log("Test.OnClickTest()");
    }
}