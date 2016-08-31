using UnityEngine;
using System.Collections;

using UnityEditor;

[CustomEditor(typeof(AreaTrigger))]
public class AreaTriggerEditor : BaseTriggerEditor
{
    [MenuItem("TRIGGER/Area Trigger")]
    static void CreateMonsterSpawn()
    {
        GameObject newObject = new GameObject();
        newObject.name = "Area Trigger";

        newObject.AddComponent<AreaTrigger> ();
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
        EditorGUILayout.LabelField("Area Trigger");

        AreaTrigger trigger = target as AreaTrigger;

        trigger.AREA_TYPE = (eAreaType)EditorGUILayout.EnumPopup("AREA_TYPE", trigger.AREA_TYPE);
        trigger.AUDIO_CLIP =
            EditorGUILayout.ObjectField("AudioClip", 
            trigger.AUDIO_CLIP, typeof(AudioClip), true) as AudioClip;


    }


}
