using UnityEngine;
using System.Collections;

public class UI_SoldierShopGrid_EventHandler : MonoBehaviour {

    [SerializeField]
    UISprite m_SoldierSprite = null;

    [SerializeField]
    UILabel m_SoldierMoney = null;


    string test = string.Empty;

    //[SerializeField]
    GameObject showObject = null;

    void Awake()
    {
        showObject = GameObject.FindWithTag("SoldierInfo");
    }


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
        
        m_SoldierMoney.text = m_SoldierTemplateData.KEY_NAME.ToString();
        m_SoldierSprite.spriteName = m_SoldierTemplateData.SOLDIERHEAD_IMAGE.ToString();
       
    }


    public void OnClickSelectSoldier()
    {
        UIManager.Instance.ShowUI(eUIType.Pf_UI_SoldierInfo);
        UI_SoldierInfo_EventHandler eventHandler = GameObject.FindWithTag("SoldierInfo").GetComponentInChildren<UI_SoldierInfo_EventHandler>();
        
        //+ 캐릭터를 선택하면, 선택한 캐릭터가 어떤것인지 선택한 캐릭터의 키 값을 넣어서, selectImage에 선택한 캐릭터의 값을 알게한다.
        SoldierTemplateData listTemplateData = SoldierManager.Instance.GetTemplate(m_SoldierTemplateData.KEY_NAME.ToString());
        eventHandler.SOLDIER_TEMPLATE = listTemplateData;
    }

}
