using System.Collections.Generic;
using UnityEngine;
using UnityTools;
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
    [ReadOnly]
    public Transform _transform;
    public RectTransform _rectTransform;
    public RectTransform[] points;
    public void OnClickTest()
    {
        RectTransform canvasRect = transform.parent as RectTransform;
        RectTransform rect = transform as RectTransform;
        Debuger.Log(rect.offsetMax.ToString());
        Debuger.Log(rect.offsetMin.ToString());
        if (points.Length >= 4)
        {
            points[0].anchoredPosition = new Vector2(rect.anchorMin.x * canvasRect.sizeDelta.x, rect.anchorMax.y * canvasRect.sizeDelta.y) +
                new Vector2(rect.offsetMin.x, rect.offsetMax.y);
            points[1].anchoredPosition = new Vector2(rect.anchorMin.x * canvasRect.sizeDelta.x, rect.anchorMin.y * canvasRect.sizeDelta.y) +
               new Vector2(rect.offsetMin.x, rect.offsetMin.y);
            points[2].anchoredPosition = new Vector2(rect.anchorMax.x * canvasRect.sizeDelta.x, rect.anchorMin.y * canvasRect.sizeDelta.y) +
                new Vector2(rect.offsetMax.x, rect.offsetMin.y);
            points[3].anchoredPosition = new Vector2(rect.anchorMax.x * canvasRect.sizeDelta.x, rect.anchorMax.y * canvasRect.sizeDelta.y) +
               new Vector2(rect.offsetMax.x, rect.offsetMax.y);
        }
    }
}