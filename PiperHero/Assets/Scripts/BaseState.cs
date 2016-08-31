using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseState : MonoBehaviour
{
    virtual public eStateType STATE_TYPE
    {
        get { return eStateType.STATE_TYPE_NONE; }
    }

    Dictionary<eStateType, BaseState> m_dicNextState = new Dictionary<eStateType, BaseState>();

    List<GameObject> m_listMakeUI = new List<GameObject>();
    List<GameObject> m_listMakeObject = new List<GameObject>();

    BaseState m_NextState = null;

    protected stSceneInfo m_SceneInfo;
    public stSceneInfo SCENE_INFO
    {
        get { return m_SceneInfo; }
        set { m_SceneInfo = value; }
    }

    public BaseState NEXT_STATE { get { return m_NextState; } }

    virtual public void SetParam( params object[] data )
    {
    }

    virtual public void StartState()
    {
        if (SCENE_INFO.arrUI != null)
        {
            for (int i = 0; i < SCENE_INFO.arrUI.Length; ++i)
            {
                GameObject prefabUI = Resources.Load<GameObject>(SCENE_INFO.arrUI[i]);
                GameObject uiInstance =NGUITools.AddChild(null, prefabUI);

                m_listMakeUI.Add(uiInstance);
            }
        }

        if( SCENE_INFO.arrObject != null )
        {
            for( int i = 0; i < SCENE_INFO.arrObject.Length; ++i )
            {
                GameObject prefabObject = Resources.Load<GameObject>(SCENE_INFO.arrObject[i]);
                GameObject objectInstance = GameObject.Instantiate(prefabObject);

                m_listMakeObject.Add(objectInstance);
            }
        }

    }
    virtual public void UpdateState() { }
    virtual public void EndState()
    {
        m_NextState = null;

        for( int i = 0; i < m_listMakeUI.Count; ++i )
        {
            GameObject uiInstance = m_listMakeUI[i];

            if (uiInstance != null)
                NGUITools.Destroy(uiInstance);
        }
        m_listMakeUI.Clear();

        for( int i = 0; i < m_listMakeObject.Count; ++i )
        {
            GameObject objectInstance = m_listMakeObject[i];
            
            if (objectInstance != null)
                Destroy(objectInstance);
        }
        m_listMakeObject.Clear();

    }

    public void AddChildState( BaseState childState )
    {
        if (m_dicNextState.ContainsKey(childState.STATE_TYPE) == true)
            return;

        m_dicNextState[childState.STATE_TYPE] = childState;
        childState._AddParent(this);
    }

    protected void _AddParent( BaseState parentState )
    {
        if (m_dicNextState.ContainsKey(parentState.STATE_TYPE) == true)
            return;

        m_dicNextState[parentState.STATE_TYPE] = parentState;
    }

    public void ChangeState( eStateType changeState, params object[] data )
    {
        BaseState nextState = null;
        m_dicNextState.TryGetValue(changeState, out nextState);
        if (nextState == null)
            return;

        m_NextState = nextState;
        m_NextState.SetParam(data);
    }



}
