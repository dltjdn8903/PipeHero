using UnityEngine;
using System.Collections;

public class SkillPotion : BaseSkill
{
    Vector3 m_Destination = Vector3.zero;

    public override void InitSkill()
    {
        Observer_Component OwnObser = OWNER.GetComponent<Observer_Component>();

        GameCharacter OwnChar = ((GameCharacter)OWNER.GetData("CHARACTER"));

        OwnChar.IncreaseCurrentHP(100);

        BaseBoard hpBoard = BoardManager.Instance.GetBoardData(OWNER, eBoardType.BOARD_HP);
        if (hpBoard != null)
        {
            hpBoard.SetData("HP", OwnObser.GetFactorData(eFactorData.MAX_HP), OwnChar.CURRENT_HP);
        }

        END = true;
    }

    public override void UpdateSkill() { }
}
