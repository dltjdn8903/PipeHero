using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Soldier_EventHandler : UISingleton<UI_Soldier_EventHandler>
{

    [SerializeField]
    GameObject m_prefabGridSoldier = null;
    [SerializeField]
    UIGrid m_soldiergrid = null;

    [SerializeField]
    public GameObject m_prefabGridSelect = null;
    [SerializeField]
    public GameObject m_selectgrid = null;

    [SerializeField]
    UILabel Label_Leadership = null;
    

    void Awake()
    {
        _RefreshGrid();
    }

    void Update()
    {
        Label_Leadership.text = UIManager.Instance.SoldierCount.ToString() + " / " + GlobalValue.Instance.m_LeaderShip;
    }

    void _RefreshGrid() // 그리드 오브젝트 추가 및 위치 세팅
    {
        GameObject gridObject = m_soldiergrid.gameObject;
                
        List<SoldierTemplateData> listTemplateData = SoldierManager.Instance.GetTemplateValueList();
        for (int i = 0; i < listTemplateData.Count; ++i)
        {
            GameObject gridSoldierObject = NGUITools.AddChild(gridObject, m_prefabGridSoldier);
            UI_SoldierGrid_EventHandler eventHandler = gridSoldierObject.GetComponentInChildren<UI_SoldierGrid_EventHandler>();
            eventHandler.SOLDIER_TEMPLATE = listTemplateData[i];
        }

        m_soldiergrid.Reposition(); //그리드 재정렬

        UIScrollView scrollView = m_soldiergrid.GetComponentInParent<UIScrollView>();
        scrollView.SetDragAmount(0, 0, false); //(스크롤뷰)드래그 위치를 초기화
    }

    public void OnClickCancel()
    {
        Debug.Log("KnightCount : " + GlobalValue.Instance.m_KnightCount + " SaintCount  : " + GlobalValue.Instance.m_SaintCount +
                    " MagicianCount    : " + GlobalValue.Instance.m_MagicianCount + " MagicianIceCount : " + GlobalValue.Instance.m_MagicianIceCount);

        UIManager.Instance.HideUI(eUIType.Pf_UI_Soldier);
    }

    public void OnClickSetting()
    {
        // 배치확정하지 않은 용병 리스트 클리어
        if (UIManager.Instance.SoldierCount > 0)
            UIManager.Instance.ClearlistSelect();

        //UIManager.Instance.HideUI(eUIType.Pf_UI_Soldier);
    }

}
