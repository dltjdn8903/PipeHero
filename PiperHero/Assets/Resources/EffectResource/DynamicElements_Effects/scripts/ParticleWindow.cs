//using UnityEngine;
//using UnityEditor;
//using System.Collections;

//public class ParticleWindow : EditorWindow 
//{
//    float mScale = 1.0f;

//    [@MenuItem("Custom/particle scale")]

//    public static void Init()
//    {
//        EditorWindow ParticleWindow = null;
//        ParticleWindow = EditorWindow.GetWindow(typeof(ParticleWindow));
//    }

//    void OnGUI()
//    {
//        Object[] go = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);

//        string name = string.Empty;

//        if (go.Length > 0)
//            name = go[0].name;
//        else
//            name = "select go";

//        GUILayout.Label("Object name : " + name, EditorStyles.boldLabel);

//        mScale = EditorGUILayout.Slider("scale : ", mScale, 0.01f, 5.0f);

//        if (GUI.Button(new Rect(50, 50, 100, 40), "set value"))
//        {
//            bool ok = false;

//            foreach (Transform child in go)
//            {
//                if (child.gameObject.GetComponent<ParticleEmitter> ())
//                {
//                    child.gameObject.GetComponent< ParticleEmitter > ().minSize *= mScale;
//                    child.gameObject.GetComponent< ParticleEmitter > ().maxSize *= mScale;
//                    child.gameObject.GetComponent< ParticleEmitter > ().worldVelocity *= mScale;
//                    child.gameObject.GetComponent< ParticleEmitter > ().localVelocity *= mScale;
//                    child.gameObject.GetComponent< ParticleEmitter > ().rndVelocity *= mScale;
//                    child.gameObject.GetComponent< ParticleEmitter > ().angularVelocity *= mScale;
//                    child.gameObject.GetComponent< ParticleEmitter > ().rndAngularVelocity *= mScale;

//                    ok = true;
//                }
//            }

//            if (ok)
//                ((Transform)go[0]).transform.localScale *= mScale;

//            if (ok)
//                Debug.Log("ok!");
//            else
//                Debug.Log("something is wrong!");



//        }
//    }

//}


