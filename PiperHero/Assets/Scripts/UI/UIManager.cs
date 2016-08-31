using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class UIManager : BaseManager< UIManager >
{
    //++ 리스트 추가
    List<Transform> m_listSelect = new List<Transform>();
    public int SoldierCount = 0;

    Dictionary<eUIType, GameObject> m_dicUI = new Dictionary<eUIType, GameObject>();
    Dictionary<BaseObject, List<BaseBoard>> m_dicBoard = new Dictionary<BaseObject, List<BaseBoard>>();
    GameObject m_BoardUI = null;

    public void ShowLoadingUI( float fValue )
    {
        GameObject loadingUI = _GetUI(eUIType.Pf_Ui_Loading);
        if (loadingUI.activeSelf == false)
            loadingUI.SetActive(true);

        UI_Loading_EventHandler eventHandler = loadingUI.GetComponentInChildren<UI_Loading_EventHandler>();
        eventHandler.SetValue(fValue);
    }

    public void HideLoadingUI()
    {
        GameObject loadingUI = _GetUI(eUIType.Pf_Ui_Loading);
        if (loadingUI.activeSelf == true)
            loadingUI.SetActive(false);
    }

    public GameObject ShowUI( eUIType uiType )
    {
        GameObject showObject = _GetUI(uiType);
        if (showObject != null && showObject.activeSelf == false)
            showObject.SetActive(true);

        return showObject;
    }

    public void HideUI( eUIType uiType )
    {
        GameObject hideObject = _GetUI(uiType);
        if (hideObject != null && hideObject.activeSelf == true)
            hideObject.SetActive(false);
    }

    GameObject _GetUI( eUIType uiType )
    {
        if( m_dicUI.ContainsKey(uiType) == true )
        {
            return m_dicUI[uiType];
        }

        GameObject makeUI = null;
        GameObject prefabUI = Resources.Load<GameObject>(uiType.ToString("F"));
        if( prefabUI != null )
        {
            makeUI = NGUITools.AddChild(null, prefabUI);
            m_dicUI.Add(uiType, makeUI);
            DontDestroyOnLoad(makeUI);

            makeUI.SetActive(false);
        }
        return makeUI;
    }


    //++ 병영에 용병을 어떤것을 추가할것인지 리스트에 추가
    public void AddlistSelect(Transform listData)
    {
        m_listSelect.Add(listData);
        
    }

    //++ 리스트 클리어, 선택했던 용병들 클리어
    public void ClearlistSelect()
    {
        for (int i = UIManager.Instance.SoldierCount-1; i >= 0; --i)
        {
            DeletelistSelect(i);
        }
        m_listSelect.Clear();
        UIManager.Instance.SoldierCount = 0;

        GlobalValue.Instance.m_KnightCount = 0;
        GlobalValue.Instance.m_SaintCount = 0;
        GlobalValue.Instance.m_MagicianCount = 0;
        GlobalValue.Instance.m_MagicianIceCount = 0;
    }

    Transform DeleteTF;
    public void DeletelistSelect(int listData)
    {
        DeleteTF = m_listSelect[listData];
        m_listSelect.Remove(DeleteTF);

        GameObject asdf = DeleteTF.gameObject;
        Destroy(asdf);

        for (int i = 0; i < m_listSelect.Count; ++i)
        {
            m_listSelect[i].gameObject.GetComponentInChildren<UI_SelectGrid_EventHandler>().m_IndexNum.text = i.ToString();
        }
        
    }

}
