using UnityEditor;
using UnityEngine;
using System.Collections;

public enum GUIState {//application states
	
	CREATBIRDS = 0,	//Creatures Effects - Birds
	CREATBUTTER,	//Creatures Effects - Butterflies
	CREATFLIES,		//Creatures Effects - Flies
	CREATEANTS,		//Creatures Effects - Ants
	
	TELEPORT,		//Teleport Effect
	
	WEATHERRAIN,	//Weather Effects - Rain
	WEATHERSNOW,	//Weather Effects - Snow
	WEATHERSANDS,	//Weather Effects - Sand Shtorm
	WEATHERPLIGHTS, //Weather Effects - Polar Lights
	
	CAMWISPS,		//Camera Effects - Wisps
	CAMLEAVES,		//Camera Effects - Leaves
	CAMDUSTS,		//Camera Effects - Dusts
	
	MAGBALLS,		//Magic Effects - Balls
	MAGWALLS,		//Magic Effects - Walls
	MAGAURA,		//Magic Effects - Aura
	MAGGATES,		//Magic Effects - Gates
	MAGSOURCES,		//Magic Effects - Sources
	
	CURSOR,
	
	OTHERWF,		//Other - Waterfall
	OTHERIVER,		//Other - River
	OTHERSWIRL		//Other - Swirls
}

public class MainTool : EditorWindow {

	private Texture2D logo = Resources.Load("logo") as Texture2D;
	private float version = 0.5f;
	private GameObject root;
	private int select_effect = -1;
	private int select_sub_effect = -1;
	
	private string[] options = {"Creatures Effects", "Teleports Effects", "Weather Effects", "Camera Effects", "Cursor Effects", "Magic Effects"/*, "Other"*/};
	private string[] creatures_options = {"Birds", "Butterflies", "Flies"/*, "Ants"*/};
	private string[] weather_options = {"Rain", "Snow", "Sand Storm", "Polar Lights"};
	private string[] camera_options = {"Wisps", "Leaves"/*, "Dusts"*/};
	private string[] magic_options = {"Balls"/*, "Walls", "Aura", "Gates", "Sources"*/};
	private string[] other_options = {/*"Waterfall", "River", "Swirls", "Other"*/};
	
	private GUIState gui_state;
	private Object[] all_textures;
	//-------------------------------------------
	/*********Creatures********/
	private GameObject butterfly_prefab;
	private GameObject butterfly;
	
	private GameObject birds_prefab;
	private GameObject birds;
	
	private GameObject flies_prefab;
	private GameObject flies;
	
	/*********Weather********/
	private GameObject rain_prefab;
	private GameObject rain;
	
	private GameObject snow_prefab;
	private GameObject snow;
	
	private GameObject sand_storm_prefab;
	private GameObject sand_storm;
	
	private GameObject polar_lights_prefab;
	private GameObject polar_lights;
	
	/*********Camera********/
	private GameObject wisp_prefab;
	private GameObject wisp;
	
	private GameObject leaves_prefab;
	private GameObject leaves;
	
	private GameObject dust_prefab;
	private GameObject dust;
	
	/********Magic Effects*********/
	private GameObject mball_prefab;
	private GameObject mball;
	
	/***********Other Effects**********/
	private GameObject swirl_prefab;
	private GameObject swirl;
	
	private GameObject cursoref_prefab;
	private GameObject cursoref;
	//--------------------------------------------
	/*********Creatures********/
	int select_bird_type = 0;
	int old_sel_bird_type = 0;
	float select_bird_size = 0.1f;
	float old_sel_bird_size = 0.1f;
	float select_bird_speed = 5.0f;
	float old_sel_bird_speed = 5.0f;
	int select_bird_dir = 0;
	int old_sel_bird_dir = 0;
	Texture custom_bird_tex = null;
	
	int select_but_type = 0;
	int old_sel_but_type = 0;	
	int select_but_count = 3;
	int old_sel_but_count = 3;
	int select_but_dir = 0;
	int old_sel_but_dir = 0;
	Texture custom_but_tex = null;
	
	int select_fly_type = 0;
	int old_sel_fly_type = 0;
	int select_fly_dir = 0;
	int old_sel_fly_dir = 0;
	Texture custom_fly_tex = null;
	/*********Teleports*******/   
	//int select_tel_type = 0;
	//int old_sel_tel_type = 0;
	/**********Weather Effects*********/
	float select_rain_intensity = 5.0f;
	float old_sel_rain_intensity = 5.0f;
	
	float select_snow_intensity = 5.0f;
	float old_sel_snow_intensity = 5.0f;
	
	//float select_sands_intensity = 5.0f;
	//float old_sel_sands_intensity = 5.0f;
	/**********Cursor Effects*********/	
	int select_cursoref_type = 0;
	int old_sel_cursoref_type = 0;
	GameObject custom_cursoref_go = null;
	/********Magic Effects*******/
	int select_mball_type = 0;
	int old_sel_mball_type = 0;
	Texture custom_mball_tex = null;
	//--------------------------------------------
	
	[MenuItem("Tools/AnBytes/APE Editor")]
	
	public static void CreateWindow()
    {
		string[] versp = Application.unityVersion.Split('.');
		string[] versp2 = versp[1].Split('.');
		string vers = versp[0] + "." + versp2[0];
		
		if (float.Parse(vers) < 3.4f) {
			Debug.LogError("For using this package, you need install unity3d 3.4 or above");
		}
		else {
        	MainTool window = GetWindow<MainTool>();
        	window.title = "APE Editor";
		}
    }
	
	void OnGUI() {
		GUI.DrawTexture(new Rect((Screen.width - 300) / 2, 10, 300, 100), logo);
		GUILayout.Space(110.0f);
		GUILayout.Label("Tool version " + version.ToString());
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(10.0f);
		GUILayout.Label("Select category:");
		select_effect = EditorGUILayout.Popup(select_effect, options);
		EditorGUILayout.EndHorizontal();
		
		if (select_effect != -1) {			
			GUILayout.Space(10.0f);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10.0f);
			GUILayout.Label("Select effect:");
			switch (select_effect) {
				
				case 0:
					select_sub_effect = EditorGUILayout.Popup(select_sub_effect, creatures_options);
					break;
				
				case 1:
					GUILayout.Label("No subspecies");
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("Attention: For using this effect, you will need add tag \"TeleportObj\" in you project.");
					break;
				
				case 2:
					select_sub_effect = EditorGUILayout.Popup(select_sub_effect, weather_options);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("Please select GameObject in the Hierarchy");
					break;
				
				case 3:					
					select_sub_effect = EditorGUILayout.Popup(select_sub_effect, camera_options);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("Please select camera");
					break;
				
				case 4:
					GUILayout.Label("No subspecies");
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label("Please select camera\nFor see how work this effect, you need run play mode.");
					break;
				
				case 5:
					select_sub_effect = EditorGUILayout.Popup(select_sub_effect, magic_options);
					break;
				
				case 6:
					select_sub_effect = EditorGUILayout.Popup(select_sub_effect, other_options);
					break;
				
				default: break;
			}
			EditorGUILayout.EndHorizontal();
		}
		
		GUILayout.Space(10.0f);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Effect Object")) {
			APEComponent tmpc;
			GameObject tmp;
			switch (select_effect) {
				
				case 0:
					root = new GameObject("APE_Creatures_Effect");
					switch (select_sub_effect) {
						case 0:
							root.AddComponent<APEBirds>();
					
							birds_prefab = Resources.Load("Birds/Prefabs/Birds") as GameObject;
							birds = (GameObject)Instantiate(birds_prefab);
							birds.transform.position = root.transform.position;
							birds.transform.parent = root.transform;
							birds.name = "Birds";
							all_textures = Resources.LoadAll("Birds/Textures", typeof(Texture));
					
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.CREATBIRDS;
							break;
						case 1:
							root.AddComponent<APEButterflies>();
					
							butterfly_prefab = Resources.Load("Butterflies/Prefabs/butterfly1") as GameObject;
							butterfly = (GameObject)Instantiate(butterfly_prefab);
							butterfly.transform.position = root.transform.position;
							butterfly.transform.parent = root.transform;
							butterfly.name = "Butterflies";
							all_textures = Resources.LoadAll("Butterflies/Textures", typeof(Texture));
					
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.CREATBUTTER;
							break;
						case 2:
							root.AddComponent<APEFlies>();
					
							flies_prefab = Resources.Load("Flies/Prefabs/fly") as GameObject;
							flies = (GameObject)Instantiate(flies_prefab);
							flies.transform.position = root.transform.position;
							flies.transform.parent = root.transform;
							flies.name = "Flies";
							all_textures = Resources.LoadAll("Flies/Textures", typeof(Texture));
					
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.CREATFLIES;
							break;
						case 3: 
							root.AddComponent<APEAnts>(); 
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.CREATEANTS;
							break;
					}
					break;
				
				case 1:
					root = new GameObject("APE_Teleport_Effect");
					root.AddComponent<APETeleportRoot>();
				
					root.AddComponent<APEComponent>();
					tmpc = root.GetComponent("APEComponent") as APEComponent;
					tmpc.ID = (int)GUIState.TELEPORT;
					break;
				
				case 2:
					if (Selection.activeObject == null) {
						Debug.LogError("APE: You need select GameObject!");
						break;
					}
					tmp = Selection.activeObject as GameObject;
					switch (select_sub_effect) {
						case 0:	
							rain_prefab = Resources.Load("Rain/Prefabs/Rain") as GameObject;
							rain = (GameObject)Instantiate(rain_prefab);
							rain.AddComponent<APERain>();
							rain.transform.position = tmp.transform.position;
							rain.transform.parent = tmp.transform;
							rain.transform.localPosition = new Vector3(0.0f, 8.0f, 4.0f);
							rain.name = "APE_Rain";
					
							rain.AddComponent<APEComponent>();
							tmpc = rain.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.WEATHERRAIN;
							break;
						case 1: 					
							snow_prefab = Resources.Load("Snow/Prefabs/Snow") as GameObject;
							snow = (GameObject)Instantiate(snow_prefab);
							snow.AddComponent<APESnow>();
							snow.transform.position = tmp.transform.position;
							snow.transform.parent = tmp.transform;
							snow.transform.localPosition = new Vector3(0.0f, 1.8f, 4.0f);
							snow.name = "APE_Snow";
							
							snow.AddComponent<APEComponent>();
							tmpc = snow.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.WEATHERSNOW;
							break;
						case 2: 				
							sand_storm_prefab = Resources.Load("SandStorm/Prefabs/SandStorm") as GameObject;
							sand_storm = (GameObject)Instantiate(sand_storm_prefab);
							sand_storm.AddComponent<APESandShtorm>();
							sand_storm.transform.position = tmp.transform.position;
							sand_storm.transform.parent = tmp.transform;
							sand_storm.transform.localPosition = new Vector3(0.0f, -1.4f, 4.0f);
							sand_storm.name = "APE_Sand_Storm";
					
							sand_storm.AddComponent<APEComponent>();
							tmpc = sand_storm.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.WEATHERSANDS;
							break;
						case 3: 					
							polar_lights_prefab = Resources.Load("PolarLights/Prefabs/PolarLights") as GameObject;
							polar_lights = (GameObject)Instantiate(polar_lights_prefab);
							polar_lights.AddComponent<APEPolarLights>();
							polar_lights.transform.position = tmp.transform.position;
							polar_lights.transform.parent = tmp.transform;
							polar_lights.transform.localPosition = new Vector3(3.5f, 2.1f, 35.0f);
							polar_lights.name = "APE_Polar_Lights";
					
							polar_lights.AddComponent<APEComponent>();
							tmpc = polar_lights.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.WEATHERPLIGHTS;
							break;
					}
					break;
				
				case 3:
					if (Selection.activeObject == null) {
						Debug.LogError("APE: You need select camera!");
						break;
					}
					tmp = Selection.activeObject as GameObject;
					switch (select_sub_effect) {
						case 0: 							
							wisp_prefab = Resources.Load("Magic_forest/Prefabs/Magic_Wisp") as GameObject;
							wisp = (GameObject)Instantiate(wisp_prefab);
							wisp.AddComponent<APEWisps>();
							wisp.transform.position = tmp.transform.position;
							wisp.transform.parent = tmp.transform;
							wisp.transform.localPosition = new Vector3(0.4f, -1.0f, 5.0f);
							wisp.name = "APE_Wisp";
					
							wisp.AddComponent<APEComponent>();
							tmpc = wisp.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.CAMWISPS;				
							break;
						case 1:							
							leaves_prefab = Resources.Load("Leaves/Prefabs/Leaves") as GameObject;
							leaves = (GameObject)Instantiate(leaves_prefab);
							leaves.AddComponent<APELeaves>();
							leaves.transform.position = tmp.transform.position;
							leaves.transform.parent = tmp.transform;
							leaves.transform.localPosition = new Vector3(0.0f, 2.0f, 6.0f);
							leaves.name = "APE_Leaves";
					
							leaves.AddComponent<APEComponent>();
							tmpc = leaves.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.CAMLEAVES;
							break;
						case 2: tmp.AddComponent<APEDusts>(); break;
					}
					break;
				
				case 4:
					if (Selection.activeObject == null) {
						Debug.LogError("APE: You need select camera!");
						break;
					}
					tmp = Selection.activeObject as GameObject;
					cursoref_prefab = Resources.Load("CursorEffects/Prefabs/Smoke") as GameObject;
					cursoref = (GameObject)Instantiate(cursoref_prefab);
					cursoref.AddComponent<APECursor>();
					cursoref.transform.position = tmp.transform.position;
					cursoref.transform.parent = tmp.transform;
					cursoref.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
					cursoref.name = "APE_Cursor_Effect";
					
					cursoref.AddComponent<APEComponent>();
					tmpc = cursoref.GetComponent("APEComponent") as APEComponent;
					tmpc.ID = (int)GUIState.CURSOR;
				
					all_textures = Resources.LoadAll("CursorEffects/Prefabs", typeof(GameObject));
					break;
				
				case 5:
					root = new GameObject("APE_Magic_Effect");
					switch (select_sub_effect) {
						case 0:
							root.AddComponent<APEBalls>();
					
							mball_prefab = Resources.Load("MagicBalls/Prefabs/FireBall") as GameObject;
							mball = (GameObject)Instantiate(mball_prefab);
							mball.transform.position = root.transform.position;
							mball.transform.parent = root.transform;
							mball.name = "Magic_Ball";
							all_textures = Resources.LoadAll("MagicBalls/Textures", typeof(Texture));
					
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.MAGBALLS;
							break;
						case 1: 
							root.AddComponent<APEWalls>(); 
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.MAGWALLS;
							break;
						case 2:
							root.AddComponent<APEAura>();
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.MAGAURA;
							break;
						case 3: 
							root.AddComponent<APEGates>(); 
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.MAGGATES;
							break;
						case 4:
							root.AddComponent<APESources>();
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.MAGSOURCES;
							break;
					}
					break;
				
				case 6:
					root = new GameObject("APE_Other_Effect");
					switch (select_sub_effect) {
						/*case 0: root.AddComponent("APEWaterfall"); break;
						case 1: root.AddComponent("APERiver"); break;*/
						case 0:
							root.AddComponent<APESwirls>();
					
							swirl_prefab = Resources.Load("Swirl/Prefabs/Swirl") as GameObject;
							swirl = (GameObject)Instantiate(swirl_prefab);

							swirl.transform.position = root.transform.position;
							swirl.transform.parent = root.transform;
							swirl.name = "Swirl";
					
							root.AddComponent<APEComponent>();
							tmpc = root.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.OTHERSWIRL;
							break;
					}
					break;
				
				default: break;
			}
			//root.AddComponent("APEffect");
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		if (Selection.activeObject != null) {
			string[] sel_obj_in_ed = Selection.activeObject.name.Split('_');
			if (sel_obj_in_ed.Length > 0) {
				if (sel_obj_in_ed[0] == "APE") {
					GameObject tmp = Selection.activeObject as GameObject;
					APEComponent cur_comp = tmp.GetComponent("APEComponent") as APEComponent;
					switch (cur_comp.ID) {
						case (int)GUIState.CREATBIRDS:
							birds = tmp.transform.FindChild("Birds").gameObject;
							all_textures = Resources.LoadAll("Birds/Textures", typeof(Texture));
							gui_state = GUIState.CREATBIRDS;
							break;
						
						case (int)GUIState.CREATBUTTER:
							butterfly = tmp.transform.FindChild("Butterflies").gameObject;
							all_textures = Resources.LoadAll("Butterflies/Textures", typeof(Texture));
							gui_state = GUIState.CREATBUTTER;
							break;
						
						case (int)GUIState.CREATFLIES:
							flies = tmp.transform.FindChild("Flies").gameObject;
							all_textures = Resources.LoadAll("Flies/Textures", typeof(Texture));
							gui_state = GUIState.CREATFLIES;
							break;
						
						case (int)GUIState.TELEPORT:
							gui_state = GUIState.TELEPORT;
							break;
						
						case (int)GUIState.WEATHERRAIN:
							gui_state = GUIState.WEATHERRAIN;
							rain = tmp;
							break;
						
						case (int)GUIState.WEATHERSNOW:
							gui_state = GUIState.WEATHERSNOW;
							snow = tmp;
							break;
						
						case (int)GUIState.WEATHERSANDS:
							gui_state = GUIState.WEATHERSANDS;
							sand_storm = tmp;
							break;
						
						case (int)GUIState.WEATHERPLIGHTS:
							gui_state = GUIState.WEATHERPLIGHTS;
							break;
						
						case (int)GUIState.CAMWISPS:
							gui_state = GUIState.CAMWISPS;
							break;
						
						case (int)GUIState.CAMLEAVES:
							gui_state = GUIState.CAMLEAVES;
							break;
						
						case (int)GUIState.CURSOR:
							gui_state = GUIState.CURSOR;
							cursoref = tmp;
							all_textures = Resources.LoadAll("CursorEffects/Prefabs", typeof(GameObject));
							break;
						
						case (int)GUIState.MAGBALLS:
							mball = tmp.transform.FindChild("Magic_Ball").gameObject;
							all_textures = Resources.LoadAll("MagicBalls/Textures", typeof(Texture));
							gui_state = GUIState.MAGBALLS;
							break;
					
						default: break;
					}
					Configuration();
				}
				else {
					ShowMessSel();
				}
			}
			else {
				ShowMessSel();	
			}
		}
		else {
			ShowMessSel();
		}
	}
	
	void Update() {
		//************Creatures Effects
		//birds
		if(old_sel_bird_type != select_bird_type) {
			old_sel_bird_type = select_bird_type;
			switch (select_bird_type) {
				case 0: 
					birds.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = all_textures[0] as Texture;
					break;
				case 1: 
					if (custom_bird_tex)
						birds.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = custom_bird_tex;
					break;
				default:break;
			}
		}
		
		if(old_sel_bird_size != select_bird_size) {
			old_sel_bird_size = select_bird_size;
			ParticleEmitter tmp = birds.GetComponent("ParticleEmitter") as ParticleEmitter;
			tmp.minSize = select_bird_size;
			tmp.maxSize = select_bird_size;
		}
		
		if(old_sel_bird_dir != select_bird_dir) {
			old_sel_bird_dir = select_bird_dir;
			ParticleEmitter tmp;
			switch (select_bird_dir) {
				case 0: 
					tmp = birds.GetComponent("ParticleEmitter") as ParticleEmitter;
					if (tmp.worldVelocity.x < 0) {
						tmp.worldVelocity = new Vector3(tmp.worldVelocity.x * -1.0f, tmp.worldVelocity.y * 1.0f, tmp.worldVelocity.z * 1.0f);
						birds.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.5f);
					}
					break;
				case 1: 
					tmp = birds.GetComponent("ParticleEmitter") as ParticleEmitter;
					if (tmp.worldVelocity.x > 0) {
						tmp.worldVelocity = new Vector3(tmp.worldVelocity.x * -1.0f, tmp.worldVelocity.y * 1.0f, tmp.worldVelocity.z * 1.0f);
						birds.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.0f);
					}
					break;
				default:break;
			}
		}
		
		if(old_sel_bird_speed != select_bird_speed) {
			old_sel_bird_speed = select_bird_speed;
			ParticleEmitter tmp = birds.GetComponent("ParticleEmitter") as ParticleEmitter;
			if (tmp.worldVelocity.x > 0) 
				tmp.worldVelocity = new Vector3(0.1f * select_bird_speed, 0.1f * select_bird_speed, tmp.worldVelocity.z);
			else
				tmp.worldVelocity = new Vector3(-0.1f * select_bird_speed, 0.1f * select_bird_speed, tmp.worldVelocity.z);
		}
		//butterflies
		if(old_sel_but_type != select_but_type) {
			old_sel_but_type = select_but_type;
			switch (select_but_type) {
				case 0: 
					butterfly.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = all_textures[0] as Texture;
					break;
				case 1: 
					butterfly.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = all_textures[1] as Texture;				
					break;
				default:break;
			}
		}
		
		if(old_sel_but_count != select_but_count) {
			old_sel_but_count = select_but_count;
			ParticleEmitter tmp = butterfly.GetComponent("ParticleEmitter") as ParticleEmitter;
			tmp.maxEmission = select_but_count;
		}
		
		if(old_sel_but_dir != select_but_dir) {
			old_sel_but_dir = select_but_dir;			
			ParticleAnimator tmp;
			switch (select_but_dir) {
				case 0: 
					tmp = butterfly.GetComponent("ParticleAnimator") as ParticleAnimator;
					if (tmp.worldRotationAxis.x < 0) {
						tmp.worldRotationAxis = new Vector3(tmp.worldRotationAxis.x * -1.0f, tmp.worldRotationAxis.y, tmp.worldRotationAxis.z);
						tmp.localRotationAxis = new Vector3(tmp.localRotationAxis.x * -1.0f, tmp.localRotationAxis.y, tmp.localRotationAxis.z);
						tmp.rndForce = new Vector3(tmp.rndForce.x * -1.0f, tmp.rndForce.y, tmp.rndForce.z);
						tmp.force = new Vector3(tmp.force.x * -1.0f, tmp.force.y, tmp.force.z);
						butterfly.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.5f);
					}
					break;
				case 1: 
					tmp = butterfly.GetComponent("ParticleAnimator") as ParticleAnimator;
					if (tmp.worldRotationAxis.x > 0) {
						tmp.worldRotationAxis = new Vector3(tmp.worldRotationAxis.x * -1.0f, tmp.worldRotationAxis.y, tmp.worldRotationAxis.z);
						tmp.localRotationAxis = new Vector3(tmp.localRotationAxis.x * -1.0f, tmp.localRotationAxis.y, tmp.localRotationAxis.z);
						tmp.rndForce = new Vector3(tmp.rndForce.x * -1.0f, tmp.rndForce.y, tmp.rndForce.z);
						tmp.force = new Vector3(tmp.force.x * -1.0f, tmp.force.y, tmp.force.z);
						butterfly.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.0f);
					}
					break;
				default:break;
			}
		}
		//flies
		if(old_sel_fly_type != select_fly_type) {
			old_sel_fly_type = select_fly_type;
			switch (select_fly_type) {
				case 0: 
					flies.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = all_textures[0] as Texture;
					break;
				case 1: 
					if (custom_fly_tex)
						flies.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = custom_fly_tex;
					break;
				default:break;
			}
		}
		
		if(old_sel_fly_dir != select_fly_dir) {
			old_sel_fly_dir = select_fly_dir;			
			ParticleAnimator tmp;
			switch (select_fly_dir) {
				case 0: 
					tmp = flies.GetComponent("ParticleAnimator") as ParticleAnimator;
					if (tmp.worldRotationAxis.x < 0) {
						tmp.worldRotationAxis = new Vector3(tmp.worldRotationAxis.x * -1.0f, tmp.worldRotationAxis.y, tmp.worldRotationAxis.z);
						tmp.localRotationAxis = new Vector3(tmp.localRotationAxis.x * -1.0f, tmp.localRotationAxis.y, tmp.localRotationAxis.z);
						tmp.rndForce = new Vector3(tmp.rndForce.x * -1.0f, tmp.rndForce.y, tmp.rndForce.z);
						tmp.force = new Vector3(tmp.force.x * -1.0f, tmp.force.y, tmp.force.z);
						flies.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.5f);
					}
					break;
				case 1: 
					tmp = flies.GetComponent("ParticleAnimator") as ParticleAnimator;
					if (tmp.worldRotationAxis.x > 0) {
						tmp.worldRotationAxis = new Vector3(tmp.worldRotationAxis.x * -1.0f, tmp.worldRotationAxis.y, tmp.worldRotationAxis.z);
						tmp.localRotationAxis = new Vector3(tmp.localRotationAxis.x * -1.0f, tmp.localRotationAxis.y, tmp.localRotationAxis.z);
						tmp.rndForce = new Vector3(tmp.rndForce.x * -1.0f, tmp.rndForce.y, tmp.rndForce.z);
						tmp.force = new Vector3(tmp.force.x * -1.0f, tmp.force.y, tmp.force.z);
						flies.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.0f, 0.0f);
					}
					break;
				default:break;
			}
		}
		//************Weather Effects
		//rain
		if (old_sel_rain_intensity != select_rain_intensity) {
			old_sel_rain_intensity = select_rain_intensity;
			ParticleEmitter tmp = rain.GetComponent("ParticleEmitter") as ParticleEmitter;
			tmp.minSize = select_rain_intensity * 0.01f;
			tmp.maxSize = select_rain_intensity * 0.01f;
		}
		
		if (old_sel_snow_intensity != select_snow_intensity) {
			old_sel_snow_intensity = select_snow_intensity;
			ParticleEmitter tmp = snow.GetComponent("ParticleEmitter") as ParticleEmitter;
			tmp.minEmission = select_snow_intensity * 10.0f;
			tmp.maxEmission = select_snow_intensity * 10.0f;
		}
		//************Cursor Effects
		if (old_sel_cursoref_type != select_cursoref_type) {
			old_sel_cursoref_type = select_cursoref_type;
			GameObject cur_parent = null;
			APEComponent tmpc;
			
			if (select_cursoref_type != 3 || custom_cursoref_go) {
				cur_parent = cursoref.transform.parent.gameObject;			
				DestroyImmediate(cursoref);
			}
			
			switch (select_cursoref_type) {
				case 0: 					
					cursoref = (GameObject)Instantiate(all_textures[0] as GameObject);
					break;
				
				case 1: 
					cursoref = (GameObject)Instantiate(all_textures[1] as GameObject);
					break;
				
				case 2: 
					cursoref = (GameObject)Instantiate(all_textures[2] as GameObject);
					break;

				case 3: 
					if (custom_cursoref_go)
						cursoref = (GameObject)Instantiate(custom_cursoref_go);
					break;
				default:break;
			}
			
			if (select_cursoref_type != 3 || custom_cursoref_go) {
				cursoref.transform.parent = cur_parent.transform;
				cursoref.transform.position = cur_parent.transform.position;
				cursoref.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
				cursoref.name = "APE_Cursor_Effect";
						
				cursoref.AddComponent<APEComponent>();
				tmpc = cursoref.GetComponent("APEComponent") as APEComponent;
				tmpc.ID = (int)GUIState.CURSOR;
				Selection.activeObject = cursoref;
			}
		}
		//Magic Balls		
		if(old_sel_mball_type != select_mball_type) {
			old_sel_mball_type = select_mball_type;
			switch (select_mball_type) {
				case 0: 
					mball.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = all_textures[0] as Texture;
					break;
				case 1: 
					mball.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = all_textures[1] as Texture;
					break;
				case 2: 
					mball.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = all_textures[2] as Texture;
					break;
				case 3: 
					if (custom_mball_tex)
						mball.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = custom_fly_tex;
					break;
				default:break;
			}
		}
		
	}
	
	void ShowMessSel() {
		GUILayout.Space(10.0f);
		GUILayout.Label("Select Effect for customisation");
	}
	void Configuration() {
		switch (gui_state) {
			case GUIState.CREATBIRDS:
				GUILayout.Space(10.0f);
				GUILayout.Label("Birds configuration:");
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Birds type:");
				string[] bird_types = {"Seagull", "Custom"};
				select_bird_type = EditorGUILayout.Popup(select_bird_type, bird_types);
				EditorGUILayout.EndHorizontal();
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Birds direction:");
				string[] bird_dirs = {"Right", "Left"};
				select_bird_dir = EditorGUILayout.Popup(select_bird_dir, bird_dirs);
				EditorGUILayout.EndHorizontal();
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Bird size:");
				select_bird_size = EditorGUILayout.FloatField(select_bird_size);
				EditorGUILayout.EndHorizontal();
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Bird speed:");
				select_bird_speed = EditorGUILayout.Slider(select_bird_speed, 0.0f, 10.0f);
				EditorGUILayout.EndHorizontal();
				
				if (select_bird_type == 1) {
					GUILayout.Label("Texture:\nSize: 512x256");
					custom_bird_tex = EditorGUILayout.ObjectField(custom_bird_tex, typeof(Texture), GUILayout.Height(128), GUILayout.Width(128)) as Texture;
					EditorGUILayout.BeginHorizontal();
					if ( GUILayout.Button("Refresh", GUILayout.Width(100)) ) {
						if (custom_bird_tex)
							birds.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = custom_bird_tex;
						else
							Debug.LogError("APE: Please add texture!");
					}
					/*if ( GUILayout.Button("Add custom effect to library", GUILayout.Width(200)) ) {
					
					}*/
					EditorGUILayout.EndHorizontal();
				}
				break;
			
			case GUIState.CREATBUTTER:
				GUILayout.Space(10.0f);
				GUILayout.Label("Butterflies configuration:");
				
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Count butterflies:");
				select_but_count = EditorGUILayout.IntField(select_but_count);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Butterflies type:");
				string[] but_types = {"Orange", "Blue", "Custom"};
				select_but_type = EditorGUILayout.Popup(select_but_type, but_types);
				EditorGUILayout.EndHorizontal();
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Butterflies direction:");
				string[] but_dirs = {"Right", "Left"};
				select_but_dir = EditorGUILayout.Popup(select_but_dir, but_dirs);
				EditorGUILayout.EndHorizontal();
			
				if (select_but_type == 2) {
					GUILayout.Label("Texture:\nSize: 256x256");
					custom_but_tex = EditorGUILayout.ObjectField(custom_but_tex, typeof(Texture), GUILayout.Height(128), GUILayout.Width(128)) as Texture;
					EditorGUILayout.BeginHorizontal();
					if ( GUILayout.Button("Refresh", GUILayout.Width(100)) ) {
						if (custom_but_tex)
							butterfly.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = custom_but_tex;
						else
							Debug.LogError("APE: Please add texture!");
					}
					EditorGUILayout.EndHorizontal();
				}
				break;
			
			case GUIState.CREATFLIES:
				GUILayout.Space(10.0f);
				GUILayout.Label("Flies configuration:");
				
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Flies type:");
				string[] fly_types = {"Normal", "Custom"};
				select_fly_type = EditorGUILayout.Popup(select_fly_type, fly_types);
				EditorGUILayout.EndHorizontal();
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Flies direction:");
				string[] fly_dirs = {"Right", "Left"};
				select_fly_dir = EditorGUILayout.Popup(select_fly_dir, fly_dirs);
				EditorGUILayout.EndHorizontal();
			
				if (select_fly_type == 1) {
					GUILayout.Label("Texture:\nSize: 256x256");
					custom_fly_tex = EditorGUILayout.ObjectField(custom_fly_tex, typeof(Texture), GUILayout.Height(128), GUILayout.Width(128)) as Texture;
					EditorGUILayout.BeginHorizontal();
					if ( GUILayout.Button("Refresh", GUILayout.Width(100)) ) {
						if (custom_fly_tex)
							flies.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = custom_fly_tex;
						else
							Debug.LogError("APE: Please add texture!");
					}
					EditorGUILayout.EndHorizontal();
				}
				break;
			
			case GUIState.TELEPORT:
				GUILayout.Space(10.0f);
				GUILayout.Label("Teleport configuration:\nIt is not customize. Wait in following updating.\nSend me you wish list on the anbytes@gmail.com");
			
				/*EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Teleport type:");
				string[] tel_types = {"Start", "End"};
				select_tel_type = EditorGUILayout.Popup(select_tel_type, tel_types);
				EditorGUILayout.EndHorizontal();*/
				break;
			
			case GUIState.WEATHERRAIN:
				GUILayout.Space(10.0f);
				GUILayout.Label("Rain configuration:");
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Intensity:");
				select_rain_intensity = EditorGUILayout.Slider(select_rain_intensity, 0.0f, 10.0f);
				EditorGUILayout.EndHorizontal();
				break;
			
			case GUIState.WEATHERSNOW:
				GUILayout.Space(10.0f);
				GUILayout.Label("Snow configuration:");
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Intensity:");
				select_snow_intensity = EditorGUILayout.Slider(select_snow_intensity, 0.0f, 10.0f);
				EditorGUILayout.EndHorizontal();
				break;
			
			case GUIState.WEATHERSANDS:
				GUILayout.Space(10.0f);
				GUILayout.Label("Sand Storm configuration:\nIt is not customize. Wait in following updating.\nSend me you wish list on the anbytes@gmail.com");
			
				/*EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Intensity:");
				select_sands_intensity = EditorGUILayout.Slider(select_sands_intensity, 0.0f, 10.0f);
				EditorGUILayout.EndHorizontal();*/
				break;
			
			case GUIState.WEATHERPLIGHTS:
				GUILayout.Space(10.0f);
				GUILayout.Label("Polar Lights configuration:\nIt is not customize. Wait in following updating.\nSend me you wish list on the anbytes@gmail.com");
				break;
			
			case GUIState.CAMWISPS:
				GUILayout.Space(10.0f);
				GUILayout.Label("Wisp configuration:\nIt is not customize. Wait in following updating.\nSend me you wish list on the anbytes@gmail.com");
				break;
			
			case GUIState.CAMLEAVES:
				GUILayout.Space(10.0f);
				GUILayout.Label("Leaves configuration:\nIt is not customize. Wait in following updating.\nSend me you wish list on the anbytes@gmail.com");
				break;
			
			case GUIState.CURSOR:
				GUILayout.Space(10.0f);
				GUILayout.Label("Cursor effect configuration:");
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Cursor effect type:");
				string[] cursoref_types = {"Bubbles", "Smoke", "Sparks", "Custom"};
				select_cursoref_type = EditorGUILayout.Popup(select_cursoref_type, cursoref_types);
				EditorGUILayout.EndHorizontal();
			
				if (select_cursoref_type == 3) {
					GUILayout.Label("GameObject (Particles):");
					custom_cursoref_go = EditorGUILayout.ObjectField( custom_cursoref_go, typeof(GameObject) ) as GameObject;
					EditorGUILayout.BeginHorizontal();
					if ( GUILayout.Button("Refresh", GUILayout.Width(100)) ) {
						if (custom_cursoref_go) {
							GameObject cur_parent = cursoref.transform.parent.gameObject;			
							DestroyImmediate(cursoref);
							cursoref = (GameObject)Instantiate(custom_cursoref_go);
						
							cursoref.transform.parent = cur_parent.transform;
							cursoref.transform.position = cur_parent.transform.position;
							cursoref.transform.localPosition = new Vector3(0.0f, 0.0f, 10.0f);
							cursoref.name = "APE_Cursor_Effect";
						
							cursoref.AddComponent<APEComponent>();
							APEComponent tmpc = cursoref.GetComponent("APEComponent") as APEComponent;
							tmpc.ID = (int)GUIState.CURSOR;
						
							MoveToCursor mov_script = cursoref.GetComponent<MoveToCursor>();
							if (mov_script == null) 
								cursoref.AddComponent<MoveToCursor>();
						
							Selection.activeObject = cursoref;
						}
						else
							Debug.LogError("APE: Please add GameObject!");
					}
					EditorGUILayout.EndHorizontal();
				}
				break;
			
			case GUIState.MAGBALLS:
				GUILayout.Space(10.0f);
				GUILayout.Label("Magic Balls configuration:");
			
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Ball type:");
				string[] mball_types = {"Fireball", "Iceball", "Energyball", "Custom"};
				select_mball_type = EditorGUILayout.Popup(select_mball_type, mball_types);
				EditorGUILayout.EndHorizontal();
			
				if (select_mball_type == 3) {
					GUILayout.Label("Texture:\nSize: 128x128");
					custom_mball_tex = EditorGUILayout.ObjectField(custom_mball_tex, typeof(Texture), GUILayout.Height(128), GUILayout.Width(128)) as Texture;
					EditorGUILayout.BeginHorizontal();
					if ( GUILayout.Button("Refresh", GUILayout.Width(100)) ) {
						if (custom_mball_tex)
							mball.GetComponent("ParticleRenderer").GetComponent<Renderer>().material.mainTexture = custom_mball_tex;
						else
							Debug.LogError("APE: Please add texture!");
					}
					EditorGUILayout.EndHorizontal();
				}
				break;
			
			default:
				ShowMessSel();
				break;
		}
	}
}
