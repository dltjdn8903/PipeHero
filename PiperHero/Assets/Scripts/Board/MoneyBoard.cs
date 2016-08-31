using UnityEngine;
using System.Collections;

public class MoneyBoard : BaseBoard
{
    [SerializeField]
    UILabel m_MoneyLabel = null;

    public override eBoardType BOARD_TYPE
    {
        get
        {
            return eBoardType.BOARD_MONEY;
        }
    }

    public override void SetData(string strKey, params object[] datas)
    {
        switch (strKey)
        {
            case "MONEY_BOARD":
                {
                    int fMoney = (int)datas[0];

                    m_MoneyLabel.text = "+ " + fMoney.ToString("F0");
                }
                break;
        }
    }

}
