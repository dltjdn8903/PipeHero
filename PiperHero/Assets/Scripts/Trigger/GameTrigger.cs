using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct stGameCondition
{
    [SerializeField]
    public eGameCondition m_ConditionType;

    [SerializeField]
    public BaseTrigger m_TargetTrigger;

    [SerializeField]
    public bool m_bOr;
}

public class GameTrigger : BaseTrigger
{
    [SerializeField]
    List<stGameCondition> m_listClearCondition = new List<stGameCondition>();

    [SerializeField]
    List<stGameCondition> m_listFailCondition = new List<stGameCondition>();

    [SerializeField]
    BaseTrigger m_ClearCallTrigger = null;

    [SerializeField]
    BaseTrigger m_FailCallTrigger = null;

    public List<stGameCondition> LIST_CLEAR_CONDITION { get { return m_listClearCondition; } }
    public List<stGameCondition> LIST_FAIL_CONDITION { get { return m_listFailCondition; } }

    public BaseTrigger CLEAR_CALL_TRIGGER
    {
        get { return m_ClearCallTrigger; }
        set { m_ClearCallTrigger = value; }
    }
    public BaseTrigger FAIL_CALL_TRIGGER
    {
        get { return m_FailCallTrigger; }
        set { m_FailCallTrigger = value; }
    }

    public override void InitTrigger()
    {
        
    }

    public override eTriggerResultType UpdateTrigger()
    {
        eTriggerResultType resultType = base.UpdateTrigger();
        if (resultType != eTriggerResultType.TRIGGER_RESULT_ING)
            return resultType;

        bool bClearResult = _CheckClear();
        bool bFailResult = _CheckFail();

        if (bClearResult == true)
        {
            if (m_ClearCallTrigger != null)
                m_ClearCallTrigger.CALL = true;

            return eTriggerResultType.TRIGGER_RESULT_ING;
        }

        if (bFailResult == true)
        {
            if (m_FailCallTrigger != null)
                m_FailCallTrigger.CALL = true;
            else
            {
                UIManager.Instance.ShowUI(eUIType.Pf_UI_Result_Fail);
            }

            return eTriggerResultType.TRIGGER_RESULT_ING;
        }

        return eTriggerResultType.TRIGGER_RESULT_ING;
    }

    bool _CheckClear()
    {
        bool bResult = false;

        for (int i = 0; i < m_listClearCondition.Count; ++i)
        {
            stGameCondition condition = m_listClearCondition[i];

            switch (condition.m_ConditionType)
            {
                case eGameCondition.CLEAR_TRIGGER:
                    {
                        if (condition.m_TargetTrigger == null)
                            bResult = true;
                    }
                    break;

                case eGameCondition.MONSTER_CLEAR:
                    {
                        bResult = !TriggerManager.Instance.HasMonsterSpawn();
                    }
                    break;
            }
        }
        return bResult;
    }

    bool _CheckFail()
    {
        for (int i = 0; i < m_listFailCondition.Count; ++i)
        {
            stGameCondition condition = m_listFailCondition[i];
            switch (condition.m_ConditionType)
            {
                case eGameCondition.SELF_CHARACTER_DIE:
                    {
                        if (TriggerManager.Instance.MAIN_OBJECT == null)
                            return true;
                    }
                    break;
            }
        }

        return false;
    }
}