using UnityEngine;
using System.Collections;

using UnityEditor;

[CustomEditor(typeof(ObjectSpawnTrigger))]
public class ObjectSpawnTriggerEditor : BaseTriggerEditor
{
    [MenuItem("TRIGGER/Object Spawn")]
    static void CreateObjectSpawn()
    {
        GameObject newObject = new GameObject();
        newObject.name = "Object Spawn";

        newObject.AddComponent<ObjectSpawnTrigger>();
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
        EditorGUILayout.LabelField("Object Spawn");

        ObjectSpawnTrigger trigger = target as ObjectSpawnTrigger;

        trigger.PREFAB_OBJECT =
            EditorGUILayout.ObjectField(
                "SPAWN", trigger.PREFAB_OBJECT, typeof(GameObject), false) as GameObject;
    }
}
