using UnityEngine;
using System.Collections;
using System;

public class SkillAgrro : BaseSkill
{
    float m_ElapsedTime = 0;

    public override void InitSkill()
    {
        int a = 0;

        // 쿨타임 시작        
        UI_Dungeon_EventHandler.Instance.b_startCoolTime1 = true;
            
    }

    public override void UpdateSkill()
    {
        m_ElapsedTime += Time.smoothDeltaTime;
        if (m_ElapsedTime >= 0.1f)
        {
            END = true;
        }
    }

    void OnTriggerEnter( Collider other )
    {
        if (END == true)
            return;
        
        GameObject colObject = other.gameObject;
        BaseObject observerObject = colObject.GetComponent<BaseObject>();

        if ((eTeamType)observerObject.GetData("TEAM") == (eTeamType)OWNER.GetData("TEAM"))
            return;

        SKILLEVENT = Agrro;

        //observerObject.ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);
        observerObject.ThrowEvent("SIDE_EFFECT", OWNER, SKILLEVENT, eConditionType.CONDITION_AGRRO);
    }
    public IEnumerator Agrro(BaseObject _caster, BaseObject _target)
    {
        ((Observer_Component)_target).AI.AGRROTIME = 5;
        ((Observer_Component)_target).ThrowEvent("ATTACK_TARGET", _caster);
        yield return null;
    }

}