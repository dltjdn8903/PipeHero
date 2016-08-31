using UnityEngine;
using System.Collections;

using UnityEditor;

[CustomEditor(typeof(BaseTrigger), true)]
public class BaseTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BaseTrigger trigger = target as BaseTrigger;

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Base Trigger");

        trigger.INVOKE_TYPE = (eTriggerInvokeType)EditorGUILayout.EnumPopup("INVOKE_TYPE", trigger.INVOKE_TYPE);
        trigger.INVOKE_TIME = EditorGUILayout.FloatField("INVOKE_TIME", trigger.INVOKE_TIME);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Call Trigger List");

        if( GUILayout.Button("Add") == true )
        {
            trigger.LIST_CALL_TRIGGER.Add(null);
        }

        int nDeleteIndex = -1;
        for( int i = 0; i < trigger.LIST_CALL_TRIGGER.Count; ++i )
        {
            BaseTrigger callTrigger = trigger.LIST_CALL_TRIGGER[i];

            EditorGUILayout.BeginHorizontal();

            trigger.LIST_CALL_TRIGGER[i] =
                EditorGUILayout.ObjectField(callTrigger, typeof(BaseTrigger), true) as BaseTrigger;

            if( GUILayout.Button("X") == true )
                nDeleteIndex = i;

            EditorGUILayout.EndHorizontal();
        }

        if( nDeleteIndex != -1 )
        {
            trigger.LIST_CALL_TRIGGER.RemoveAt(nDeleteIndex);
        }

    }
}
