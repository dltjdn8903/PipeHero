using UnityEngine;
using System.Collections;

using UnityEditor;

[CustomEditor(typeof(GameTrigger))]
public class GameTriggerEditor : BaseTriggerEditor
{
    [MenuItem("TRIGGER/Game Trigger")]
    static void CreateGameTrigger()
    {
        GameObject newObject = new GameObject();
        newObject.name = "Game Trigger";

        newObject.AddComponent<GameTrigger>();
        SphereCollider collider = newObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;

        Camera currentCamera = Camera.current;

        RaycastHit hit;
        if (Physics.Raycast(currentCamera.transform.position, currentCamera.transform.forward, out hit) == true)
        {
            newObject.transform.position = hit.point;
        }
        else
        {
            newObject.transform.position = currentCamera.transform.position + (currentCamera.transform.forward * 5);
        }


        newObject.transform.SetParent(Selection.activeTransform, true);
    }



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Game Trigger");

        GameTrigger trigger = target as GameTrigger;

        _ClearInspector(trigger);
        _FailInspector(trigger);


        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("CLEAR CALL TRIGGER");
        trigger.CLEAR_CALL_TRIGGER =
            EditorGUILayout.ObjectField(
                trigger.CLEAR_CALL_TRIGGER, typeof(BaseTrigger), true) as BaseTrigger;

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("FAIL CALL TRIGGER");
        trigger.FAIL_CALL_TRIGGER =
            EditorGUILayout.ObjectField(
                trigger.FAIL_CALL_TRIGGER, typeof(BaseTrigger), true) as BaseTrigger;
    }


    void _ClearInspector(GameTrigger trigger )
    {
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("CLEAR CONDITION");
        if (GUILayout.Button("Add") == true)
        {
            trigger.LIST_CLEAR_CONDITION.Add(new stGameCondition());
        }

        int nDeleteIndex = -1;
        for (int i = 0; i < trigger.LIST_CLEAR_CONDITION.Count; ++i)
        {
            EditorGUILayout.BeginHorizontal();

            stGameCondition condition = trigger.LIST_CLEAR_CONDITION[i];

            
            int nCount = eGameCondition.CLEAR_END - eGameCondition.CLEAR_START - 1;

            int[] arrValue = new int[nCount];
            string[] arrString = new string[nCount];

            int nIndex = (int)condition.m_ConditionType;

            for (int k = 0; k < nCount; ++k)
            {
                arrValue[k] = (int)eGameCondition.CLEAR_START + k + 1;
                arrString[k] = ((eGameCondition)arrValue[k]).ToString("F");
            }

            int nNewIndex = EditorGUILayout.IntPopup(nIndex, arrString, arrValue);
            BaseTrigger newTrigger =
                EditorGUILayout.ObjectField(
                    condition.m_TargetTrigger, typeof(BaseTrigger), true) as BaseTrigger;

            if (nNewIndex != nIndex || newTrigger != condition.m_TargetTrigger)
            {
                condition.m_ConditionType = (eGameCondition)nNewIndex;
                condition.m_TargetTrigger = newTrigger;
                trigger.LIST_CLEAR_CONDITION[i] = condition;
            }

            if (GUILayout.Button("X") == true)
            {
                nDeleteIndex = i;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (nDeleteIndex != -1)
        {
            trigger.LIST_CLEAR_CONDITION.RemoveAt(nDeleteIndex);
        }
    }

    void _FailInspector(GameTrigger trigger )
    {
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("FAIL CONDITION");
        if (GUILayout.Button("Add") == true)
        {
            trigger.LIST_FAIL_CONDITION.Add(new stGameCondition());
        }

        int nDeleteIndex = -1;
        for (int i = 0; i < trigger.LIST_FAIL_CONDITION.Count; ++i)
        {
            EditorGUILayout.BeginHorizontal();

            stGameCondition condition = trigger.LIST_FAIL_CONDITION[i];

            int nIndex = (int)condition.m_ConditionType;
            int nCount = eGameCondition.FAIL_END - eGameCondition.FAIL_START - 1;

            int[] arrValue = new int[nCount];
            string[] arrString = new string[nCount];

            for (int k = 0; k < nCount; ++k)
            {
                arrValue[k] = (int)eGameCondition.FAIL_START + k + 1;
                arrString[k] = ((eGameCondition)arrValue[k]).ToString("F");
            }

            int nNewIndex = EditorGUILayout.IntPopup(nIndex, arrString, arrValue);
            BaseTrigger newTrigger =
                EditorGUILayout.ObjectField(
                    condition.m_TargetTrigger, typeof(BaseTrigger), true) as BaseTrigger;

            if (nNewIndex != nIndex || newTrigger != condition.m_TargetTrigger)
            {
                condition.m_ConditionType = (eGameCondition)nNewIndex;
                condition.m_TargetTrigger = newTrigger;
                trigger.LIST_FAIL_CONDITION[i] = condition;
            }

            if (GUILayout.Button("X") == true)
            {
                nDeleteIndex = i;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (nDeleteIndex != -1)
        {
            trigger.LIST_FAIL_CONDITION.RemoveAt(nDeleteIndex);
        }
    }
}
