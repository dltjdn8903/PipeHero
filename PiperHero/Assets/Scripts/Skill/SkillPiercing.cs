using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SkillPiercing : BaseSkill
{
    Vector3 m_Destination = Vector3.zero;

    float m_ElapsedTime = 0;

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

        if (m_ElapsedTime < 0.5f)
        {
            m_ElapsedTime += Time.smoothDeltaTime;
        }
        else
        {
            END = true;
        }

        Vector3 targetPosition = Vector3.MoveTowards(SelfTransform.position, DESTINATION, Time.smoothDeltaTime * 10.0f);
        targetPosition.y = SelfTransform.position.y;
        SelfTransform.position = targetPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject colObject = other.gameObject;
        BaseObject observerObject = colObject.GetComponent<BaseObject>();

        if ((eTeamType)observerObject.GetData("TEAM") == (eTeamType)OWNER.GetData("TEAM"))
            return;

        observerObject.ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);
    }
}