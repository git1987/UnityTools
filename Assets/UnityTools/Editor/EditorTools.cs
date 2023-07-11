using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
}