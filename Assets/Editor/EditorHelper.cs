using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorHelper
{
    public static GUIStyle RichLabel => GetDefaultLabelStyle();
    public static GUIStyle Bold => EditorStyles.boldLabel;

    public static void Label(string text)
    {
        EditorGUILayout.LabelField(text, RichLabel);
    }

    public static bool Button(string text)
    {
        return GUILayout.Button(text);
    }

    public static void Header(string text)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(text, Bold);
    }

    private static GUIStyle GetDefaultLabelStyle()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.richText = true;
        return style;
    }

}