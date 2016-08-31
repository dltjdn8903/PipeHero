using UnityEngine;
using System.Collections;

public class ObjectSpawnTrigger : BaseTrigger
{
    [SerializeField]
    GameObject m_prefabObject = null;

    public GameObject PREFAB_OBJECT
    {
        get { return m_prefabObject; }
        set { m_prefabObject = value; }
    }

    public override void InitTrigger()
    {

    }
    public override eTriggerResultType UpdateTrigger()
    {
        eTriggerResultType resultType = base.UpdateTrigger();
        if (resultType != eTriggerResultType.TRIGGER_RESULT_ING)
            return resultType;

        if( m_prefabObject != null )
        {
            GameObject.Instantiate(m_prefabObject, SelfTransform.position, SelfTransform.rotation);
        }

        return eTriggerResultType.TRIGGER_RESULT_END;
    }

}
