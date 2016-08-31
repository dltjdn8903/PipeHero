using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireNovaBullet : BaseSkill
{
    float m_ElapsedTime = 0;

    public override void InitSkill()
    {

    }

    public override void UpdateSkill()
    {
        if (END == true)
            return;

        m_ElapsedTime += Time.smoothDeltaTime;

        Vector3 targetPosition = Vector3.MoveTowards(SelfTransform.position, DESTINATION, Time.smoothDeltaTime * 3.0f);
        SelfTransform.position = targetPosition;

        if (m_ElapsedTime >= 0.2f)
        {
            EffectManager.Instance.ThrowEffect("FIRENOVA", SelfTransform.position);
            Explosion();
            m_ElapsedTime = 0;
        }

        if (Vector3.Distance(DESTINATION, SelfTransform.position) < 0.2f)
        {
            END = true;
        }
    }

    void Explosion()
    {
        List<Observer_Component> EnemyList = ObserverManager.Instance.GetOtherTeam((eTeamType)OWNER.GetData("TEAM"));

        if (EnemyList != null)
        {
            for (int i = 0; i < EnemyList.Count; ++i)
            {
                if (Vector3.Distance(EnemyList[i].SelfTransform.position, SelfTransform.position) < 0.5f)
                {
                    EnemyList[i].ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);
                }
            }
        }
    }
}
