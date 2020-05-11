using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static EditorHelper;

[CustomEditor(typeof(PlayerController))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerController p = (PlayerController)target;
        GUIStyle style = RichLabel;

        Header("Debug Info");
        if (p.Rigidbody != null)
        {
            EditorGUILayout.LabelField("Velocity\t" + p.Rigidbody.velocity, style);
            EditorGUILayout.LabelField("OnGround\t" + p.OnGround, style);
            EditorGUILayout.LabelField("Holding Jump\t" + Input.GetKey(p.jumpKey), style);
            EditorGUILayout.LabelField("IsFalling\t" + (p.IsFalling), style);
        }
    }
}
