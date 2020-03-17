using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerController p = (PlayerController)target;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.richText = true;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Info", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Velocity/Update\t" + p.Velocity, style);
        EditorGUILayout.LabelField("Velocity/Second\t" + p.VelocityPerSecond, style);
        EditorGUILayout.LabelField("OnGround\t" + p.OnGround, style);
    }
}
