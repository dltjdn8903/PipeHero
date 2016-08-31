using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;

public class SkillMeteorBullet : BaseSkill
{
    GameObject[] m_MeteorPoint = new GameObject[6];

    public override void InitSkill()
    {
        GameObject prefabEffect = Resources.Load<GameObject>("Skills/Pf_Skill_MeteorBullet");

        GameObject EffectObject = GameObject.Instantiate(prefabEffect);

        EffectObject.transform.SetParent(SelfTransform, false);
    }

    public override void UpdateSkill()
    {
        if (END == true)
            return;

        Vector3 targetPosition = Vector3.MoveTowards(SelfTransform.position, DESTINATION, Time.smoothDeltaTime * 5.0f);
        SelfTransform.position = targetPosition;

        if (SelfTransform.position.y <= 0.1f)
        {
            Explosion();
        }
    }

    void Explosion()
    {
        List<Observer_Component> EnemyList = ObserverManager.Instance.GetOtherTeam((eTeamType)OWNER.GetData("TEAM"));

        if (EnemyList != null)
        {
            for (int i = 0; i < EnemyList.Count; ++i)
            {
                if (Vector3.Distance(EnemyList[i].SelfTransform.position, SelfTransform.position) < 1.0f)
                {
                    EnemyList[i].ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);
                }
            }
        }

        Vector3 tempVec = new Vector3(0, 0.5f, 0);

        EffectManager.Instance.ThrowEffect("EXPLOSION", SelfTransform.position /*+ tempVec*/);
        END = true;
    }
}
