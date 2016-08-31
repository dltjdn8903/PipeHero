using UnityEngine;
using System.Collections;

using UnityEditor;

[CustomEditor(typeof(MonsterSpawnTrigger))]
public class MonsterSpawnTriggerEditor : BaseTriggerEditor
{
    [MenuItem("TRIGGER/Monster Spawn")]
    static void CreateMonsterSpawn()
    {
        GameObject newObject = new GameObject();
        newObject.name = "Monster Spawn";

        newObject.AddComponent<MonsterSpawnTrigger>();
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
        EditorGUILayout.LabelField("Monster Spawn");

        MonsterSpawnTrigger trigger = target as MonsterSpawnTrigger;

        if( GUILayout.Button("Add") == true )
        {
            trigger.LIST_MONSTER.Add(null);
        }

        int nDeleteIndex = -1;
        for (int i = 0; i < trigger.LIST_MONSTER.Count; ++i)
        {
            EditorGUILayout.BeginHorizontal();

            trigger.LIST_MONSTER[i] =
                EditorGUILayout.ObjectField("Monster", 
                trigger.LIST_MONSTER[i], typeof(BaseObject), true) as BaseObject;

            if( GUILayout.Button("X") == true )
            {
                nDeleteIndex = i;
            }

            EditorGUILayout.EndHorizontal();
        }

        if( nDeleteIndex != -1 )
        {
            trigger.LIST_MONSTER.RemoveAt(nDeleteIndex);
        }
    }


}
