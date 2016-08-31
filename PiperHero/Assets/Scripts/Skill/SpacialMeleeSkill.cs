using UnityEngine;
using System.Collections;
using System;

public class SpacialMeleeSkill : BaseSkill
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

        //Vector3 vectorTemp = OWNER.SelfTransform.position - TARGET.SelfTransform.position;
        //vectorTemp.Normalize();

        //float fDot = Vector3.Dot(vectorTemp, OWNER.SelfTransform.forward * -1);
        //double fAngle = 0;

        //if (fDot < 1)
        //{
        //    double fData = Math.Acos(fDot);
        //    if (fData != 0)
        //        fAngle = Mathf.Rad2Deg * fData;
        //}

        //if (Math.Abs(fAngle) <= SKILL_TEMPLATE.RANGE_DATA_2 * 0.5f)
        //{
        //    TARGET.ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);
        //    END = true;
        //}
    }
}
