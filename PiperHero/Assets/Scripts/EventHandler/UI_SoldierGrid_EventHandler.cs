using UnityEngine;
using System.Collections;

public class UI_SoldierGrid_EventHandler : MonoBehaviour
{   
    [SerializeField]
    UILabel m_LevelLabel = null;    

    [SerializeField]
    UISprite m_SpriteImage = null;

    [SerializeField]
    UILabel m_CountLabel = null;


    float fLevel = 1;



    // 용병선택시 추가되는 그리드 이미지
    [SerializeField]
    GameObject m_prefabGridSelect = null;

    

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
                
        m_SpriteImage.spriteName = m_SoldierTemplateData.SOLDIER_IMAGE.ToString();
                
    }

    void Update()
    {
        if (m_SoldierTemplateData.KEY_NAME == "KNIGHT")
            m_CountLabel.text = "보유용병 : " + GlobalValue.Instance.Have_KnightCount;
        else if (m_SoldierTemplateData.KEY_NAME == "SAINT")
            m_CountLabel.text = "보유용병 : " + GlobalValue.Instance.Have_SaintCount;
        else if (m_SoldierTemplateData.KEY_NAME == "MAGICIAN")
            m_CountLabel.text = "보유용병 : " + GlobalValue.Instance.Have_MagicianCount;
        else if (m_SoldierTemplateData.KEY_NAME == "MAGICIAN_ICE")
            m_CountLabel.text = "보유용병 : " + GlobalValue.Instance.Have_MagicianIceCount;
    }
    
    public void OnClickLevelUp()
    {
        ++fLevel;
        if (fLevel >= 10)
            fLevel = 10;

        SetValue(fLevel);
    }

    public void OnClickSelect()
    {
        _RefreshGrid();
            
    }

    public void SetValue(float fValue)
    {
        m_LevelLabel.text = string.Format("Lv. {0}", fValue);
    }


    GameObject gridSoldierObject = null;
    void _RefreshGrid()
    {
        GameObject m_selectgrid = UI_Soldier_EventHandler.Instance.m_selectgrid;
        GameObject gridObject = m_selectgrid.gameObject;


        Transform spriteImage = m_prefabGridSelect.transform.Find("SelectSprite");
        spriteImage.GetComponent<UISprite>().spriteName = m_SoldierTemplateData.SOLDIERHEAD_IMAGE.ToString();
        Transform SoldierCount = m_prefabGridSelect.transform.Find("Label_SoldierCount");
        SoldierCount.GetComponent<UILabel>().text = UIManager.Instance.SoldierCount.ToString();
        Transform SoldierKeyName = m_prefabGridSelect.transform.Find("Label_Keyname");
        SoldierKeyName.GetComponent<UILabel>().text = m_SoldierTemplateData.KEY_NAME;


        if (GlobalValue.Instance.m_LeaderShip > UIManager.Instance.SoldierCount )
        {
            if (m_SoldierTemplateData.KEY_NAME == "KNIGHT" && GlobalValue.Instance.Have_KnightCount > GlobalValue.Instance.m_KnightCount)
            {
                // 태그가 "SelectGrid"인 Grid에 SelectGrid 오브젝트를 추가한다.
                gridSoldierObject = NGUITools.AddChild(gridObject, m_prefabGridSelect);                
                //++ 병영에 어떤 용병을 선택했는지 리스트에 추가
                UIManager.Instance.AddlistSelect(gridSoldierObject.transform);

                GlobalValue.Instance.m_KnightCount += 1;
                UIManager.Instance.SoldierCount += 1;
            }
            else if (m_SoldierTemplateData.KEY_NAME == "SAINT" && GlobalValue.Instance.Have_SaintCount > GlobalValue.Instance.m_SaintCount )
            {
                gridSoldierObject = NGUITools.AddChild(gridObject, m_prefabGridSelect);                
                UIManager.Instance.AddlistSelect(gridSoldierObject.transform);

                GlobalValue.Instance.m_SaintCount += 1;
                UIManager.Instance.SoldierCount += 1;
            }
            else if (m_SoldierTemplateData.KEY_NAME == "MAGICIAN" && GlobalValue.Instance.Have_MagicianCount > GlobalValue.Instance.m_MagicianCount )
            {
                gridSoldierObject = NGUITools.AddChild(gridObject, m_prefabGridSelect);                
                UIManager.Instance.AddlistSelect(gridSoldierObject.transform);

                GlobalValue.Instance.m_MagicianCount += 1;
                UIManager.Instance.SoldierCount += 1;
            }
            else if (m_SoldierTemplateData.KEY_NAME == "MAGICIAN_ICE" && GlobalValue.Instance.Have_MagicianIceCount > GlobalValue.Instance.m_MagicianIceCount )
            {
                gridSoldierObject = NGUITools.AddChild(gridObject, m_prefabGridSelect);                
                UIManager.Instance.AddlistSelect(gridSoldierObject.transform);

                GlobalValue.Instance.m_MagicianIceCount += 1;
                UIManager.Instance.SoldierCount += 1;
            }

        }


        m_selectgrid.GetComponent<UIGrid>().Reposition(); //그리드 재정렬

        UIScrollView scrollView = m_selectgrid.GetComponentInParent<UIScrollView>();
        scrollView.SetDragAmount(0, 0, false); //(스크롤뷰)드래그 위치를 초기화
    }

}
