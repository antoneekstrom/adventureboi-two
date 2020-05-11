using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NpcMovement))]
public class NpcMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty settings = serializedObject.FindProperty("settings");
        EditorGUILayout.PropertyField(settings);

        SerializedProperty freeze = serializedObject.FindProperty("freeze");
        EditorGUILayout.PropertyField(freeze);

        serializedObject.ApplyModifiedProperties();
    }
}
