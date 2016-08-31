using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using SimpleJSON;

public class ItemManager : BaseManager<ItemManager>
{
    Dictionary<int, List<ItemTemplateData>> m_dicItemTemplate = new Dictionary<int, List<ItemTemplateData>>(); //딕셔너리와 리스트 같이 씀
    List<ItemTemplateData> listTemplate = new List<ItemTemplateData>();
    void Awake()
    {
        TextAsset ItemText = Resources.Load<TextAsset>("ITEM_TEMPLATE");
        if (ItemText != null)
        {
            JSONClass nodeData = JSON.Parse(ItemText.text) as JSONClass;
            if (nodeData != null)
            {
                JSONArray ItemTemplateNode = nodeData["ITEM_TEMPLATE"] as JSONArray;
                for (int i = 0; i < ItemTemplateNode.Count; ++i)
                {
                    ItemTemplateData tData = new ItemTemplateData(ItemTemplateNode[i]); //JSON 0번째 받아오기
                    int nItem = tData.ITEM_ID;

                    listTemplate.Add(tData);
                    m_dicItemTemplate.Add(nItem, listTemplate);

                }

            }
        }

    }
    
      
    public List<ItemTemplateData> GetItemList(int nItem)
    {
        List<ItemTemplateData> listTemplateData = null;
        m_dicItemTemplate.TryGetValue(nItem, out listTemplateData);

        return listTemplateData;

    }


}
