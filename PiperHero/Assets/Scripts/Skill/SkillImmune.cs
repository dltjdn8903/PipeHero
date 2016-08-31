using UnityEngine;
using System.Collections;

public class SkillImmune : BaseSkill
{
    float m_ElapsedTime = 0;

    public override void InitSkill()
    {
        SKILLEVENT = Immune;

        EffectManager.Instance.ThrowEffect("IMMUNE_0", OWNER.SelfTransform.position, OWNER);
        EffectManager.Instance.ThrowEffect("IMMUNE_1", OWNER.SelfTransform.position, OWNER);

        // 쿨타임 시작
        UI_Dungeon_EventHandler.Instance.b_startCoolTime3 = true;

        OWNER.ThrowEvent("SIDE_EFFECT", OWNER, SKILLEVENT, eConditionType.CONDITION_IMMUNEDAMAGE);
        END = true;
    }

    public IEnumerator Immune(BaseObject _caster, BaseObject _target)
    {
        Debug.Log("ImmuneSTART");
        _caster.ThrowEvent("TRANS_CONDISION", eConditionType.CONDITION_IMMUNEDAMAGE);
        yield return new WaitForSeconds(10.0f);
        Debug.Log("ImmuneEND");
        _caster.ThrowEvent("TRANS_CONDITION", eConditionType.CONDITION_NONE);
        yield return null;
    }

    public override void UpdateSkill() {  }
}
