using UnityEngine;
using System.Collections;

public class LogoState : BaseState
{
    float m_fElapsedTime = 0.0f;
    float m_fLimitTime = 5.0f;

    StageTemplateData m_StageTemplateData = null;

    override public eStateType STATE_TYPE
    {
        get { return eStateType.STATE_TYPE_LOGO; }
    }

    override public void StartState()
    {
        m_StageTemplateData = StageManager.Instance.GetStage(1, Random.Range(4, 6));

        base.StartState();
    }

    override public void UpdateState()
    {
        base.UpdateState();
        m_fElapsedTime += Time.smoothDeltaTime;

        if( m_fElapsedTime > m_fLimitTime )
        {
            ChangeState(eStateType.STATE_TYPE_STAGE, m_StageTemplateData);
        }
    }

    override public void EndState()
    {
        base.EndState();
        m_fElapsedTime = 0;
    }
}
