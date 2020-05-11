using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimationEffect))]
public class AnimationEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effect"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("offset"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("origin"));

        SerializedProperty useParent = serializedObject.FindProperty("useCreatorAsParent");
        EditorGUILayout.PropertyField(useParent);

        if (!useParent.boolValue)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("parent"));

        serializedObject.ApplyModifiedProperties();
    }
}
