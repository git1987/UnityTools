using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityTools.Extend;

public class Test : MonoBehaviour
{
    [CustomEditor(typeof(Test))]
    class TestEditor : Editor
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

    void OnClickTest()
    {
        transform.RemoveChild();
    }
}