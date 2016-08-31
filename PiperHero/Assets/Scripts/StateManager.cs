using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;


public struct stSceneInfo
{
    public string SceneName;
    public string[] arrUI;
    public string[] arrObject;
}

public class StateManager : BaseManager< StateManager >
{
    Dictionary<eStateType, BaseState> m_dicState = new Dictionary<eStateType, BaseState>();
    Dictionary<eStateType, stSceneInfo> m_dicSceneInfo = new Dictionary<eStateType, stSceneInfo>();

    BaseState m_CurrnetState = null;
    AsyncOperation m_Operation = null;

    float m_fElapsedTime = 0.0f;

    void Awake()
    {
        TextAsset sceneText = Resources.Load<TextAsset>("SCENE_INFO");
        if( sceneText != null )
        {
            JSONClass nodeData = JSON.Parse(sceneText.text) as JSONClass;
            if( nodeData != null )
            {
                JSONClass sceneInfoNode = nodeData["SCENE_INFO"] as JSONClass;

                for ( int i = (int)eStateType.STATE_TYPE_LOGO; i < (int)eStateType.STATE_TYPE_COUNT; ++i )
                {
                    JSONClass sceneClass = sceneInfoNode[((eStateType)i).ToString("F")] as JSONClass;
                    if( sceneClass != null )
                    {
                        stSceneInfo sceneInfo = new stSceneInfo();
                        sceneInfo.SceneName = sceneClass["SCENE_NAME"];

                        JSONArray arrUI = sceneClass["UI"] as JSONArray;
                        if( arrUI != null && arrUI.Count > 0 )
                        {
                            sceneInfo.arrUI = new string[arrUI.Count];
                            for (int k = 0; k < arrUI.Count; ++k)
                            {
                                sceneInfo.arrUI[k] = arrUI[k];
                            }
                        }
                        

                        JSONArray arrObject = sceneClass["OBJECT"] as JSONArray;
                        if( arrObject != null && arrObject.Count > 0 )
                        {
                            sceneInfo.arrObject = new string[arrObject.Count];
                            for (int k = 0; k < arrObject.Count; ++k)
                            {
                                sceneInfo.arrObject[k] = arrObject[k];
                            }
                        }

                        m_dicSceneInfo.Add((eStateType)i, sceneInfo);
                    }
                }
            }
        }

        for( int i = (int)eStateType.STATE_TYPE_LOGO; i < (int)eStateType.STATE_TYPE_COUNT; ++i )
        {
            eStateType stateType = (eStateType)i;
            BaseState makeState = _CreateState( stateType );
            m_dicState.Add(stateType, makeState);
        }

        StartState(eStateType.STATE_TYPE_LOGO);

        RegisterChild(eStateType.STATE_TYPE_LOGO, eStateType.STATE_TYPE_LOBBY);
        RegisterChild(eStateType.STATE_TYPE_LOGO, eStateType.STATE_TYPE_STAGE);

        RegisterChild(eStateType.STATE_TYPE_LOBBY, eStateType.STATE_TYPE_LOGO);
        RegisterChild(eStateType.STATE_TYPE_LOBBY, eStateType.STATE_TYPE_STAGE);

        RegisterChild(eStateType.STATE_TYPE_STAGE, eStateType.STATE_TYPE_LOGO);
        RegisterChild(eStateType.STATE_TYPE_STAGE, eStateType.STATE_TYPE_LOBBY);
    }

    void Update()
    {
        if( m_Operation != null )
        {
            m_fElapsedTime += Time.smoothDeltaTime;
            if (m_fElapsedTime > 2.0f)
                m_fElapsedTime = 2.0f;

            UIManager.Instance.ShowLoadingUI(m_fElapsedTime / 2.0f);
            //UIManager.Instance.ShowLoadingUI(m_Operation.progress);

            if ( m_Operation.isDone == true && m_fElapsedTime >= 2.0f )
            {
                // 완료
                m_Operation = null;
                UIManager.Instance.HideLoadingUI();

                if( m_CurrnetState != null)
                {
                    m_CurrnetState.StartState();
                }
            }
            else
            {
                // 진행중
                return;
            }
        }


        if (m_CurrnetState == null)
            return;

        m_CurrnetState.UpdateState();
        if( m_CurrnetState.NEXT_STATE != null )
        {
            BaseState prevState = m_CurrnetState;
            m_CurrnetState = m_CurrnetState.NEXT_STATE;

            prevState.EndState();

            if( prevState.SCENE_INFO.SceneName != m_CurrnetState.SCENE_INFO.SceneName)
            {
                m_Operation = Application.LoadLevelAsync(m_CurrnetState.SCENE_INFO.SceneName);
                UIManager.Instance.ShowLoadingUI(0.0f);
                m_fElapsedTime = 0;
            }
            else
            {
                m_CurrnetState.StartState();
            }
        }
    }

    void StartState( eStateType startState )
    {
        BaseState start = _GetState(startState);
        if( start != null )
        {
            m_CurrnetState = start;
            m_CurrnetState.StartState();
        }
    }

    void RegisterChild( eStateType parentStateType, eStateType childStateType )
    {
        BaseState parentState = _GetState(parentStateType);
        BaseState childState = _GetState(childStateType);

        if( parentState != null && childState != null )
        {
            parentState.AddChildState(childState);
        }
    }

    BaseState _GetState( eStateType stateType )
    {
        BaseState returnState = null;
        m_dicState.TryGetValue(stateType, out returnState);
        return returnState;
    }

    BaseState _CreateState( eStateType stateType )
    {
        BaseState makeState = null;

        switch( stateType )
        {
            case eStateType.STATE_TYPE_LOGO:
                {
                    GameObject stateObject = new GameObject();
                    stateObject.name = eStateType.STATE_TYPE_LOGO.ToString("F");

                    DontDestroyOnLoad(stateObject);

                    makeState = stateObject.AddComponent<LogoState>();

                    if( m_dicSceneInfo.ContainsKey(stateType) )
                    {
                        makeState.SCENE_INFO = m_dicSceneInfo[stateType];
                    }
                }
                break;

            case eStateType.STATE_TYPE_LOBBY:
                {
                    GameObject stateObject = new GameObject();
                    stateObject.name = eStateType.STATE_TYPE_LOBBY.ToString("F");

                    DontDestroyOnLoad(stateObject);

                    makeState = stateObject.AddComponent<LobbyState>();
                    if (m_dicSceneInfo.ContainsKey(stateType))
                    {
                        makeState.SCENE_INFO = m_dicSceneInfo[stateType];
                    }
                }
                break;

            case eStateType.STATE_TYPE_STAGE:
                {
                    GameObject stateObject = new GameObject();
                    stateObject.name = eStateType.STATE_TYPE_STAGE.ToString("F");

                    DontDestroyOnLoad(stateObject);

                    makeState = stateObject.AddComponent<StageState>();
                    if (m_dicSceneInfo.ContainsKey(stateType))
                    {
                        makeState.SCENE_INFO = m_dicSceneInfo[stateType];
                    }
                }
                break;
        }

        return makeState;
    }

    public void ChangeState( eStateType stateType, params object[] data )
    {
        if (m_CurrnetState != null)
            m_CurrnetState.ChangeState(stateType, data);
    }

}
