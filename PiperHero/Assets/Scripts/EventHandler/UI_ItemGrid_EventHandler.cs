 using UnityEngine;
using System.Collections;

public class UI_ItemGrid_EventHandler : MonoBehaviour
{
    [SerializeField]
    UILabel m_ItemName = null;
    [SerializeField]
    UILabel m_ItemExplain = null;
    [SerializeField]
    UISprite m_ItemImage = null;
        

    int ColorChange_Item = 0;

    ItemTemplateData m_ItemTemplateData = null;   

    public ItemTemplateData ITEM_TEMPLATE
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

        m_ItemImage.spriteName = m_ItemTemplateData.ITEM_IMAGE.ToString();
        m_ItemName.text = m_ItemTemplateData.ITEM_NAME.ToString();
        m_ItemExplain.text = m_ItemTemplateData.ITEM_MONEY.ToString();
        
    }

    public void OnClickItem()
    {
        if (ColorChange_Item == 0)
        {
            ColorChange_Item = 1;
        }
        else if (ColorChange_Item == 1)
        {
            ColorChange_Item = 0;
        }

    }


}
