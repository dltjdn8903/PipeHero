using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using SimpleJSON;

public class SoldierManager : BaseManager<SoldierManager>
{
    Dictionary<string, SoldierTemplateData> m_dicTemplateData = new Dictionary<string, SoldierTemplateData>();

    public int SoldierCount = 0;

    void Awake()
    {
        TextAsset soldierText = Resources.Load<TextAsset>("SOLDIER_TEMPLATE");
        if (soldierText != null)
        {
            JSONClass rootnodeData = JSON.Parse(soldierText.text) as JSONClass;
            if (rootnodeData != null)
            {
                JSONClass soldierTemplateNode = rootnodeData["SOLDIER_TEMPLATE"] as JSONClass;
                if (soldierTemplateNode != null)
                {
                    foreach (KeyValuePair<string, JSONNode> templateNode in soldierTemplateNode)
                    {
                           
                        m_dicTemplateData.Add(templateNode.Key, new SoldierTemplateData(templateNode.Key, templateNode.Value));
                        SoldierCount = soldierTemplateNode.Count;
                    }
                }
            }
        }
        

    }


    //키 값을 넣어 딕셔너리에 있는 값중 하나를 찾아서 값을 가져온다.
    public SoldierTemplateData GetTemplate(string strTemplateKey)
    {
        SoldierTemplateData templateData = null;
        m_dicTemplateData.TryGetValue(strTemplateKey, out templateData);
                
        return templateData;
    }

    public List<SoldierTemplateData> GetTemplateValueList()
    {
        List<SoldierTemplateData> templateData = new List<SoldierTemplateData>();

        foreach( KeyValuePair<string,SoldierTemplateData> ValueData in m_dicTemplateData)
        {
            templateData.Add(ValueData.Value);
        }
        return templateData;
    }
    

}
