using UnityEngine;
using System.Collections;
using System;

public class SpacialRangeSkill : BaseSkill
{
    Vector3 m_Destination = Vector3.zero;

    public override void InitSkill()
    {
        GameObject prefabEffect = Resources.Load<GameObject>("Pf_Effect_Skel");
        GameObject EffectObject = GameObject.Instantiate(prefabEffect);

        EffectObject.transform.SetParent(SelfTransform, false);

        m_Destination = TARGET.transform.position;
    }

    public override void UpdateSkill()
    {
        if (TARGET == null)
        {
            END = true;
            return;
        }

        Vector3 targetPosition = Vector3.MoveTowards(SelfTransform.position, TARGET.transform.position, Time.smoothDeltaTime * 5.0f);
        targetPosition.y = SelfTransform.position.y;
        SelfTransform.position = targetPosition;

        //if(Vector3.Distance(m_Destination, SelfTransform.position) < 0.1f || TARGET == null)
        //{
        //    END = true;
        //}
    }

    void OnTriggerEnter( Collider other )
    {
        if (END == true)
            return;

        GameObject colliderObject = other.gameObject;
        BaseObject targetObject = colliderObject.GetComponent<BaseObject>();

        if (targetObject != TARGET)
            return;

        TARGET.ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);
        END = true;
    }
}
