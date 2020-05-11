using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StringConstant))]
public class StringConstantEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(((StringConstant)target).Value);
    }
}
