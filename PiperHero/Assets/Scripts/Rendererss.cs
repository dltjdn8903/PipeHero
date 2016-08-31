using UnityEngine;
using System.Collections;

public class Rendererss : MonoBehaviour {

    Renderer renderer = null;
    Material m_MyMaterial = null;


    Shader m_MyShader = null;

	// Use this for initialization
	void Awake ()
    {
        renderer = gameObject.GetComponent<Renderer>();

        m_MyMaterial = renderer.material;

        m_MyMaterial.color = new Color(1, 0, 0, 0);

        Debug.Log(renderer);
        Debug.Log(m_MyMaterial);

        //renderer.material.SetColor(0,);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
