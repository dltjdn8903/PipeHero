using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Lobby_EventHandler : MonoBehaviour
{
    [SerializeField]
    GameObject m_prefabGridItem = null;

    [SerializeField]
    UIGrid m_grid = null;

    int m_nCurrentSelectEpisode = 1;

    void Awake()
    {
        _RefreshGrid();
    }

    void _RefreshGrid()
    {
        List<Transform> listDelete = new List<Transform>();

        
        for( int i = 0; i < m_grid.transform.childCount; ++i )
        {
            Transform childTransform = m_grid.transform.GetChild(i);
            listDelete.Add(childTransform);
        }

        for( int i = 0; i < listDelete.Count; ++i )
        {
            listDelete[i].parent = null;
            Destroy(listDelete[i].gameObject);
        }

        GameObject gridObject = m_grid.gameObject;
        List<StageTemplateData> listTemplateData = StageManager.Instance.GetStageList(m_nCurrentSelectEpisode);
        for( int i = 0; i < listTemplateData.Count; ++i )
        {
            GameObject gridItemObject = NGUITools.AddChild(gridObject, m_prefabGridItem);

            UI_StageGrid_EventHandler eventHandler = gridItemObject.GetComponentInChildren<UI_StageGrid_EventHandler>();
            eventHandler.STAGE_TEMPLATE = listTemplateData[i];
        }

        m_grid.Reposition();

        UIScrollView scrollView = m_grid.GetComponentInParent<UIScrollView>();
        scrollView.SetDragAmount(0, 0, false);
    }

    public void OnClickPrevEpisode()
    {
        if (StageManager.Instance.IsValidEpisode(m_nCurrentSelectEpisode - 1) == false)
            return;

        --m_nCurrentSelectEpisode;
        _RefreshGrid();
    }

    public void OnClickNextEpisode()
    {
        if (StageManager.Instance.IsValidEpisode(m_nCurrentSelectEpisode + 1) == false)
            return;

        ++m_nCurrentSelectEpisode;
        _RefreshGrid();
    }

    public void OnClickIdle()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        observer.AI.AddNextAI(eAIStateType.AI_STATE_IDLE);
    }

    public void OnClickAttack1()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        observer.AI.AddNextAI(eAIStateType.AI_STATE_NORMALATTACK);
        observer.AI.AddNextAI(eAIStateType.AI_STATE_NORMALATTACK);
    }

    public void OnClickAttack2()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        observer.AI.AddNextAI(eAIStateType.AI_STATE_SKILL1);
    }

    public void OnClickRun()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Observer_Component observer = playerObject.GetComponent<Observer_Component>();
        observer.AI.AddNextAI(eAIStateType.AI_STATE_RUN);
    }




}
