using UnityEngine;
using System.Collections;
using System;

public class SkillBanish : BaseSkill
{
//    SkillCondition m_EffectDelegate;

    bool m_StartSkill = false;

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
    }

    void OnTriggerEnter( Collider other )
    {
        if (END == true)
            return;

        GameObject colliderObject = other.gameObject;
        BaseObject targetObject = colliderObject.GetComponent<BaseObject>();

        if (targetObject != TARGET)
            return;

        //m_EffectDelegate = SkillManager.Instance.SkillKnockBack;

        TARGET.ThrowEvent("HIT", OWNER.GetData("CHARACTER"), SKILL_TEMPLATE);
        //TARGET.ThrowEvent("SIDEEFFECT", OWNER, m_EffectDelegate);
        END = true;
    }

    bool Banish(BaseObject _target)
    {
        return false;
    }
}
