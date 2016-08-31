using UnityEngine;
using System.Collections;

public class ExpBoard : BaseBoard
{
    [SerializeField]
    UILabel m_ExpLabel = null;

    override public eBoardType BOARD_TYPE
    {
        get { return eBoardType.BOARD_EXP; }
    }

    //+ 보드 Exp보드 추가
    public override void SetData(string strKey, params object[] datas)
    {
        switch (strKey)
        {
            case "EXP_BOARD":
                {
                    int fExp = (int)datas[0];

                    m_ExpLabel.text = "+ " + fExp.ToString("F0");
                }
                break;
        }

    }
}
