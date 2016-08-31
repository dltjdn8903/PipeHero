using UnityEngine;
using System.Collections;
using System;



public class RangeSkill : BaseSkill
{
    Vector3 m_Destination = Vector3.zero;

    public override void InitSkill()
    {
        GameObject prefabEffect = Resources.Load<GameObject>("Skills/Pf_Skill_Fireball");

        GameObject EffectObject = GameObject.Instantiate(prefabEffect);

        EffectObject.transform.SetParent(SelfTransform, false);
    }

    public override void UpdateSkill()
    {
        if (TARGET == null || OWNER == null)
        {
            END = true;
            return;
        }

        Vector3 targetPosition = Vector3.MoveTowards(SelfTransform.position, TARGET.transform.position, Time.smoothDeltaTime * 5.0f);
        targetPosition.y = SelfTransform.position.y;
        SelfTransform.position = targetPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (END == true)
            return;

        GameObject colliderObject = other.gameObject;
        BaseObject targetObject = colliderObject.GetComponent<BaseObject>();

        if (targetObject != TARGET)
            return;

        SKILLEVENT = null;

        TARGET.ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);

        END = true;
    }
}