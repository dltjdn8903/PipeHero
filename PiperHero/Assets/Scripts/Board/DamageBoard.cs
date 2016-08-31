using UnityEngine;
using System.Collections;

public class DamageBoard : BaseBoard
{
    [SerializeField]
    UILabel m_DamageLabel = null;

    public override eBoardType BOARD_TYPE
    {
        get
        {
            return eBoardType.BOARD_DAMAGE;
        }
    }

    public override void SetData(string strKey, params object[] datas)
    {
        switch( strKey )
        {
            case "DAMAGE":
                {
                    double fDamage = (double)datas[0];
                    m_DamageLabel.text = "- " + fDamage.ToString("F0");
                }
                break;
        }
    }

}
