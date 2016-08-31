using UnityEngine;
using System.Collections;
//using System;

public class SkillMeteor : BaseSkill
{
    float m_ElapsedTime = 0;

    public override void InitSkill()
    {
        UI_Dungeon_EventHandler.Instance.b_startCoolTime3 = true;

        SKILLEVENT = Meteor;

        OWNER.ThrowEvent("SIDE_EFFECT", OWNER, SKILLEVENT, eConditionType.CONDITION_NONE);

        END = true;
    }

    public override void UpdateSkill()
    {

    }

    IEnumerator Meteor(BaseObject _caster, BaseObject _target)
    {
        while (true)
        {
            for (int i = 0; i < 15; ++i)
            {
                Vector3 RandomVector = new Vector3(Random.Range(-2.5f, 2.5f), 0, Random.Range(-2.5f, 2.5f));
                RandomVector = _caster.SelfTransform.position + RandomVector;
                SkillManager.Instance.RunSkill(OWNER, "METEOR_BULLET_1", RandomVector);
                yield return new WaitForSeconds(0.5f);
            }
            break;
        }
    }
}
