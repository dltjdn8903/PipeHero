using UnityEngine;
using System.Collections;
using System;

public class MeleeSkill : BaseSkill
{
    float m_ElapsedTime = 0;

    SkillImmune m_Immune = new SkillImmune();

    public override void InitSkill()
    {

    }

    public override void UpdateSkill()
    {
        m_ElapsedTime += Time.smoothDeltaTime;
        if( m_ElapsedTime >= 0.1f)
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

        if (observerObject != TARGET)
            return;

        TARGET.ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);

        //StartCoroutine(Whirlwind(OWNER, TARGET));

        //      if (m_Immune.MeleeCondition() == eSkillTemplateType.SKILL_IMMUNEDAMAGE)
        //      {
        //          TARGET.ThrowEvent("IMMUNE", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);
        //          TARGET.ThrowEvent("SIDE_EFFECT", OWNER.GetData("CHARACTER"), SKILLFUNCTION);
        //      }
        //else
        //      {
        //          
        //          TARGET.ThrowEvent("SIDE_EFFECT", OWNER.GetData("CHARACTER"), SKILLFUNCTION);
        //      }
        END = true;
    }
}
