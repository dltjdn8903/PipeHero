using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class SkillHeal : BaseSkill
{
    Vector3 m_Destination = Vector3.zero;

    public override void InitSkill()
    {
        GameObject prefabEffect = Resources.Load<GameObject>("Skills/Pf_Skill_MeteorBullet");

        GameObject EffectObject = Instantiate(prefabEffect) as GameObject;

        EffectObject.transform.SetParent(SelfTransform, false);
    }

    public override void UpdateSkill()
    {
        if (END == true)
            return;

        Vector3 targetPosition = Vector3.MoveTowards(SelfTransform.position, DESTINATION, Time.smoothDeltaTime * 5.0f);
        targetPosition.y = SelfTransform.position.y;
        SelfTransform.position = targetPosition;

        if (Vector3.Distance(DESTINATION, SelfTransform.position) <= 0.1f)
        {
            Explosion();
        }
    }

    void Explosion()
    {
        List<Observer_Component> myTeamList = ObserverManager.Instance.GetSameTeam((eTeamType)OWNER.GetData("TEAM"));

        if (myTeamList != null)
        {
            for (int i = 0; i < myTeamList.Count; ++i)
            {
                if (Vector3.Distance(myTeamList[i].SelfTransform.position, SelfTransform.position) < 1.0f)
                {
                    //myTeamList[i].ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);

                }
            }
        }

        EffectManager.Instance.ThrowEffect("EXPLOSION", SelfTransform.position);
        END = true;
    }
}
