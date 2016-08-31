using UnityEngine;
using System.Collections;

public class WeatherChanger : MonoBehaviour {
	
	public GameObject effect1, effect2, effect3, effect4;

	void OnGUI() {
		GUILayout.Space(20.0f);
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Rain")) {
			effect2.active = false;	
			effect3.active = false;
			
			foreach (Transform child in effect4.transform) {
				child.gameObject.SetActive(false);
			}
			
			effect1.active = true;
		}
		
		if(GUILayout.Button("Snow")) {
			effect1.active = false;	
			effect3.active = false;
			
			foreach (Transform child in effect4.transform) {
				child.gameObject.SetActive(false);
			}
			
			effect2.active = true;
		}
		
		if(GUILayout.Button("Sand Storm")) {
			effect1.active = false;	
			effect2.active = false;
			
			foreach (Transform child in effect4.transform) {
                child.gameObject.SetActive(false);
			}
			
			effect3.active = true;
		}
		
		if(GUILayout.Button("Polar Lights")) {
			effect1.active = false;	
			effect2.active = false;
			effect3.active = false;
			
			foreach (Transform child in effect4.transform) {
				child.gameObject.SetActive(true);
			}
		}
		GUILayout.EndHorizontal();
	}
}
