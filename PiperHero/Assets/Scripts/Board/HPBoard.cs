using UnityEngine;
using System.Collections;

public class HPBoard : BaseBoard
{
    [SerializeField]
    UISprite m_BarSprite = null;

    [SerializeField]
    UILabel m_HPLabel = null;

    override public eBoardType BOARD_TYPE
    {
        get { return eBoardType.BOARD_HP; }
    }

    public override void SetData(string strKey, params object[] datas)
    {
        switch( strKey )
        {
            case "HP":
                {
                    double fMaxHP = (double)datas[0];
                    double fCurrentHP = (double)datas[1];

                    m_BarSprite.fillAmount = (float)(fCurrentHP / fMaxHP);
                    m_HPLabel.text = fCurrentHP.ToString("F0") + " / " + fMaxHP.ToString("F0");
                }
                break;
        }

    }
}
