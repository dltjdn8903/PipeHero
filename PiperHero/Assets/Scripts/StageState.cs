using UnityEngine;
using System.Collections;

public class StageState : BaseState
{
    StageTemplateData m_StageTemplate = null;

    override public eStateType STATE_TYPE
    {
        get { return eStateType.STATE_TYPE_STAGE; }
    }

    public override void SetParam(params object[] data)
    {
        if( data.Length > 0 )
            m_StageTemplate = data[0] as StageTemplateData;

        m_SceneInfo.SceneName = m_StageTemplate.SCENE_NAME;
    }

    override public void StartState()
    {
        base.StartState();

        if( string.IsNullOrEmpty(m_StageTemplate.SPAWN_ID) == false )
        {
            GameObject prefabSpawn = Resources.Load<GameObject>(m_StageTemplate.SPAWN_ID);
            GameObject.Instantiate(prefabSpawn);
        }

        GlobalValue.Instance.SetMercenary();
        GlobalValue.Instance.EnterArea(eAreaType.AREA_TOWN, Resources.Load<AudioClip>("SisPuellaMagica"));
    }

    override public void UpdateState()
    {
        base.UpdateState();
    }

    override public void EndState()
    {
        base.EndState();

        //TriggerManager.Instance.Clear();
        m_StageTemplate = null;
    }
}
