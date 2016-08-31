using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_SoldierInfo_EventHandler : MonoBehaviour
{
    [SerializeField]
    UISprite m_CharacterImage = null;

    [SerializeField]
    UILabel m_CharacterName = null;

    [SerializeField]
    UILabel m_CharacterInfo = null;

    [SerializeField]
    UILabel m_CharacterPrice = null;

    [SerializeField]
    GameObject m_TempGameObject = null;

    [SerializeField]
    GameObject m_LabelGameObject = null;

    bool b_chackBuy = false;

    SoldierTemplateData m_SoldierTemplateData = null;
    public SoldierTemplateData SOLDIER_TEMPLATE
    {
        set
        {
            m_SoldierTemplateData = value;
            _Refresh();

        }
    }

    void _Refresh()
    {
        
        if (m_SoldierTemplateData == null)
            return;
        
        m_CharacterImage.spriteName = m_SoldierTemplateData.SOLDIERINFO_IMAGE.ToString();
        m_CharacterName.text = m_SoldierTemplateData.SOLDIER_NAME.ToString();
        m_CharacterInfo.text = m_SoldierTemplateData.SOLDIER_INFO.ToString();
        m_CharacterPrice.text = m_SoldierTemplateData.SOLDIER_PRICE.ToString();

    }



    public void OnClickBuy()
    {
        GlobalValue.Instance.m_Money -= m_SoldierTemplateData.SOLDIER_PRICE;

        if (m_SoldierTemplateData.KEY_NAME == "KNIGHT")
            GlobalValue.Instance.Have_KnightCount += 1;
        else if (m_SoldierTemplateData.KEY_NAME == "SAINT")
            GlobalValue.Instance.Have_SaintCount += 1;
        else if (m_SoldierTemplateData.KEY_NAME == "MAGICIAN")
            GlobalValue.Instance.Have_MagicianCount += 1;
        else if (m_SoldierTemplateData.KEY_NAME == "MAGICIAN_ICE")
            GlobalValue.Instance.Have_MagicianIceCount += 1;


        if (GlobalValue.Instance.m_Money < 0)
        {
            // 구매 실패시 다시 원래대로
            GlobalValue.Instance.m_Money += m_SoldierTemplateData.SOLDIER_PRICE;
            if (m_SoldierTemplateData.KEY_NAME == "KNIGHT")
                GlobalValue.Instance.Have_KnightCount -= 1;
            else if (m_SoldierTemplateData.KEY_NAME == "SAINT")
                GlobalValue.Instance.Have_SaintCount -= 1;
            else if (m_SoldierTemplateData.KEY_NAME == "MAGICIAN")
                GlobalValue.Instance.Have_MagicianCount -= 1;
            else if (m_SoldierTemplateData.KEY_NAME == "MAGICIAN_ICE")
                GlobalValue.Instance.Have_MagicianIceCount -= 1;

            // 구매 실패 보드 띄우기
            UIPlayTween testPlayTween = m_TempGameObject.AddComponent<UIPlayTween>();
            testPlayTween.tweenTarget = m_LabelGameObject;
            testPlayTween.playDirection = AnimationOrTween.Direction.Toggle;
            testPlayTween.ifDisabledOnPlay = AnimationOrTween.EnableCondition.EnableThenPlay;
            testPlayTween.disableWhenFinished = AnimationOrTween.DisableCondition.DisableAfterReverse;
            
            Debug.Log("You Don't Buy it!!");
            return;
        }

        
        UILabel m_Money = GameObject.FindWithTag("Money").GetComponent<UILabel>();
        m_Money.text = string.Format("{0}", GlobalValue.Instance.m_Money);

        UIManager.Instance.HideUI(eUIType.Pf_UI_SoldierInfo);

    }


    public void OnClickCancel()
    {
        UIManager.Instance.HideUI(eUIType.Pf_UI_SoldierInfo);
    }

}


