using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Shop_EventHandler : MonoBehaviour
{
    [SerializeField]
    UILabel m_iCountLabel = null;
    [SerializeField]
    GameObject m_prefabGridItem = null;
    [SerializeField]
    UIGrid m_grid = null;

    int iCount = 0;
    int m_nCurrentSelectItem = 1;
    

    void Awake()
    {
        _RefreshGrid();
    }

    ItemTemplateData m_ItemTemplateData = null;
    public ItemTemplateData ITEM_SHOP_TEMPLATE
    {
        set
        {
            m_ItemTemplateData = value;
            _Refresh();
        }
    }

    void _Refresh()
    {
        if (m_ItemTemplateData == null)
            return;
    }


    void _RefreshGrid()
    {
        GameObject gridObject = m_grid.gameObject;
     
        List<ItemTemplateData> listTemplateData = ItemManager.Instance.GetItemList(m_nCurrentSelectItem);
        
        for (int i = 0; i < listTemplateData.Count; ++i)
        {               
            GameObject gridItemObject = NGUITools.AddChild(gridObject, m_prefabGridItem);

            UI_ItemGrid_EventHandler eventHandler = gridItemObject.GetComponentInChildren<UI_ItemGrid_EventHandler>();
            eventHandler.ITEM_TEMPLATE = listTemplateData[i];

            ITEM_SHOP_TEMPLATE = listTemplateData[i];
        }

        m_grid.Reposition(); //그리드 재정렬

        UIScrollView scrollView = m_grid.GetComponentInParent<UIScrollView>();
        scrollView.SetDragAmount(0, 0, false); //(스크롤뷰)드래그 위치를 초기화

    }
    

    public void OnClickUp()
    {
        ++iCount;
        if (iCount >= 10)
            iCount = 10;

        SetValue(iCount);
    }

    public void OnClickDown()
    {
        --iCount;
        if (iCount <= 0)
            iCount = 0;

        SetValue(iCount);         
    }    

    public void OnClickBuy()
    {
        if( GlobalValue.Instance.m_Money < 300 * iCount)
        {
            UIManager.Instance.ShowUI(eUIType.Pf_Status_Label);
            Debug.Log("You Don't Buy it!!");
            return;
        }

        GlobalValue.Instance.m_ItemCount += iCount;

        UILabel m_ItemCount = GameObject.FindWithTag("ItemCount").GetComponent<UILabel>();
        m_ItemCount.text = string.Format("{0}", GlobalValue.Instance.m_ItemCount);

        if (iCount != 0)
            GlobalValue.Instance.m_Money -= 300 * iCount; // m_ItemTemplateData.ITEM_MONEY * iCount;

        UILabel m_Money = GameObject.FindWithTag("Money").GetComponent<UILabel>();
        m_Money.text = string.Format("{0}", GlobalValue.Instance.m_Money);

        iCount = 0;
        SetValue(iCount);
                
    }

    public void OnClickCancel()
    {
        UIManager.Instance.HideUI(eUIType.Pf_UI_Shop);
    }

    public void SetValue(int iValue)
    {
        m_iCountLabel.text = string.Format("{0}", iValue);
    }

}
