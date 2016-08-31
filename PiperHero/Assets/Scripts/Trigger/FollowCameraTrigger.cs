using UnityEngine;
using System.Collections;

public class FollowCameraTrigger : BaseTrigger
{
    [SerializeField]
    Camera m_Camera = null;

    public Camera CAMERA
    {
        get { return m_Camera; }
        set { m_Camera = value; }
    }

    public override void InitTrigger()
    {
    }

    public override eTriggerResultType UpdateTrigger()
    {
        eTriggerResultType resultType = base.UpdateTrigger();
        if (resultType != eTriggerResultType.TRIGGER_RESULT_ING)
            return resultType;

        if( COMPLETE == true )
        {
            BaseObject mainObject = TriggerManager.Instance.MAIN_OBJECT;

            if( mainObject != null )
            {
                m_Camera.transform.position = mainObject.SelfTransform.position + (m_Camera.transform.forward * -4);
            }

            return eTriggerResultType.TRIGGER_RESULT_ING;
        }

        TriggerManager.Instance.MAIN_CAMERA = m_Camera;
        COMPLETE = true;

        return eTriggerResultType.TRIGGER_RESULT_ING;
    }

}
