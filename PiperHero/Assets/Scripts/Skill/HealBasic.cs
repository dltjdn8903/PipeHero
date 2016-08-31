using UnityEngine;
using System.Collections;
using System;

public class HealBasic : BaseSkill
{
    Vector3 m_Destination = Vector3.zero;

    public override void InitSkill()
    {
        if (TARGET != null)
        {
            Observer_Component targetObserv = TARGET.GetComponent<Observer_Component>();

            GameCharacter targetChacrecter = ((GameCharacter)TARGET.GetData("CHARACTER"));

            targetChacrecter.IncreaseCurrentHP(20);

            BaseBoard hpBoard = BoardManager.Instance.GetBoardData(TARGET, eBoardType.BOARD_HP);
            if (hpBoard != null)
            {
                hpBoard.SetData("HP", targetObserv.GetFactorData(eFactorData.MAX_HP), targetChacrecter.CURRENT_HP);
            }

            END = true;
        }
    }

    public override void UpdateSkill() { }
}
