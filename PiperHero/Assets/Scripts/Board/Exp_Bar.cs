using UnityEngine;
using System.Collections;

public class Exp_Bar : BaseBoard
{
    [SerializeField]
    UISprite m_BarSprite = null;

    [SerializeField]
    UILabel m_ExpLabel = null;

    override public eBoardType BOARD_TYPE
    {
        get { return eBoardType.BOARD_EXP; }
    }

    //+ 보드 HpBar 추가
    public override void SetData(string strKey, params object[] datas)
    {
        switch (strKey)
        {
            case "EXP_BAR":
                {
                    double fExpPoint = ((int)datas[0] * 1.0f);

                    m_BarSprite.fillAmount = (float)( fExpPoint / GlobalValue.Instance.m_NeedExp );
                    if (m_BarSprite.fillAmount >= 1.0f)
                    {
                        m_BarSprite.fillAmount = 0;
                        GlobalValue.Instance.m_ExpPoint = 0;
                        m_ExpLabel.text = (m_BarSprite.fillAmount * 100).ToString("F0") + " % ";
                        GlobalValue.Instance.m_Level += 1;
                        UI_Dungeon_EventHandler.Instance.m_Level.text = string.Format("{0}", GlobalValue.Instance.m_Level);

                        //const float magicnumber = 1.1958131745004019414600484002536f;
                        //GlobalValue.Instance.m_MaxHP = (double)(Mathf.Pow(magicnumber, GlobalValue.Instance.m_Level - 1) * 100 + 0.5f);
                        //UI_Dungeon_EventHandler.Instance.setHpData("HP_BAR", GlobalValue.Instance.m_MaxHP, GlobalValue.Instance.m_MaxHP);
                        

                        return;
                    }

                    m_ExpLabel.text = (m_BarSprite.fillAmount * 100).ToString("F0") + " % ";
                }
                break;
        }

    }
}
