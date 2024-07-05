using System.Collections.Generic;
using UnityTools;
using UnityTools.Extend;
using UnityTools.UI;
using UnityEngine;

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
    public Transform _transform;
    public RectTransform _rectTransform;
    public RectTransform[] points;
    public void OnClickTest()
    {
        if (_transform == null) return;
        VirtualRocker rocker = _transform.GetComponent<VirtualRocker>();
        rocker.AddListener(Move);
        _transform = null;
    }
    void Move(Vector2 direction)
    {
        transform.position = transform.position + (Vector3)direction * Time.deltaTime;
    }
}