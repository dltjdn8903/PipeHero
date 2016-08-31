using UnityEngine;
using System.Collections;

public class EffectTemplate
{
    string m_Key = string.Empty;
    string m_ResourceName = string.Empty;
    eEffectType m_EffectType = eEffectType.EFFECT_ONESHOT;
    float m_Time = 0.0f;
    float m_AdditionTime = 0.0f;

    public string Key
    {
        get { return m_Key; }
        set { m_Key = value; }
    }

    public eEffectType EffectType
    {
        get { return m_EffectType; }
        set { m_EffectType = value; }
    }

    public float Time
    {
        get { return m_Time; }
        set { m_Time = value; }
    }
    public float AdditionTime
    {
        get { return m_AdditionTime; }
        set { m_AdditionTime = value; }
    }

    public string ResourceName
    {
        get{ return m_ResourceName; }
        set { m_ResourceName = value; }
    }

    public EffectTemplate(string strKey, SimpleJSON.JSONNode nodeData)
    {
        m_Key = strKey;
        m_ResourceName = nodeData["RESOURCE_NAME"].Value;
        m_EffectType = (eEffectType)nodeData["TYPE"].AsInt;
        m_Time = nodeData["TIME"].AsInt;
        m_AdditionTime = nodeData["ADDION_TIME"].AsInt;
    }
}
