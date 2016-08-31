using UnityEngine;
using System.Collections;
using System;

public class SkillWhirlwind : BaseSkill
{
    float m_ElapsedTime = 0;

    public override void InitSkill()
    {
        int a = 0;

        // 쿨타임 시작
        UI_Dungeon_EventHandler.Instance.b_startCoolTime2 = true;
    }

    public override void UpdateSkill()
    {
        m_ElapsedTime += Time.smoothDeltaTime;
        if (m_ElapsedTime >= 0.1f)
        {
            END = true;
            UI_Dungeon_EventHandler.Instance.b_SkillEnd = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (END == true)
            return;

        GameObject colObject = other.gameObject;
        BaseObject observerObject = colObject.GetComponent<BaseObject>();

        if ((eTeamType)observerObject.GetData("TEAM") == (eTeamType)OWNER.GetData("TEAM"))
            return;

        SKILLEVENT = Whirlwind;

        observerObject.ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);
        observerObject.ThrowEvent("SIDE_EFFECT", OWNER, SKILLEVENT, eConditionType.CONDITION_KNOCKBACK);
    }

    public IEnumerator Whirlwind(BaseObject _caster, BaseObject _target)
    {
        Vector3 Direction = (_target.SelfTransform.position - _caster.SelfTransform.position).normalized;
        
        NavMeshAgent testNav = ((Observer_Component)_target).AI.NAV_MESH_AGENT;
        testNav.Resume();
        testNav.SetDestination(_target.SelfTransform.position + (Direction * 2.0f));
        yield return null;
    }
}
