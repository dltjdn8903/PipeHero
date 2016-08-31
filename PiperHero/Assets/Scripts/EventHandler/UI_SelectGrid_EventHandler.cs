using UnityEngine;
using System.Collections;

public class UI_SelectGrid_EventHandler : MonoBehaviour
{
    [SerializeField]
    UISprite m_SelectSpriteImage = null;

    [SerializeField]
    public UILabel m_IndexNum = null;

    [SerializeField]
    public UILabel KeyName = null;

    int m_IndexCount = 0;
    
    

    void Update()
    {
        GameObject m_selectgrid = UI_Soldier_EventHandler.Instance.m_selectgrid;
        m_selectgrid.GetComponent<UIGrid>().Reposition(); //그리드 재정렬
        UIScrollView scrollView = m_selectgrid.GetComponentInParent<UIScrollView>();
        scrollView.SetDragAmount(0, 0, false);
    }

    public void OnClickImage()
    {
        UIManager.Instance.DeletelistSelect(int.Parse(m_IndexNum.text));
        UIManager.Instance.SoldierCount -= 1;

        if (KeyName.text == "KNIGHT")
            GlobalValue.Instance.m_KnightCount -= 1;
        else if (KeyName.text == "SAINT")
            GlobalValue.Instance.m_SaintCount -= 1;
        else if (KeyName.text == "MAGICIAN")
            GlobalValue.Instance.m_MagicianCount -= 1;
        else if (KeyName.text == "MAGICIAN_ICE")
            GlobalValue.Instance.m_MagicianIceCount -= 1;

    }

}
