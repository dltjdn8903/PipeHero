using UnityEngine;
using System.Collections;
using System;

public class SkillSummon : BaseSkill
{
    float m_ElapsedTime = 0;

    public override void InitSkill()
    {
        GameObject prefabEffect = Resources.Load<GameObject>("Skills/Pf_Skill_Summon");
        GameObject EffectObject = GameObject.Instantiate(prefabEffect);
        EffectObject.transform.SetParent(SelfTransform, false);
        EffectObject.transform.position = OWNER.SelfTransform.position;
    }

    public override void UpdateSkill()
    {
        m_ElapsedTime += Time.smoothDeltaTime;
        //EffectManager.Instance.ThrowEffect()//이펙트 나오고 
        if( m_ElapsedTime >= 2.0f)//몇초뒤에 적 소환
        {
            END = true;
        }
    }
}
