using UnityEngine;
using System.Collections;

public class UI_Status_EventHandler : MonoBehaviour
{
    [SerializeField]
    UILabel m_Level = null;
    [SerializeField]
    UILabel m_Hp = null;
    [SerializeField]
    UILabel m_Exp = null;
    [SerializeField]
    UILabel m_Money = null;

    [SerializeField]
    UILabel m_Attack = null;

    [SerializeField]
    UILabel m_Leader = null;

    void Update()
    {
        m_Level.text = "Lv : " + GlobalValue.Instance.m_Level.ToString("F0");
        m_Hp.text = "MaxHP : " + GlobalValue.Instance.m_MaxHP.ToString("F0");
        m_Exp.text = "Exp : " + GlobalValue.Instance.m_ExpPoint.ToString("F0") + " / " + GlobalValue.Instance.m_NeedExp.ToString("F0");
        m_Money.text = "Money : " + GlobalValue.Instance.m_Money.ToString("F0");

        m_Attack.text = "공격력 : " + GlobalValue.Instance.m_AttackDmg.ToString("F0");

        m_Leader.text = "통솔력 : " + GlobalValue.Instance.m_LeaderShip.ToString("F0");
    }

    public void OnClickCancel()
    {
        UIManager.Instance.HideUI(eUIType.Pf_UI_Popup_Status);
    }
    
    	
}
