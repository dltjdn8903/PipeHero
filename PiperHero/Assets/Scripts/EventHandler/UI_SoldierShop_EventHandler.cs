using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_SoldierShop_EventHandler : MonoBehaviour
{    
    [SerializeField]
    GameObject m_prefabTableSoldier = null;
    [SerializeField]
    UITable m_Table = null;

    void Awake()
    {
        _RefreshGrid();
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
        
    }


    void _RefreshGrid() // 그리드 오브젝트 추가 및 위치 세팅
    {
        GameObject TableObject = m_Table.gameObject;
        
        List<SoldierTemplateData> listTemplateData = SoldierManager.Instance.GetTemplateValueList();
        for (int i = 0; i < listTemplateData.Count; ++i)
        {
            GameObject gridSoldierObject = NGUITools.AddChild(TableObject, m_prefabTableSoldier);
            UI_SoldierShopGrid_EventHandler eventHandler = gridSoldierObject.GetComponentInChildren<UI_SoldierShopGrid_EventHandler>();
            eventHandler.SOLDIER_TEMPLATE = listTemplateData[i];
            
        }

        m_Table.Reposition(); //테이블 재정렬

    }
    
    public void OnClickCancel()
    {
        UIManager.Instance.HideUI(eUIType.Pf_UI_SoldierShop);
    }

}
