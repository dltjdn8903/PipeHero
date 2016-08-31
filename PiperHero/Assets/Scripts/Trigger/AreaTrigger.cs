using UnityEngine;
using System.Collections;

using System.Collections.Generic;


public class AreaTrigger : BaseTrigger
{
    [SerializeField]
    eAreaType m_AreaType = eAreaType.AREA_TOWN;

    [SerializeField]
    AudioClip m_AudioClip = null;

    public eAreaType AREA_TYPE
    {
        get { return m_AreaType; }
        set { m_AreaType = value; }
    }

    public AudioClip AUDIO_CLIP
    {
        get { return m_AudioClip; }
        set { m_AudioClip = value; }
    }

    void OnTriggerEnter(Collider other)
    {
        if (GlobalValue.Instance.m_AreaType == m_AreaType)
            return;

        GameObject colObject = other.gameObject;
        BaseObject colBaseObject = colObject.GetComponent<BaseObject>();

        if (colBaseObject == TriggerManager.Instance.MAIN_OBJECT)
        {
            GlobalValue.Instance.EnterArea(m_AreaType, m_AudioClip);
        }
    }


}
