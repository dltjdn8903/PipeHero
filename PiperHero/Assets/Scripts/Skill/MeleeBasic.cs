using UnityEngine;
using System.Collections;
using System;

public class MeleeBasic : BaseSkill
{
    float m_ElapsedTime = 0;

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
        END = true;
    }
}
