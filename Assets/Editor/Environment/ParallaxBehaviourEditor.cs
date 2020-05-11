using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static EditorHelper;

[CustomEditor(typeof(ParallaxBehaviour)), CanEditMultipleObjects]
public class ParallaxBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ParallaxBehaviour p = (ParallaxBehaviour)target;

        Header("Target");

        SerializedProperty useCamera = serializedObject.FindProperty("useCamera");
        SerializedProperty depth = serializedObject.FindProperty("depth");

        useCamera.boolValue = GUILayout.Toggle(useCamera.boolValue, "Use Camera");
        if (!useCamera.boolValue)
        {
            p.parent = (Transform)EditorGUILayout.ObjectField("Parent", p.parent, typeof(Transform), true);
        }

        Header("Properties");
        ParallaxBehaviour.baseZ = EditorGUILayout.FloatField("Base Z", ParallaxBehaviour.baseZ);
        depth.floatValue = EditorGUILayout.Slider("Depth", depth.floatValue, 0f, 1f);

        serializedObject.ApplyModifiedProperties();
    }
}
