using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Toggle))]
public class ToggleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enabledEvent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("disabledEvent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("stateChangedEvent"));
        serializedObject.ApplyModifiedProperties();

        ((Toggle)target).State = EditorGUILayout.Toggle("State", ((Toggle)target).State);
    }
}
