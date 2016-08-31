using UnityEngine;
using System.Collections;

using UnityEditor;

[CustomEditor(typeof(SelfSpawnTrigger))]
public class SelfSpawnTriggerEditor : BaseTriggerEditor
{
    [MenuItem("TRIGGER/Self Spawn")]
    static void CreateSelfSpawnTrigger()
    {
        GameObject newObject = new GameObject();
        newObject.name = "Self Spawn";

        newObject.AddComponent<SelfSpawnTrigger>();
        SphereCollider collider = newObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;

        Camera currentCamera = Camera.current;

        RaycastHit hit;
        if( Physics.Raycast(currentCamera.transform.position, currentCamera.transform.forward, out hit) == true )
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
        EditorGUILayout.LabelField("Self Spawn");

        SelfSpawnTrigger trigger = target as SelfSpawnTrigger;

        trigger.SELF_CHARACTER = 
            EditorGUILayout.ObjectField("CHARACTER", 
            trigger.SELF_CHARACTER, typeof(BaseObject), true) as BaseObject;
    }


}
