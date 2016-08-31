using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SkillFireball : BaseSkill
{
    Vector3 m_Destination = Vector3.zero;

    public override void InitSkill()
    {
        GameObject prefabEffect = Resources.Load<GameObject>("Skills/Pf_Skill_MeteorBullet");

        GameObject EffectObject = GameObject.Instantiate(prefabEffect) as GameObject;

        EffectObject.transform.SetParent(SelfTransform, false);
    }

    public override void UpdateSkill()
    {
        if (END == true)
            return;

        Vector3 targetPosition = Vector3.MoveTowards(SelfTransform.position, DESTINATION, Time.smoothDeltaTime * 5.0f);
        targetPosition.y = SelfTransform.position.y;
        SelfTransform.position = targetPosition;

        if (Vector3.Distance(DESTINATION, SelfTransform.position) <= 0.1f)
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

                Observer_Component targetObserv = TARGET.GetComponent<Observer_Component>();

                GameCharacter targetChacrecter = ((GameCharacter)TARGET.GetData("CHARACTER"));

                targetChacrecter.IncreaseCurrentHP(20);

                BaseBoard hpBoard = BoardManager.Instance.GetBoardData(TARGET, eBoardType.BOARD_HP);
                if (hpBoard != null)
                {
                    hpBoard.SetData("HP", targetObserv.GetFactorData(eFactorData.MAX_HP), targetChacrecter.CURRENT_HP);
                }


                if (Vector3.Distance(EnemyList[i].SelfTransform.position, SelfTransform.position) < 1.0f)
                {

                }
            }
        }

        //EffectManager.Instance.ThrowEffect("EXPLOSION", SelfTransform.position);
        END = true;
    }
}
