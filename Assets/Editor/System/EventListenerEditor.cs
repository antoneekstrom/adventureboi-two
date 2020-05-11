using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventListener))]
public class EventListenerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty actionName = serializedObject.FindProperty("actionName");
        SerializedProperty onActionEvent = serializedObject.FindProperty("_onAction");
        SerializedProperty triggerOnlyOnSelf = serializedObject.FindProperty("triggerOnlyOnSelf");

        EventListener el = (EventListener)target;
        el.EventTrigger = (EventHandle)EditorGUILayout.ObjectField("Event Handle", el.EventTrigger, typeof(EventHandle), true);

        EditorGUILayout.PropertyField(actionName);
        EditorGUILayout.PropertyField(triggerOnlyOnSelf);
        EditorGUILayout.PropertyField(onActionEvent);

        serializedObject.ApplyModifiedProperties();
    }
}
