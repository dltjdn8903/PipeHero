using UnityEngine;
using System.Collections;

public class UI_StageGrid_EventHandler : MonoBehaviour
{
    [SerializeField]
    UILabel m_StageNumberLabel = null;

    StageTemplateData m_StageTemplateData = null;

    public StageTemplateData STAGE_TEMPLATE
    {
        set
        {
            m_StageTemplateData = value;
            _Refresh();
        }
    }

    void _Refresh()
    {
        if (m_StageTemplateData == null)
            return;

        string strNumber = m_StageTemplateData.EPISODE_ID.ToString() + " - " + m_StageTemplateData.STAGE_ID.ToString();
        m_StageNumberLabel.text = strNumber;
    }


    public void OnClickStageStart()
    {
        StateManager.Instance.ChangeState(eStateType.STATE_TYPE_STAGE, m_StageTemplateData);
    }


}
