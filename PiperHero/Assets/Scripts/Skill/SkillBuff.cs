using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillBuff : BaseSkill
{
    float m_ElapsedTime = 0;

    public override void InitSkill()
    {
        SKILLEVENT = Buff;

        eTeamType tmpType = (eTeamType)OWNER.GetData("TEAM");

        List<Observer_Component> TeamList = ObserverManager.Instance.GetMinions(tmpType);
        if (TeamList != null)
        {
            for (int i = 0; i < TeamList.Count; ++i)
            {
                TeamList[i].ThrowEvent("SIDE_EFFECT", TeamList[i], SKILLEVENT, eConditionType.CONDITION_NONE);
            }
        }
        END = true;
    }

    public override void UpdateSkill() { }

    public IEnumerator Buff(BaseObject _caster, BaseObject _target)
    {
        _target.ThrowEvent("TRANS_FACTOR", eFactorData.ATTACK.ToString("F"), 20);
        _target.ThrowEvent("TRANS_FACTOR", eFactorData.DEFENCE.ToString("F"), 20);
        yield return new WaitForSeconds(10.0f);

        _target.ThrowEvent("TRANS_FACTOR", eFactorData.ATTACK.ToString("F"), -20);
        _target.ThrowEvent("TRANS_FACTOR", eFactorData.DEFENCE.ToString("F"), -20);
        yield return null;
    }
}
