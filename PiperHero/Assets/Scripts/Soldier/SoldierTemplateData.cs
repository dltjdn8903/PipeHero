using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eSoldierTemplateType
{
    KEY_NAME     ,
        
    SOLDIER_IMAGE,
    SOLDIERHEAD_IMAGE,
    SOLDIERINFO_IMAGE,

    SOLDIER_NAME,
    SOLDIER_INFO,
    SOLDIER_PRICE,

    SOLDIER_COUNT,
}

public class SoldierTemplateData
{
    Dictionary<eSoldierTemplateType, string> m_dicData = new Dictionary<eSoldierTemplateType, string>();

    public string m_strKey = string.Empty;

    string m_keyName = string.Empty;    
    string m_soldier = string.Empty;
    string m_soldierHead = string.Empty;
    string m_soldierInfo = string.Empty;    
    string m_name = string.Empty;
    string m_info = string.Empty;
    int m_price = 0;

    public string KEY_NAME { get { return m_keyName; } }    
    public string SOLDIER_IMAGE { get { return m_soldier; } }
    public string SOLDIERHEAD_IMAGE { get { return m_soldierHead; } }
    public string SOLDIERINFO_IMAGE { get { return m_soldierInfo; } }
    public string SOLDIER_NAME { get { return m_name; } }
    public string SOLDIER_INFO { get { return m_info; } }
    public int SOLDIER_PRICE { get { return m_price; } }


    public SoldierTemplateData(string strKey, SimpleJSON.JSONNode nodeData)
    {
        m_strKey = strKey;

        m_keyName = nodeData["KEY_NAME"];
        
        m_soldier = nodeData["SOLDIER_IMAGE"];
        m_soldierHead = nodeData["SOLDIERHEAD_IMAGE"];
        m_soldierInfo = nodeData["SOLDIERINFO_IMAGE"];

        m_name = nodeData["SOLDIER_NAME"];
        m_info = nodeData["SOLDIER_INFO"];
        m_price = nodeData["SOLDIER_PRICE"].AsInt;


        for (int i = 0; i < (int)eSoldierTemplateType.SOLDIER_COUNT; ++i)
        {
            eSoldierTemplateType Data = (eSoldierTemplateType)i;
            string valueData = nodeData[Data.ToString("F")];

            if (valueData != null)
                TemplateData(Data, valueData);
            
        }


    }

    public void TemplateData(eSoldierTemplateType data, string valueData)
    {
        string PrevValue = "";
        m_dicData.TryGetValue(data, out PrevValue);

        m_dicData[data] = PrevValue;
        
    }
    

}
