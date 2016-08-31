using UnityEngine;
using System.Collections;

using UnityEditor;

[CustomEditor(typeof(FollowCameraTrigger))]
public class FollowCameraTriggerEditor : BaseTriggerEditor
{
    [MenuItem("TRIGGER/Follow Camera")]
    static void CreateFollowCamera()
    {
        GameObject newObject = new GameObject();
        newObject.name = "Follow Camera";

        newObject.AddComponent<FollowCameraTrigger>();
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

        FollowCameraTrigger trigger = target as FollowCameraTrigger;

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("FOLLOW CAMERA");

        trigger.CAMERA =
            EditorGUILayout.ObjectField("CAMERA", trigger.CAMERA, typeof(Camera), true) as Camera;
    }


}
