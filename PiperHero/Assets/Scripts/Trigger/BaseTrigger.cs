using UnityEngine;
using System.Collections;

using System.Collections.Generic;


public class BaseTrigger : CacheObject
{
    [SerializeField]
    eTriggerInvokeType m_InvokeType = eTriggerInvokeType.INVOKE_IMMEDIATE;

    [SerializeField]
    float m_InvokeTime = 0.0f;

    [SerializeField]
    List<BaseTrigger> m_listCallTrigger = new List<BaseTrigger>();

    float m_ElapsedTime = 0.0f;

    public bool CALL { get; set; }
    public bool COLLISION { get; set; }
    public bool COMPLETE { get; set; }

    public float INVOKE_TIME
    {
        get { return m_InvokeTime; }
        set { m_InvokeTime = value; }
    }

    public List<BaseTrigger> LIST_CALL_TRIGGER
    {
        get { return m_listCallTrigger; }
    }

    public eTriggerInvokeType INVOKE_TYPE
    {
        get { return m_InvokeType; }
        set { m_InvokeType = value; }
    }

    void Awake()
    {
        TriggerManager.Instance.AddTrigger(this);
    }

    virtual public void InitTrigger()
    {

    }

    virtual public eTriggerResultType UpdateTrigger()
    {
        eTriggerResultType resultType = eTriggerResultType.TRIGGER_RESULT_WAIT;

        switch( m_InvokeType )
        {
            case eTriggerInvokeType.INVOKE_IMMEDIATE:
                {
                    resultType = eTriggerResultType.TRIGGER_RESULT_ING;
                }
                break;

            case eTriggerInvokeType.INVOKE_CALL:
                {
                    if (CALL == true)
                        resultType = eTriggerResultType.TRIGGER_RESULT_ING;
                }
                break;

            case eTriggerInvokeType.INVOKE_COLLISION:
                {
                    if (COLLISION == true)
                        resultType = eTriggerResultType.TRIGGER_RESULT_ING;
                }
                break;
        }

        if( resultType == eTriggerResultType.TRIGGER_RESULT_ING )
        {
            m_ElapsedTime += Time.smoothDeltaTime;
            if (m_ElapsedTime < m_InvokeTime)
            {
                resultType = eTriggerResultType.TRIGGER_RESULT_WAIT;
            }
        }

        return resultType;
    }

    public void CallTrigger()
    {
        for( int i = 0; i < m_listCallTrigger.Count; ++i )
        {
            m_listCallTrigger[i].CALL = true;
        }
    }
}
